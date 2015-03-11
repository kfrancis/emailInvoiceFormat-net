#region License, Terms and Conditions
// InvoiceEmailHeader.cs
// Author: Kori Francis <twitter.com/djbyter>

//The MIT License (MIT)
//Copyright (c) 2015 Kori Francis
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

namespace emailInvoiceFormat
{
    #region Imports
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    #endregion

    public static class HeaderExtensions
    {
        /// <summary>
        /// Serialize the header object into the X-Header header value
        /// </summary>
        /// <param name="header">The object to serialize</param>
        /// <param name="shouldFormat">Should the json be formatted, usually not</param>
        /// <param name="serializationSettings">Optional serialization settings</param>
        /// <returns>The json representation of the X-Header object</returns>
        public static string AsJSON(this InvoiceEmailHeader header, bool shouldFormat = false, JsonSerializerSettings serializationSettings = null)
        {
            if (serializationSettings == null)
            {
                return JsonConvert.SerializeObject(header, shouldFormat ? Formatting.None : Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(header, shouldFormat ? Formatting.None : Formatting.Indented, serializationSettings);
            }
        }

        /// <summary>
        /// Useful when parsing or testing, deserialize the json back into the X-Header object
        /// </summary>
        /// <param name="json">The json to deserialize</param>
        /// <returns>The X-Header object</returns>
        public static InvoiceEmailHeader FromJSON(this string json)
        {
            return JsonConvert.DeserializeObject<InvoiceEmailHeader>(json);
        }
    }

    /// <summary>
    /// From http://email-invoice-format.org/
    /// </summary>
    /// <remarks>Used http://json2csharp.com/ to generate this base of this class</remarks>
    public class InvoiceEmailHeader
    {
        public static string HeaderName = "X-Invoice";

        /// <summary>
        /// The only existing version of this format to date
        /// </summary>
        /// <example>1.0</example>
        [JsonProperty("version", Required = Required.Always)]
        public string Version { get { return "1.0"; } }

        /// <summary>
        /// The name of the company issuing the invoice
        /// </summary>
        /// <example>Service Provider</example>
        [JsonProperty("issuer", Required = Required.Always)]
        public string Issuer { get; set; }

        /// <summary>
        /// Filename of the attachment in the email body. Refers to the filename part of the Content-disposition header.
        /// </summary>
        /// <example>bill.pdf</example>
        [JsonProperty("filename", Required = Required.Always)]
        public string Filename { get; set; }

        /// <summary>
        /// Service provider internal reference for the invoice
        /// </summary>
        /// <example>XF4321-89</example>
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        /// <summary>
        /// ISO 8601 formatted timestamp. Date of invoice issuance.
        /// </summary>
        /// <example>2015-01-10T00:00:00+01:00</example>
        [JsonProperty("invoice_date", Required = Required.Always)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// ISO 8601 formatted timestamp. Date when invoice is due.
        /// </summary>
        /// <example>2015-01-30T12:00:00+01:00</example>
        [JsonProperty("due_date")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Flag to tell if the invoice is paid
        /// </summary>
        /// <example>true</example>
        [JsonProperty("paid", Required = Required.Always)]
        public bool Paid { get; set; }

        /// <summary>
        /// ISO 8601 formatted timestamp. Date when the invoice was paid.
        /// </summary>
        /// <example>2015-01-10T08:35:12+01:00</example>
        [JsonProperty("paid_date")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? PaidDate { get; set; }

        /// <summary>
        /// Amount of the invoice
        /// </summary>
        /// <example>39.90</example>
        [JsonProperty("amount", Required = Required.Always)]
        public decimal Amount { get; set; }

        /// <summary>
        /// ISO 4217 3-letter code of the currency of the invoice
        /// </summary>
        /// <example>USD</example>
        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }

        /// <summary>
        /// A direct link to the page where the bill can be paid, if not already paid
        /// </summary>
        /// <example>https://…/paybill?id=XF4321-89</example>
        [JsonProperty("payUrl")]
        public string PayURL { get; set; }
    }
}
