using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Bitlogic
{
    /// <summary>
    /// Compare byte arrays.
    /// </summary>
    public static class Compare
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int memcmp(byte* b1, byte* b2, long count);

        private static unsafe int memcmp(byte[] b1, byte[] b2, long count, long startIndex)
        {
            fixed (byte* ptr1 = &b1[startIndex], ptr2 = &b2[startIndex])
                return memcmp(ptr1, ptr2, count);
        }


        /// <summary>
        /// Returns true if b1 and b2 contain the same elements.
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BAEqual(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            // Verify same length
            // Compare byte arrays with memcmp
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length, 0) == 0;
        }

        /// <summary>
        /// Returns true if b1 and b2 do not contain the same elements.
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BANotEqual(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            return !BAEqual(b1, b2);
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a lower value in array b1 (evaluated as a signed byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BALessThanSigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            // Smaller array holds smaller number
            if (b1.Length < b2.Length)
                return true;
            if (b1.Length > b2.Length)
                return false;
            if (b1.Length == 0)
                return false;

            if ((sbyte)b1[0] < (sbyte)b2[0])
                return true;

            return memcmp(b1, b2, b1.Length-1, 1) < 0;
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a lower value in array b1 (evaluated as a unsigned byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BALessThanUnsigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            // Smaller array holds smaller number
            if (b1.Length < b2.Length)
                return true;
            if (b1.Length > b2.Length)
                return false;
            if (b1.Length == 0)
                return false;

            return memcmp(b1, b2, b1.Length, 0) < 0;
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a lower or equal value in array b1 (evaluated as a signed byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BALessThanOrEqualSigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            // Smaller array holds smaller number
            if (b1.Length < b2.Length)
                return true;
            if (b1.Length > b2.Length)
                return false;
            if (b1.Length == 0)
                return false;

            if ((sbyte)b1[0] < (sbyte)b2[0])
                return true;

            int i = memcmp(b1, b2, b1.Length - 1, 1);

            // If b1 < b2
            // If b1 == b2 from 1 onward, make sure b1[0] == b2[0]
            if (i < 0)
                return true;
            if (i == 0)
                return b1[0] == b2[0];

            return false;
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a lower or equal value in array b1 (evaluated as a unsigned byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BALessThanOrEqualUnsigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            // Smaller array holds smaller number
            if (b1.Length < b2.Length)
                return true;
            if (b1.Length > b2.Length)
                return false;
            if (b1.Length == 0)
                return false;

            return memcmp(b1, b2, b1.Length, 0) <= 0;
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a greater value in array b1 (evaluated as a signed byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BAGreaterThanSigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            return !BALessThanOrEqualSigned(b1, b2);
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a greater value in array b1 (evaluated as a unsigned byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BAGreaterThanUnsigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            return !BALessThanOrEqualUnsigned(b1, b2);
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a greater or equal value in array b1 (evaluated as a signed byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BAGreaterThanOrEqualSigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            return !BALessThanSigned(b1, b2);
        }

        /// <summary>
        /// Returns true if the first byte that does not match in both byte arrays has a greater or equal value in array b1 (evaluated as a unsigned byte).
        /// </summary>
        /// <param name="b1">An array of bytes.</param>
        /// <param name="b2">An array of bytes.</param>
        public static bool BAGreaterThanOrEqualUnsigned(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
                return false;

            return !BALessThanUnsigned(b1, b2);
        }

    }
}
