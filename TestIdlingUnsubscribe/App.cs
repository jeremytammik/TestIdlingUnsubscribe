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
    /// Store the Idling event handler when subscribed.
    /// </summary>
    static EventHandler<IdlingEventArgs> _handler = null;

    /// <summary>
    /// Are we currently subscribed to the Idling event?
    /// </summary>
    public static bool Subscribed
    {
      get
      {
        return null != _handler;
      }
    }

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
        _uiapp.Idling -= _handler;
        _handler = null;
        Debug.Print( "Unsubscribed." );
      }
      else
      {
        Debug.Print( "Subscribing..." );
        _uiapp.Idling += handler;
        _handler = handler;
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
        _uiapp.Idling -= _handler;
      }
      return Result.Succeeded;
    }
  }
}
