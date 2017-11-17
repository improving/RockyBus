using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RockyBus.DemoMessages.Commands;
using RockyBus.DemoMessages.Events;
using RockyBus.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RockyBus.UnitTests.Message
{
    [TestClass]
    public class BusMessagesTests
    {
        BusMessages _busMessages;
        MessageScanRules _messageScanRules;
        Type[] _expectedEventTypes;
        Type[] _expectedCommandTypes;

        [TestInitialize]
        public void Setup()
        {
            given_these_event_types(new[] {typeof(Cat), typeof(Dog)});
            given_these_command_types(new [] { typeof(Apple), typeof(Banana) });
            given_message_scan_rules();
            when_bus_messages_is_created();
        }

        [TestMethod]
        public void finding_events_and_commands()
        {
            then_event_message_types_found_should_be(_expectedEventTypes);
            then_command_message_types_found_should_be(_expectedCommandTypes);
        }

        [TestMethod]
        public void identifying_messages()
        {
            then_bus_should_identify_commands();
            then_bus_should_identify_events();
        }

        [TestMethod]
        public void translating_message_type_name_to_type()
        {
            then_message_type_names_are_translated_to_types();
        }

        [TestMethod]
        public void translating_type_to_message_type_name()
        {
            then_types_are_translated_to_message_type_name();
        }

        // TODO: CL on 2017-11-16 - This test doesn't use the same setup as the other ones,
        // so it belongs in another fixture...
        [TestMethod]
        public void no_messages_found_should_throw_exception() {
            Action action = () => new BusMessages(
                new MessageScanRules()
                .DefineEventScanRuleWith(t => t.Namespace == "Blah")
                .DefineCommandScanRuleWith(t => t.Namespace == "Blah"));
            action.ShouldThrow<TypeLoadException>();
        }

        void given_these_command_types(Type[] commandTypes)
        {
            _expectedCommandTypes = commandTypes;
        }

        void given_these_event_types(Type[] eventType)
        {
            _expectedEventTypes = eventType;
        }

        void given_message_scan_rules()
        {
            _messageScanRules = new MessageScanRules()
                .DefineEventScanRuleWith(t => t.Namespace == "RockyBus.DemoMessages.Events")
                .DefineCommandScanRuleWith(t => t.Namespace == "RockyBus.DemoMessages.Commands");
        }

        void when_bus_messages_is_created()
        {
            _busMessages = new BusMessages(_messageScanRules);
        }

        void then_message_type_names_are_translated_to_types()
        {
            assert(CommandsAndEvents(),
                type => { _busMessages.GetTypeByMessageTypeName(type.FullName).Should().Be(type); });
        }

        void then_types_are_translated_to_message_type_name()
        {
            assert(CommandsAndEvents(),
                type => { _busMessages.GetMessageTypeNameByType(type).Should().Be(type.FullName); });
        }

        IEnumerable<Type> CommandsAndEvents()
        {
            return _expectedEventTypes.Union(_expectedCommandTypes);
        }

        void then_event_message_types_found_should_be(Type[] expectedTypes)
        {
            assert_types("EVENT", _busMessages.EventMessageTypeNames, expectedTypes);
        }

        void then_command_message_types_found_should_be(Type[] expectedTypes)
        {
            assert_types("COMMAND", _busMessages.CommandMessageTypeNames, expectedTypes);
        }

        void assert_types(string messageType, IEnumerable<string> typeNames, IEnumerable<Type> expectedTypes)
        {
            typeNames.Should().HaveCount(expectedTypes.Count(), $"Incorrect number of {messageType} message types.");

            expectedTypes.ForEach(expectedType =>
            {
                var expectedTypeFullName = expectedType.FullName;
                typeNames.Should().Contain(expectedTypeFullName, $"Type {expectedTypeFullName} not found on {messageType}");
            });
        }
        
        void then_bus_should_identify_events()
        {
            assert(_expectedEventTypes, type => _busMessages.IsAnEvent(type).Should().BeTrue());
            assert(_expectedCommandTypes, type => _busMessages.IsAnEvent(type).Should().BeFalse());
        }

        void then_bus_should_identify_commands()
        {
            assert(_expectedCommandTypes, type => _busMessages.IsACommand(type).Should().BeTrue());
            assert(_expectedEventTypes, type => _busMessages.IsACommand(type).Should().BeFalse());
        }

        void assert(IEnumerable<Type> types, Action<Type> assertion)
        {
            types.ForEach(assertion);
        }
    }
}
