﻿using System;
namespace RockyBus.Message
{
    class MessageScanRules
    {
        private Func<Type, bool> isAnEvent = type => type.FullName.EndsWith("Event");
        private Func<Type, bool> isACommand = type => type.FullName.EndsWith("Command");

        public MessageScanRules DefineEventScanRuleWith(Func<Type, bool> isAnEvent)
        {
            this.isAnEvent = isAnEvent;
            return this;
        }

        public MessageScanRules DefineCommandScanRuleWith(Func<Type, bool> isACommand)
        {
            this.isACommand = isACommand;
            return this;
        }

        public bool IsAnEvent(Type type) => isAnEvent(type);
        public bool IsACommand(Type type) => isACommand(type);
    }
}
