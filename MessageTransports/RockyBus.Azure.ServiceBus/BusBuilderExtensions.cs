﻿using System;
namespace RockyBus
{
    public static class BusBuilderExtensions
    {
        public static BusBuilder UseAzureServiceBus(
            this BusBuilder busBuilder, string connectionString, Action<AzureServiceBusConfiguration> configuration) 
        => busBuilder.UseMessageTransport(new AzureServiceBusTransport(connectionString, configuration));
    }
}
