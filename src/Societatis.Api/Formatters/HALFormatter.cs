namespace Societatis.Api.Formatters
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using Newtonsoft.Json;
    using Societatis.HAL;
    using Societatis.Misc;
    
    public class HALFormatter : JsonOutputFormatter
    {
        public HALFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool)
            :base (serializerSettings, charPool)
        {
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/hal+json"));
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/hal+xml"));

            this.SupportedEncodings.Add(Encoding.UTF8);
            this.SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type type)
        {
            type.ThrowIfNull(nameof(type));

            bool canWrite = false;
            if (typeof(Resource).IsAssignableFrom(type) 
                || typeof(IEnumerable<Resource>).IsAssignableFrom(type))
            {
                canWrite = base.CanWriteType(type);
            }

            return canWrite;
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            
        }

    }
}