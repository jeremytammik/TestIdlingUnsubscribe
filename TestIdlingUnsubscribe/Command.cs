#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
#endregion

namespace TestIdlingUnsubscribe
{
  [Transaction( TransactionMode.ReadOnly )]
  public class Command : IExternalCommand
  {
    /// <summary>
    /// How many Idling calls to wait before reporting
    /// </summary>
    const int _message_interval = 100;

    /// <summary>
    /// Number of Idling calls received in this session
    /// </summary>
    static int _counter = 0;

    void OnIdling(
      object sender,
      IdlingEventArgs ea )
    {
      ++_counter;

      if( 0 == ( _counter % _message_interval ) )
      {
        Debug.Print( "{0} OnIdling called {1} times",
          DateTime.Now.ToString( "HH:mm:ss.fff" ),
          _counter );
      }

      ea.SetRaiseWithoutDelay();
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Debug.Print( 
        "We are currently {0}subscribed to the Idling event.", 
        App.Subscribed ? "" : "not " );

      App.ToggleSubscription( OnIdling );

      return Result.Succeeded;
    }
  }
}
