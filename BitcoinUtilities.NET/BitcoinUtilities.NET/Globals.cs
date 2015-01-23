using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.BitcoinUtilities
{
    public static class Globals
    {
        public static Byte[] ProdAddressVersion = {0};
        public static Byte[] TestAddressVersion = {111};
        public static Byte[] ProdDumpKeyVersion = {128};
        public static Byte[] TestDumpKeyVersion = {239};

        public enum NORM_FORM
        {
            NormalizationOther = 0,
            NormalizationC = 0x1,
            NormalizationD = 0x2,
            NormalizationKC = 0x5,
            NormalizationKD = 0x6
        };
    }
}
