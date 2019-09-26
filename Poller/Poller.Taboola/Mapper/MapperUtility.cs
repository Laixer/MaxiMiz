using System;

using ApprovalStateInternal = Maximiz.Model.Enums.ApprovalState;
using ApprovalStateExternal = Poller.Taboola.Model.ApprovalState;
using System.Text.RegularExpressions;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// Contains some utility functions for our data mappers to be able to deal
    /// with external enums. Entity specific cases are placed within the partial
    /// classes of each mapper.
    /// </summary>
    internal class MapperUtility
    {
        /// <summary>
        /// Converts an approval state to a <see cref="ApprovalStateExternal"/>
        /// upper case string that the Taboola API understands.
        /// </summary>
        /// <remarks>Default: <see cref="ApprovalStateExternal.Pending"/>.</remarks>
        /// <param name="enumInternal">The internal approval state</param>
        /// <returns>The upper case string for external approval state</returns>
        internal string ApprovalStateToString(ApprovalStateInternal enumInternal)
        {
            var external = ApprovalStateExternal.Pending;

            switch (enumInternal)
            {
                case ApprovalStateInternal.Unknown:
                    external = ApprovalStateExternal.Pending;
                    break;
                case ApprovalStateInternal.Submitted:
                    external = ApprovalStateExternal.Pending;
                    break;
                case ApprovalStateInternal.Approved:
                    external = ApprovalStateExternal.Approved;
                    break;
                case ApprovalStateInternal.Pending:
                    external = ApprovalStateExternal.Pending;
                    break;
                case ApprovalStateInternal.Rejected:
                    external = ApprovalStateExternal.Rejected;
                    break;
            }

            return ToUpperString(external);
        }

        /// <summary>
        /// Maps a string representing a <see cref="ApprovalStateExternal"/> back
        /// to the enum from upper case, to then map it onto our own model.
        /// </summary>
        /// <remarks>Default: <see cref="ApprovalStateInternal.Unknown"/>.</remarks>
        /// <param name="input">Upper case string</param>
        /// <returns>Internal approval state model</returns>
        internal ApprovalStateInternal ApprovalStateToInternal(string input)
        {
            var external = FromUpperString(input, ApprovalStateExternal.Pending);
            switch (external)
            {
                case ApprovalStateExternal.Approved:
                    return ApprovalStateInternal.Approved;
                case ApprovalStateExternal.Pending:
                    return ApprovalStateInternal.Pending;
                case ApprovalStateExternal.Rejected:
                    return ApprovalStateInternal.Rejected;
                default:
                    return ApprovalStateInternal.Unknown;
            }
        }

        /// <summary>
        /// Converts an enum to all caps. This is because Taboola uses upper 
        /// case to denote these enums, while we use lower case.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="input">Input enum</param>
        /// <returns>Upper case string</returns>
        internal string ToUpperString<T>(T input)
            where T : Enum
        {
            var split = Regex.Split(input.ToString(), @"(?<!^)(?=[A-Z])");

            if (split.Length > 1)
            {
                string result = "";
                for (int i = 0; i < split.Length - 1; i++)
                {
                    result += split[i].ToUpper() + "_";
                }
                result += split[split.Length - 1].ToUpper();
                return result;
            }
            else
            {
                return input.ToString().ToUpper();
            }
        }

        /// <summary>
        /// Attempt to convert an enum from all caps snake case.
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="input">The input string, IN_THIS_FORMAT</param>
        /// <param name="defaultReturn">If we can't parse we return this</param>
        /// <returns>Converted enum</returns>
        internal TEnum FromUpperString<TEnum>(string input, TEnum defaultReturn)
            where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(input)) { return defaultReturn; }

            var words = input.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            var pascal = "";
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].Trim('_');
                pascal += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
            }

            if (Enum.TryParse(pascal, out TEnum result))
            {
                return result;
            }
            else
            {
                return defaultReturn;
            }
        }

    }
}
