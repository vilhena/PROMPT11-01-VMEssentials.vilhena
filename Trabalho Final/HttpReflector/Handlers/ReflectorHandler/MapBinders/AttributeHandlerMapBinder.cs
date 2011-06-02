using System;
using System.Collections.Generic;
using HttpReflector.Contracts.Handler;
using HttpReflector.Handlers.Attributes;
using HttpReflector.Handlers.Exceptions;

namespace HttpReflector.Handlers.MapBinders
{
    public class AttributeHandlerMapBinder: IHandlerMapBinder
    {
        public void Bind(Dictionary<string, string> map, IHandler handler)
        {
            var hType = handler.GetType();
            

            foreach (var propertyInfo in hType.GetProperties())
            {
                var attrs = propertyInfo.GetCustomAttributes(typeof(HandlerMapAttribute), false);
                if (attrs.Length > 0)
                {
                    var mapper = (HandlerMapAttribute)attrs[0];
                    
                    if (!map.ContainsKey(mapper.Pattern))
                        throw new MapKeyNotFoundMapBinderException(mapper.Pattern);

                    propertyInfo.SetValue(handler, map[mapper.Pattern], new object[] {});
                }
            }

            
        }
    }
}
