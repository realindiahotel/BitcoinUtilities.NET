BitcoinUtilities.NET
====================

This is a C# PCL Library designed to contain any shared functionality used by various other Bitcoin modules of mine such a KeyCore.NET and BIP39.NET

This PCL targets Universal Apps (Windows 8.1/Windows Phone 8.1) and .NET 4.5.1 ONLY. Support for Windows 8 and Windows Phone 8/8.1 Silverlight has been removed.

Also this is using a PCL version of BouncyCastle to provide cryptographic functionality.

Initially this project started from the Bitcoinsharp project however I have decided to break things up into modules that can be modified/updated independent of eachother.

I have removed all references to the Microsoft crypto stuff and am using BouncyCastle instead. This is mostly because PCL projects can't access the System.Security.Cryptography part of the .Net framework, and also because I want to be somewhat standardised and I know it is what BitcoinJ thus MultiBit is using etc. The BouncyCastle.dll used/provided is a special PCL compatible variant, so if you think you are going to be really clever and put the latest BouncyCastle.dll in the project perish the thought now.

This code is put out for all to use for free, I don't have a great deal of Bitcoin but I'd really like some so if you find yourself using this code in a commercial implementation and you feel you are going to make some money from it, I’d appreciate it if you fling me some bitcoin to 1ETQjMkR1NNh4jwLuN5LxY7bHC9PUPSV thanks :)
