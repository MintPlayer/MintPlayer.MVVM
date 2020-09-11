﻿using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace MintPlayer.MVVM.Platforms.Common.Behaviors
{
    public class Event2CommandBehavior : BehaviorBase<View>
	{
		Delegate eventHandler;

		#region Bindable Properties
		public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(Event2CommandBehavior), null, propertyChanged: OnEventNameChanged);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(Event2CommandBehavior), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Event2CommandBehavior), null);
		public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(Event2CommandBehavior), null);
		#endregion

		#region Properties
		public string EventName
		{
			get => (string)GetValue(EventNameProperty);
			set => SetValue(EventNameProperty, value);
		}

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		public IValueConverter Converter
		{
			get => (IValueConverter)GetValue(InputConverterProperty);
			set => SetValue(InputConverterProperty, value);
		}
		#endregion

		protected override void OnAttachedTo(View bindable)
		{
			base.OnAttachedTo(bindable);
			RegisterEvent(EventName);
		}

		protected override void OnDetachingFrom(View bindable)
		{
			DeregisterEvent(EventName);
			base.OnDetachingFrom(bindable);
		}

		void RegisterEvent(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return;
			}

			var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
			if (eventInfo == null)
			{
				throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
			}
			var methodInfo = typeof(Event2CommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(Event2CommandBehavior.OnEvent));
			eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler(AssociatedObject, eventHandler);
		}

		void DeregisterEvent(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return;
			}

			if (eventHandler == null)
			{
				return;
			}
			EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
			if (eventInfo == null)
			{
				throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
			}
			eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
			eventHandler = null;
		}

		void OnEvent(object sender, object eventArgs)
		{
			if (Command == null)
			{
				return;
			}

			object resolvedParameter;
			if (CommandParameter != null)
			{
				resolvedParameter = CommandParameter;
			}
			else if (Converter != null)
			{
				resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
			}
			else
			{
				resolvedParameter = eventArgs;
			}

			if (Command.CanExecute(resolvedParameter))
			{
				Command.Execute(resolvedParameter);
			}
		}

		static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (Event2CommandBehavior)bindable;
			if (behavior.AssociatedObject == null)
			{
				return;
			}

			string oldEventName = (string)oldValue;
			string newEventName = (string)newValue;

			behavior.DeregisterEvent(oldEventName);
			behavior.RegisterEvent(newEventName);
		}
	}
}
