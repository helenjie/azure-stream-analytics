﻿//********************************************************* 
// 
//    Copyright (c) Microsoft. All rights reserved. 
//    This code is licensed under the Microsoft Public License. 
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF 
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY 
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT. 
// 
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Configuration;
using GalaSoft.MvvmLight.Messaging;

namespace TwitterClient
{
	public class EventHubObserverWPF: IObserver<Payload>
	{
		private EventHubConfig _config;
		private EventHubClient _eventHubClient;


		public EventHubObserverWPF(EventHubConfig config)
		{
			try
			{
				_config = config;
				_eventHubClient = EventHubClient.CreateFromConnectionString(_config.ConnectionString, config.EventHubName);

			}
			catch (Exception ex)
			{

			}

		}

		public void OnNext(Payload TwitterPayloadData)
		{
			try
			{

				var serialisedString = JsonConvert.SerializeObject(TwitterPayloadData);
				EventData data = new EventData(Encoding.UTF8.GetBytes(serialisedString)) { PartitionKey = TwitterPayloadData.Topic };
				_eventHubClient.Send(data);

				Messenger.Default.Send<Payload>(TwitterPayloadData);

			}
			catch (Exception ex)
			{

			}

		}

		public void OnCompleted()
		{
			Messenger.Default.Send<Payload>(new Payload() {CreatedAt=DateTime.UtcNow, Text ="Stopped Per User."});
		}

		public void OnError(Exception error)
		{

		}

	}
}
