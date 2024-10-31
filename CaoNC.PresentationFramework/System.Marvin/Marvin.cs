using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CaoNC.System.Marvin
{
    internal static class Marvin
    {
        public static ulong DefaultSeed { get; } = GenerateSeed();


        /// <summary>
        /// Compute a Marvin hash and collapse it into a 32-bit hash.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComputeHash32(ReadOnlySpan<byte> data, ulong seed)
        {
            return ComputeHash32(ref MemoryMarshal.GetReference(data), (uint)data.Length, (uint)seed, (uint)(seed >> 32));
        }

        /// <summary>
        /// Compute a Marvin hash and collapse it into a 32-bit hash.
        /// </summary>
        public static int ComputeHash32(ref byte data, uint count, uint p0, uint p1)
        {
            uint num;
            if (count < 8)
            {
                if (count < 4)
                {
                    num = ((!BitConverter.IsLittleEndian) ? 2147483648u : 128u);
                    if ((count & (true ? 1u : 0u)) != 0)
                    {
                        num = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref data, (IntPtr)((uint)count & (uint)2u));
                        if (BitConverter.IsLittleEndian)
                        {
                            num |= 0x8000u;
                        }
                        else
                        {
                            num <<= 24;
                            num |= 0x800000u;
                        }
                    }
                    if ((count & 2u) != 0)
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            num <<= 16;
                            num |= System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ushort>(ref data);
                        }
                        else
                        {
                            num |= System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ushort>(ref data);
                            num = RotateLeft(num, 16);
                        }
                    }
                    goto IL_00bd;
                }
            }
            else
            {
                uint num2 = count / 8u;
                do
                {
                    p0 += System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(ref data);
                    uint num3 = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref data, (IntPtr)(uint)4u));
                    Block(ref p0, ref p1);
                    p0 += num3;
                    Block(ref p0, ref p1);
                    data = ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref data, (IntPtr)(uint)8u);
                }
                while (--num2 != 0);
                if ((count & 4) == 0)
                {
                    goto IL_006a;
                }
            }
            p0 += System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(ref data);
            Block(ref p0, ref p1);
            goto IL_006a;
            IL_006a:
            num = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref data, (IntPtr)((uint)count & (uint)7u)), -4));
            count = ~count << 3;
            if (BitConverter.IsLittleEndian)
            {
                num >>= 8;
                num |= 0x80000000u;
                num >>= (int)(count & 0x1F);
            }
            else
            {
                num <<= 8;
                num |= 0x80u;
                num <<= (int)(count & 0x1F);
            }
            goto IL_00bd;
            IL_00bd:
            p0 += num;
            Block(ref p0, ref p1);
            Block(ref p0, ref p1);
            return (int)(p1 ^ p0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Block(ref uint rp0, ref uint rp1)
        {
            uint num = rp0;
            uint num2 = rp1;
            num2 ^= num;
            num = RotateLeft(num, 20);
            num += num2;
            num2 = RotateLeft(num2, 9);
            num2 ^= num;
            num = RotateLeft(num, 27);
            num += num2;
            num2 = RotateLeft(num2, 19);
            rp0 = num;
            rp1 = num2;
        }

        private unsafe static ulong GenerateSeed()
        {
            byte[] array = new byte[8];
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(array);
                fixed (byte* ptr = array)
                {
                    return *(ulong*)ptr;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RotateLeft(uint value, int shift)
        {
            return (value << shift) | (value >> 32 - shift);
        }
    }
}
