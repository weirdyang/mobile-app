using System;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Moq;
using Moq.Language.Flow;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.Tests
{
    public static class MockExtensions
    {
        public static void SetupMessengerSubscribe<TEvent>(this Mock<IMvxMessenger> messengerMock, Action<Action<TEvent>> saveCallAction) where TEvent : MvxMessage
        {
            messengerMock
                .Setup(x => x.Subscribe(It.IsAny<Action<TEvent>>(), MvxReference.Weak, null))
                .Callback<Action<TEvent>, MvxReference, string>((action, reference, tag) => saveCallAction.Invoke(action));
        }


        public static void SetupAlert(this Mock<IUserInteraction> userInteractionMock)
        {
            userInteractionMock.Setup(x => x.AlertAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAwaitable();
        }

        public static void SetupConfirm(this Mock<IUserInteraction> userInteractionMock, bool result)
        {
            userInteractionMock.Setup(x => x.ConfirmAsync(It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>()));
        }

        public static ISetup<TMock, Task> ReturnsAwaitable<TMock>(this ISetup<TMock, Task> setup) where TMock : class
        {
            setup.Returns(Task.FromResult(0));

            return setup;
        }
    }
}