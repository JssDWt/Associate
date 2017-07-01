using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

public class HALFormatter : JsonOutputFormatter
{
    public HALFormatter()
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