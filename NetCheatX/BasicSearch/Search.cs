using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch
{
    static class Search
    {
        public const ulong SEARCH_BLOCK_SIZE = 0x10000;

        public enum SearchType
        {
            EqualTo,
            NotEqualTo,
            LessThan,
            LessThanOrEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo,
            ValueBetween
        }

        public static bool FirstScan(IPluginHost host, ISearchMethod method, Types.MemoryRange range, ref List<ISearchResult> result, SearchType type, bool signed, int align, byte[] param0, byte[] param1 = null)
        {
            bool pass = false;
            int progress = 0, lastProgress = 0;

            // Return false if param is invalid
            if (param0 == null || param0.Length == 0 || (type == SearchType.ValueBetween && (param0 == null || param1.Length < 2)))
                return false;
            // Return false if any of the other arguments are invalid
            if (host == null || method == null || result == null)
                return false;

            // Define total range
            ulong distance = range.stop - range.stop;

            // Define max block size
            ulong blockSize = SEARCH_BLOCK_SIZE;
            if (blockSize > distance)
                blockSize = distance;

            // Define block increment value
            // Subtract param0.Length-1 to ensure memory at end of blocks is scanned
            ulong blockInc = blockSize - (ulong)(param0.Length - 1);

            // Initialize our buffers
            byte[] block = new byte[blockSize];
            byte[] compare = new byte[param0.Length];

            // Reset progress bar to 0
            host.SetProgress(method, 0, "");

            // Loop through memory, grabbing blocks and comparing them
            for (ulong addr = range.start; addr < range.stop; addr += blockInc)
            {
                // Update progress bar
                progress = (int)((float)((float)(addr - range.start) / (float)distance));
                if (progress > lastProgress)
                {
                    lastProgress = progress;
                    host.SetProgress(method, progress, result.Count.ToString() + " results found");
                }

                // Update blockSize if range smaller than SEARCH_BLOCK_SIZE
                if (blockSize > (range.stop - addr))
                    blockSize = (range.stop - addr);

                // Grab block
                if (host.GetMemory(addr, ref block))
                {
                    for (int off = 0; off < (int)blockSize; off += align)
                    {
                        // Copy section for comparison
                        Array.Copy(block, off, compare, 0, param0.Length);

                        // Compare based on SearchType
                        switch (type)
                        {
                            case SearchType.EqualTo:
                                pass = NetCheatX.Core.Bitlogic.Compare.BAEqual(param0, compare);
                                break;
                            case SearchType.NotEqualTo:
                                pass = NetCheatX.Core.Bitlogic.Compare.BANotEqual(param0, compare);
                                break;
                            case SearchType.LessThan:
                                if (signed)
                                    pass = NetCheatX.Core.Bitlogic.Compare.BALessThanSigned(compare, param0);
                                else
                                    pass = NetCheatX.Core.Bitlogic.Compare.BALessThanUnsigned(compare, param0);
                                break;
                            case SearchType.LessThanOrEqualTo:
                                if (signed)
                                    pass = NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualSigned(compare, param0);
                                else
                                    pass = NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualUnsigned(compare, param0);
                                break;
                            case SearchType.GreaterThan:
                                if (signed)
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanSigned(compare, param0);
                                else
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanUnsigned(compare, param0);
                                break;
                            case SearchType.GreaterThanOrEqualTo:
                                if (signed)
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualSigned(compare, param0);
                                else
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualUnsigned(compare, param0);
                                break;
                            case SearchType.ValueBetween:
                                if (signed)
                                {
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualSigned(compare, param0) &
                                        NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualSigned(compare, param1);
                                }
                                else
                                {
                                    pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualUnsigned(compare, param0) &
                                        NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualUnsigned(compare, param1);
                                }
                                break;
                        }

                        // If passed then add the result
                        if (pass)
                            result.Add(new NetCheatX.Core.Basic.BasicSearchResult() { Address = addr + (ulong)off, Value = compare });
                    }
                }
            }

            // Finish progress
            host.SetProgress(method, 100, result.Count.ToString() + " results found");

            return true;
        }

        public static bool NextScan(IPluginHost host, ISearchMethod method, ref List<ISearchResult> result, SearchType type, bool signed, byte[] param0, byte[] param1 = null)
        {
            bool pass = false;
            int progress = 0, lastProgress = 0;
            ulong lastAddr = 0;
            NetCheatX.Core.Basic.BasicSearchResult resultEdit;

            // Return false if param is invalid
            if (param0 == null || param0.Length == 0 || (type == SearchType.ValueBetween && (param0 == null || param1.Length < 2)))
                return false;
            // Return false if any of the other arguments are invalid
            if (host == null || method == null || result == null || result.Count == 0)
                return false;

            // Initialize counters
            int resultSize = result.Count;
            int resultIndex = 0;
            int resultFound = 0;

            // Initialize our buffers
            byte[] block = new byte[SEARCH_BLOCK_SIZE];
            byte[] compare = new byte[param0.Length];

            // Reset progress bar to 0
            host.SetProgress(method, 0, "");

            for (int off = 0; off < result.Count; off++)
            {
                // Update progress bar
                progress = (int)((float)((float)resultIndex / (float)resultSize));
                if (progress > lastProgress)
                {
                    lastProgress = progress;
                    host.SetProgress(method, progress, resultFound.ToString() + " results found");
                }

                // Grab current result
                resultEdit = (NetCheatX.Core.Basic.BasicSearchResult)result[off];

                // Update block if address is out of block range
                if (resultEdit.Address > (lastAddr + SEARCH_BLOCK_SIZE - (ulong)(param0.Length - 1)) || off == 0)
                {
                    // Load block
                    lastAddr = result[off].Address;
                    host.GetMemory(lastAddr, ref block);
                }

                // Copy section for comparison
                Array.Copy(block, (int)(resultEdit.Address - lastAddr), compare, 0, param0.Length);

                // Compare based on SearchType
                switch (type)
                {
                    case SearchType.EqualTo:
                        pass = NetCheatX.Core.Bitlogic.Compare.BAEqual(param0, compare);
                        break;
                    case SearchType.NotEqualTo:
                        pass = NetCheatX.Core.Bitlogic.Compare.BANotEqual(param0, compare);
                        break;
                    case SearchType.LessThan:
                        if (signed)
                            pass = NetCheatX.Core.Bitlogic.Compare.BALessThanSigned(compare, param0);
                        else
                            pass = NetCheatX.Core.Bitlogic.Compare.BALessThanUnsigned(compare, param0);
                        break;
                    case SearchType.LessThanOrEqualTo:
                        if (signed)
                            pass = NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualSigned(compare, param0);
                        else
                            pass = NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualUnsigned(compare, param0);
                        break;
                    case SearchType.GreaterThan:
                        if (signed)
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanSigned(compare, param0);
                        else
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanUnsigned(compare, param0);
                        break;
                    case SearchType.GreaterThanOrEqualTo:
                        if (signed)
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualSigned(compare, param0);
                        else
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualUnsigned(compare, param0);
                        break;
                    case SearchType.ValueBetween:
                        if (signed)
                        {
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualSigned(compare, param0) &
                                NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualSigned(compare, param1);
                        }
                        else
                        {
                            pass = NetCheatX.Core.Bitlogic.Compare.BAGreaterThanOrEqualUnsigned(compare, param0) &
                                NetCheatX.Core.Bitlogic.Compare.BALessThanOrEqualUnsigned(compare, param1);
                        }
                        break;
                }

                if (!pass)
                {
                    // Clean up
                    resultEdit.Value = null;
                    result[off] = resultEdit;

                    // Remove
                    result.RemoveAt(off);

                    // Lower off so the next result is not skipped
                    off--;
                }
                else
                {
                    // Update result
                    resultEdit.Value = compare;
                    result[off] = resultEdit;

                    resultFound++;
                }

                resultIndex++;
            }

            return true;
        }

    }
}
