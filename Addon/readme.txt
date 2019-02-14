
Get EventGrid emulator from here
https://github.com/ravinsp/eventgrid-emulator/releases


### Content of emulator-config.json

{
  "port": 5000,
  "topics": [
    {
      "name": "Names",
      "subscriptions": [
        {
          "name": "Names",
          "eventTypes": [ "NameFound", "NameNotFound" ], 
          "SubjectBeginsWith": "",
          "SubjectEndsWith": "",
          "endpointUrl": "http://localhost:7071/runtime/webhooks/eventgrid?functionName=EventTriggerFunc",
          "dispatchStrategy": "DefaultHttpStrategy"
        }
      ]
    }
  ],
  "dispatchStrategies": [
    {
      "name": "DefaultHttpStrategy",
      "type": "EventGridEmulator.Logic.DispatchStrategies.DefaultHttpStrategy"
    }
  ]
}

