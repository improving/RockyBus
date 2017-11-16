using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockyBus.DemoMessages;

namespace RockyBus.UnitTests
{
    [TestClass]
    public class MessageHandlerTypeCreatorTests
    {
        Type _createdType;
        MessageHandlerTypeCreator _messageHandlerTypeCreator;

        [TestMethod]
        public void create_message_handler_of_type_should_match_message_handler_type()
        {
            given_a_creator();
            when_creating_a_command_type(_messageHandlerTypeCreator);
            then_created_type_should_be<AppleCommand>();
        }

        [TestMethod]
        public void create_message_handler_of_different_type_should_not_match_message_handler_type()
        {
            given_a_creator();
            when_creating_a_command_type(_messageHandlerTypeCreator);
            then_created_type_should_NOT_be<BananaCommand>();
        }

        void given_a_creator()
        {
            _messageHandlerTypeCreator = new MessageHandlerTypeCreator();
        }

        void when_creating_a_command_type(MessageHandlerTypeCreator creator)
        {
            _createdType = creator.Create(typeof(AppleCommand));
        }

        void then_created_type_should_be<T>()
        {
            _createdType.Should().Be(typeof(IMessageHandler<T>));
        }

        void then_created_type_should_NOT_be<T>()
        {
            _createdType.Should().NotBe(typeof(IMessageHandler<T>));
        }
    }
}