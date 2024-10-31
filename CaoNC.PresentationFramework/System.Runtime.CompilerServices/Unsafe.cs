using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace CaoNC.System.Runtime.CompilerServices
{
    public static class Unsafe
    {
        /// <summary>Reads a value of type <typeparamref name="T">T</typeparamref> from the given location.</summary>
        /// <param name="source">The location to read from.</param>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static T Read<T>(void* source)
        {
            return *(T*)source;
        }

        /// <summary>Reads a value of type <typeparamref name="T">T</typeparamref> from the given location
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="source">The location to read from.</param>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static T ReadUnaligned<T>(void* source)
        {
            return Unsafe.ReadUnaligned<T>(source);
        }

        /// <summary>Reads a value of type <typeparamref name="T">T</typeparamref> from the given location
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="source">The location to read from.</param>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static T ReadUnaligned<T>(ref byte source)
        {
            return Unsafe.ReadUnaligned<T>(ref source);
        }

        /// <summary>Writes a value of type <typeparamref name="T">T</typeparamref> to the given location.</summary>
        /// <param name="destination">The location to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <typeparam name="T">The type of value to write.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void Write<T>(void* destination, T value)
        {
            *(T*)destination = value;
        }

        /// <summary>Writes a value of type <typeparamref name="T">T</typeparamref> to the given location
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="destination">The location to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <typeparam name="T">The type of value to write.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void WriteUnaligned<T>(void* destination, T value)
        {
            Unsafe.WriteUnaligned(destination, value);
        }

        /// <summary>Writes a value of type <typeparamref name="T">T</typeparamref> to the given location
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="destination">The location to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <typeparam name="T">The type of value to write.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static void WriteUnaligned<T>(ref byte destination, T value)
        {
            Unsafe.WriteUnaligned(ref destination, value);
        }

        /// <summary>Copies a value of type <typeparamref name="T">T</typeparamref> to the given location.</summary>
        /// <param name="destination">The location to copy to.</param>
        /// <param name="source">A reference to the value to copy.</param>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void Copy<T>(void* destination, ref T source)
        {
            *(T*)destination = source;
        }

        /// <summary>Copies a value of type <typeparamref name="T">T</typeparamref> to the given location.</summary>
        /// <param name="destination">The location to copy to.</param>
        /// <param name="source">A pointer to the value to copy.</param>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void Copy<T>(ref T destination, void* source)
        {
            destination = *(T*)source;
        }

        /// <summary>Returns a pointer to the given by-ref parameter.</summary>
        /// <param name="value">The object whose pointer is obtained.</param>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <returns>A pointer to the given value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void* AsPointer<T>(ref T value)
        {
            return Unsafe.AsPointer(ref value);
        }

        /// <summary>Returns the size of an object of the given type parameter.</summary>
        /// <typeparam name="T">The type of object whose size is retrieved.</typeparam>
        /// <returns>The size of an object of type <typeparamref name="T">T</typeparamref>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static int SizeOf<T>()
        {
            return sizeof(T);
        }

        /// <summary>Copies bytes from the source address to the destination address.</summary>
        /// <param name="destination">The destination address to copy to.</param>
        /// <param name="source">The source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void CopyBlock(void* destination, void* source, uint byteCount)
        {
            // IL cpblk instruction
            Unsafe.CopyBlock(destination, source, byteCount);
        }

        /// <summary>Copies bytes from the source address to the destination address.</summary>
        /// <param name="destination">The destination address to copy to.</param>
        /// <param name="source">The source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static void CopyBlock(ref byte destination, ref byte source, uint byteCount)
        {
            // IL cpblk instruction
            Unsafe.CopyBlock(ref destination, ref source, byteCount);
        }

        /// <summary>Copies bytes from the source address to the destination address 
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="destination">The destination address to copy to.</param>
        /// <param name="source">The source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
        {
            // IL cpblk instruction
            Unsafe.CopyBlockUnaligned(destination, source, byteCount);
        }

        /// <summary>Copies bytes from the source address to the destination address 
        /// without assuming architecture dependent alignment of the addresses.</summary>
        /// <param name="destination">The destination address to copy to.</param>
        /// <param name="source">The source address to copy from.</param>
        /// <param name="byteCount">The number of bytes to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static void CopyBlockUnaligned(ref byte destination, ref byte source, uint byteCount)
        {
            // IL cpblk instruction
            Unsafe.CopyBlockUnaligned(ref destination, ref source, byteCount);
        }

        /// <summary>Initializes a block of memory at the given location with a given initial value.</summary>
        /// <param name="startAddress">The address of the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize the block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void InitBlock(void* startAddress, byte value, uint byteCount)
        {
            // IL initblk instruction
            Unsafe.InitBlock(startAddress, value, byteCount);
        }

        /// <summary>Initializes a block of memory at the given location with a given initial value.</summary>
        /// <param name="startAddress">The address of the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize the block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static void InitBlock(ref byte startAddress, byte value, uint byteCount)
        {
            // IL initblk instruction
            Unsafe.InitBlock(ref startAddress, value, byteCount);
        }

        /// <summary>Initializes a block of memory at the given location with a given initial value 
        /// without assuming architecture dependent alignment of the address.</summary>
        /// <param name="startAddress">The address of the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize the block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
        {
            // IL initblk instruction
            Unsafe.InitBlockUnaligned(startAddress, value, byteCount);
        }

        /// <summary>Initializes a block of memory at the given location with a given initial value 
        /// without assuming architecture dependent alignment of the address.</summary>
        /// <param name="startAddress">The address of the start of the memory block to initialize.</param>
        /// <param name="value">The value to initialize the block to.</param>
        /// <param name="byteCount">The number of bytes to initialize.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
        {
            // IL initblk instruction
            Unsafe.InitBlockUnaligned(ref startAddress, value, byteCount);
        }

        /// <summary>Casts the given object to the specified type.</summary>
        /// <param name="o">The object to cast.</param>
        /// <typeparam name="T">The type which the object will be cast to.</typeparam>
        /// <returns>The original object, casted to the given type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static T As<T>(object o) where T : class
        {
            return (T)o;
        }

        /// <summary>Reinterprets the given location as a reference to a value of type <typeparamref name="T">T</typeparamref>.</summary>
        /// <param name="source">The location of the value to reference.</param>
        /// <typeparam name="T">The type of the interpreted location.</typeparam>
        /// <returns>A reference to a value of type <typeparamref name="T">T</typeparamref>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static ref T AsRef<T>(void* source)
        {
            return ref *(T*)source;
        }

        /// <summary>Reinterprets the given reference as a reference to a value of type <typeparamref name="TTo">TTo</typeparamref>.</summary>
        /// <param name="source">The reference to reinterpret.</param>
        /// <typeparam name="TFrom">The type of reference to reinterpret..</typeparam>
        /// <typeparam name="TTo">The desired type of the reference.</typeparam>
        /// <returns>A reference to a value of type <typeparamref name="TTo">TTo</typeparamref>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref TTo As<TFrom, TTo>(ref TFrom source)
        {
            return ref Unsafe.As<TFrom, TTo>(ref source);
        }

        /// <summary>Adds an element offset to the given reference.</summary>
        /// <param name="source">The reference to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the addition of offset to pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T Add<T>(ref T source, int elementOffset)
        {
            return ref Unsafe.Add(ref source, elementOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void* Add<T>(void* source, int elementOffset)
        {
            return (byte*)source + (long)elementOffset * (long)sizeof(T);
        }

        /// <summary>Adds an element offset to the given reference.</summary>
        /// <param name="source">The reference to add the offset to.</param>
        /// <param name="elementOffset">The offset to add.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the addition of offset to pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T Add<T>(ref T source, IntPtr elementOffset)
        {
            return ref Unsafe.Add(ref source, elementOffset);
        }

        /// <summary>Adds a byte offset to the given reference.</summary>
        /// <param name="source">The reference to add the offset to.</param>
        /// <param name="byteOffset">The offset to add.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the addition of byte offset to pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
        {
            return ref Unsafe.AddByteOffset(ref source, byteOffset);
        }

        /// <summary>Subtracts an element offset from the given reference.</summary>
        /// <param name="source">The reference to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the subraction of offset from pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T Subtract<T>(ref T source, int elementOffset)
        {
            return ref Unsafe.Subtract(ref source, elementOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public unsafe static void* Subtract<T>(void* source, int elementOffset)
        {
            return (byte*)source - (long)elementOffset * (long)sizeof(T);
        }

        /// <summary>Subtracts an element offset from the given reference.</summary>
        /// <param name="source">The reference to subtract the offset from.</param>
        /// <param name="elementOffset">The offset to subtract.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the subraction of offset from pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T Subtract<T>(ref T source, IntPtr elementOffset)
        {
            return ref Unsafe.Subtract(ref source, elementOffset);
        }

        /// <summary>Subtracts a byte offset from the given reference.</summary>
        /// <param name="source">The reference to subtract the offset from.</param>
        /// <param name="byteOffset"></param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>A new reference that reflects the subraction of byte offset from pointer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset)
        {
            return ref Unsafe.SubtractByteOffset(ref source, byteOffset);
        }

        /// <summary>Determines the byte offset from origin to target from the given references.</summary>
        /// <param name="origin">The reference to origin.</param>
        /// <param name="target">The reference to target.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>Byte offset from origin to target i.e. <paramref name="target">target</paramref> - <paramref name="origin">origin</paramref>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static IntPtr ByteOffset<T>(ref T origin, ref T target)
        {
            return Unsafe.ByteOffset(target: ref target, origin: ref origin);
        }

        /// <summary>Determines whether the specified references point to the same location.</summary>
        /// <param name="left">The first reference to compare.</param>
        /// <param name="right">The second reference to compare.</param>
        /// <typeparam name="T">The type of reference.</typeparam>
        /// <returns>true if <paramref name="left">left</paramref> and <paramref name="right">right</paramref> point to the same location; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static bool AreSame<T>(ref T left, ref T right)
        {
            return Unsafe.AreSame(ref left, ref right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
        {
            return Unsafe.IsAddressGreaterThan(ref left, ref right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Runtime.Versioning.NonVersionable]
        public static bool IsAddressLessThan<T>(ref T left, ref T right)
        {
            return Unsafe.IsAddressLessThan(ref left, ref right);
        }
    }
}
