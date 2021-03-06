﻿using System;

namespace Mapper.Configuration
{
    public class MapperMappingException : Exception
    {
        public MapperMappingException(string message, Exception innerExeption):base(message,innerExeption)
        {
            
        }

        public MapperMappingException(string message, string propertyName) : base(message)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
    }
}