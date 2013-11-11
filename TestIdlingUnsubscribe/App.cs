#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
#endregion

namespace TestIdlingUnsubscribe
{
  class App : IExternalApplication
  {
    /// <summary>
    /// Are we currently subscribed to the Idling event?
    /// </summary>
    public static bool Subscribed = false;

    /// <summary>
    /// Our one and only Revit-provided 
    /// UIControlledApplication instance.
    /// </summary>
    static UIControlledApplication _uiapp;

    /// <summary>
    /// Toggle on and off subscription to 
    /// automatic cloud updates.
    /// </summary>
    public static void ToggleSubscription(
      EventHandler<IdlingEventArgs> handler )
    {
      if( Subscribed )
      {
        Debug.Print( "Unsubscribing..." );
        _uiapp.Idling -= handler;
        Subscribed = false;
        Debug.Print( "Unsubscribed." );
      }
      else
      {
        Debug.Print( "Subscribing..." );
        _uiapp.Idling += handler;
        Subscribed = true;
        Debug.Print( "Subscribed." );
      }
    }

    public Result OnStartup( UIControlledApplication a )
    {
      _uiapp = a;
      return Result.Succeeded;
    }

    public Result OnShutdown( UIControlledApplication a )
    {
      if( Subscribed )
      {
        _uiapp.Idling
          -= new EventHandler<IdlingEventArgs>(
            ( sender, ea ) => { } );
      }
      return Result.Succeeded;
    }
  }
}
