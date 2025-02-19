﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace pvcms
{
    /// <summary>
    /// PEM converter
    /// </summary>
    public class PemConverter
    {
        public const string CertificateTag = "CERTIFICATE";
        public const string CertificateRequestTag = "CERTIFICATE REQUEST";
        public const string PublicKeyTag = "PUBLIC KEY";
        public const string PrivateKeyTag = "PRIVATE KEY";
        private const string Pattern = "-{5}BEGIN [A-Z0-9 ]+-{5}([a-zA-Z0-9=+\\/\\n\\r]+)-{5}END [A-Z0-9 ]+-{5}";

        /// <summary>
        /// Converts PEM to byte[]
        /// </summary>
        /// <param name="pem">PEM string</param>
        /// <returns></returns>
        public static byte[] Decode(string pem)
        {
            if (pem is null)
            {
                throw new ArgumentNullException(nameof(pem));
            }

            var matches = Regex.Matches(pem, Pattern);

            foreach (Match item in matches)
            {
                if (item.Success)
                {
                    var base64 = item.Groups[1].Value
                        .Replace("\r", "")
                        .Replace("\n", "");
                    return Convert.FromBase64String(base64);
                }
            }
            throw new Exception("Incorrect PEM string");
        }

        public static byte[] Decode(byte[] data)
        {
            return Decode(Encoding.ASCII.GetString(data));
        }

        /// <summary>
        /// Converts byte[] to PEM
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string Encode(byte[] rawData, string tag)
        {
            #region Check arguments
            if (rawData is null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("message", nameof(tag));
            }
            #endregion

            var base64 = Convert.ToBase64String(rawData);
            string sliced;
            int offset = 0;
            var rows = new List<string>();
            while (offset < base64.Length)
            {
                if (base64.Length - offset < 64)
                {
                    sliced = base64.Substring(offset);
                }
                else
                {
                    sliced = base64.Substring(offset, 64);
                    offset += 64;
                }
                if (sliced.Length != 0)
                {
                    rows.Add(sliced);
                    if (sliced.Length < 64)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            var stringRows = rows.ToArray();

            var upperCaseTag = tag.ToUpper();
            return $"-----BEGIN {upperCaseTag}-----\n{string.Join("\n", stringRows)}\n-----END {upperCaseTag}-----";
        }

        public static bool IsPEM(string data)
        {
            return Regex.IsMatch(data, Pattern);
        }

        public static bool IsPEM(byte[] data)
        {
            return IsPEM(Encoding.ASCII.GetString(data));
        }

    }
}
