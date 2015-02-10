/*
 * Copyright 2011 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Bitcoin.BitcoinUtilities
{
    /// <summary>
    /// Represents an elliptic curve keypair that we own and can use for signing transactions. Currently,
    /// Bouncy Castle is used. In future this may become an interface with multiple implementations using different crypto
    /// libraries. The class also provides a static method that can verify a signature with just the public key.
    /// </summary>

    public class EcKeyPair
    {
        private static readonly ECDomainParameters _ecParams;

        private static readonly SecureRandom _secureRandom;

        static EcKeyPair()
        {
            // All clients must agree on the curve to use by agreement. BitCoin uses secp256k1.
            var @params = SecNamedCurves.GetByName("secp256k1");
            _ecParams = new ECDomainParameters(@params.Curve, @params.G, @params.N, @params.H);
            _secureRandom = new SecureRandom();
        }

        private readonly BigInteger _priv;
        private readonly byte[] _pub;
        private readonly bool _isCompressedPub;

        /// <summary>
        /// Generates an entirely new keypair.
        /// </summary>
        public EcKeyPair(bool compressedPublicKey=true)
        {
            _isCompressedPub = compressedPublicKey;
            var generator = new ECKeyPairGenerator();
            var keygenParams = new ECKeyGenerationParameters(_ecParams, _secureRandom);
            generator.Init(keygenParams);
            var keypair = generator.GenerateKeyPair();
            var privParams = (ECPrivateKeyParameters) keypair.Private;
            var pubParams = (ECPublicKeyParameters) keypair.Public;
            _priv = privParams.D;
            // The public key is an encoded point on the elliptic curve. It has no meaning independent of the curve.

            _pub = pPublicKeyFromPrivate(_priv, _isCompressedPub);
            
        }

        /// <summary>
        /// Creates an ECKey given only the private key in a universlly friendly byte array form. This works because
        /// EC public keys are derivable from their private keys by doing a multiply with the generator value.
        /// </summary>
        public EcKeyPair(byte[] privKey, bool compressedPublicKey=true)
        {
            _priv = Utilities.NewPositiveBigInteger(privKey);
            _pub = pPublicKeyFromPrivate(_priv, compressedPublicKey);
            _isCompressedPub = compressedPublicKey;
        }

        /// <summary>
        /// Returns the raw public key bytes from private key bytes
        /// </summary>
        /// <param name="privateKey">The private key bytes to derive public key from</param>
        /// <param name="compressedPublicKey">Force the public key to be compressed, default is true</param>
        /// <returns>public key bytes</returns>
        public static byte[] GetPublicKeyBytesFromPrivateKeyBytes(byte[] privateKey, bool compressedPublicKey=true)
        {
            return pPublicKeyFromPrivate(Utilities.NewPositiveBigInteger(privateKey), compressedPublicKey);
        }

        /// <summary>
        /// Derive the public key by doing a point multiply of G * priv.
        /// </summary>
        private static byte[] pPublicKeyFromPrivate(Org.BouncyCastle.Math.BigInteger privKey, bool compressedPublicKey)
        {
            if (!compressedPublicKey)
            {
                //not compressed public key
                return _ecParams.G.Multiply(privKey).GetEncoded();
            }

            //below is for compressed public key
            int Y = _ecParams.G.Multiply(privKey).Y.ToBigInteger().IntValue;

            byte b;

            if (Y % 2 == 0)
            {
                b = 2;
            }
            else
            {
                b = 3;
            }

            byte[] pub = _ecParams.G.Multiply(privKey).GetEncoded().Take(33).ToArray();
            pub[0]=b;
            return pub;
        }       

        /// <summary>
        /// Gets the raw public key value. This appears in transaction scriptSigs. Note that this is <b>not</b> the same
        /// as the pubKeyHash/address.
        /// </summary>
        public byte[] PublicKey
        {
            get { return _pub; }
        }

        public byte[] PrivateKey
        {
            get { return pGetPrivateKeyBytes(); }
        }

        /// <summary>
        /// Gets the status of compression of the public key
        /// </summary>
        public bool IsCompressedPublicKey
        {
            get
            {
                return _isCompressedPub;
            }
        }
        
        public override string ToString()
        {
            var b = new StringBuilder();
            b.Append("pub:").Append(Utilities.BytesToHexString(_pub));
            b.Append(" priv:").Append(Utilities.BytesToHexString(_priv.ToByteArray()));
            b.Append(" ispubkeycompressed:").Append(_isCompressedPub);

            return b.ToString();
        }

        /// <summary>
        /// Calculates an ECDSA signature in DER format for the given input hash. Note that the input is expected to be
        /// 32 bytes long.
        /// </summary>
        public byte[] Sign(byte[] input)
        {
            var signer = new ECDsaSigner();
            var privKey = new ECPrivateKeyParameters(_priv, _ecParams);
            signer.Init(true, privKey);
            var sigs = signer.GenerateSignature(input);
            // What we get back from the signer are the two components of a signature, r and s. To get a flat byte stream
            // of the type used by BitCoin we have to encode them using DER encoding, which is just a way to pack the two
            // components into a structure.
            using (var bos = new MemoryStream())
            {
                var seq = new DerSequenceGenerator(bos);
                seq.AddObject(new DerInteger(sigs[0]));
                seq.AddObject(new DerInteger(sigs[1]));
                seq.Close();
                return bos.ToArray();
            }
        }

        /// <summary>
        /// Verifies the given ASN.1 encoded ECDSA signature against a hash using the public key.
        /// </summary>
        /// <param name="data">Hash of the data to verify.</param>
        /// <param name="signature">ASN.1 encoded signature.</param>
        /// <param name="pub">The public key bytes to use.</param>
        public static bool Verify(byte[] data, byte[] signature, byte[] pub)
        {
            var signer = new ECDsaSigner();
            var @params = new ECPublicKeyParameters(_ecParams.Curve.DecodePoint(pub), _ecParams);
            signer.Init(false, @params);
            DerInteger r;
            DerInteger s;
            using (var decoder = new Asn1InputStream(signature))
            {
                var seq = (DerSequence) decoder.ReadObject();
                r = (DerInteger) seq[0];
                s = (DerInteger) seq[1];
            }
            return signer.VerifySignature(data, r.Value, s.Value);
        }

        /// <summary>
        /// Verifies the given ASN.1 encoded ECDSA signature against a hash using the public key.
        /// </summary>
        /// <param name="data">Hash of the data to verify.</param>
        /// <param name="signature">ASN.1 encoded signature.</param>
        public bool Verify(byte[] data, byte[] signature)
        {
            return Verify(data, signature, _pub);
        }

        /// <summary>
        /// Returns a 32 byte array containing the private key.
        /// </summary>
        private byte[] pGetPrivateKeyBytes()
        {
            // Getting the bytes out of a BigInteger gives us an extra zero byte on the end (for signedness)
            // or less than 32 bytes (leading zeros).  Coerce to 32 bytes in all cases.
            var bytes = new byte[32];

            var privArray = _priv.ToByteArray();
            var privStart = (privArray.Length == 33) ? 1 : 0;
            var privLength = Math.Min(privArray.Length, 32);
            Array.Copy(privArray, privStart, bytes, 32 - privLength, privLength);

            return bytes;
        }        
    }
}