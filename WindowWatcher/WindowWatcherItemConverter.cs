﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WindowWatcher
{
    public class WindowWatcherItemConverter : JsonConverter<IWindowWatcherItem>
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override IWindowWatcherItem ReadJson( JsonReader reader, Type objectType, IWindowWatcherItem existingValue, bool hasExistingValue, JsonSerializer serializer )
        {
            JObject jObject = JObject.Load( reader );
            var mMsgToSend = jObject["MsgToSend"];

            IWindowWatcherItem windowWatcherItem = null;
            if( jObject["MsgToSend"] != null )
            {
                windowWatcherItem = (WindowCreateWatcher.WindowCreateWatcherItem)jObject.ToObject<WindowCreateWatcher.WindowCreateWatcherItem>();
            }
            else
            {
                windowWatcherItem = jObject.ToObject<WindowWatcherItem>();
            }

            return windowWatcherItem;
            //
        }

        public override void WriteJson( JsonWriter writer, IWindowWatcherItem value, JsonSerializer serializer )
        {
            throw new NotImplementedException();
        }

    }

}
