﻿// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
#if WINDOWS_APP
using Dots.WinApps.Windows;
#endif
using Microsoft.WindowsAzure.MobileServices;
#if WINDOWS_PHONE_APP
using Dots.WinApps.WindowsPhone;
#endif

namespace Dots.WinApps.Shared
{
   /// <summary>
   /// Provides application-specific behavior to supplement the default Application class.
   /// </summary>
   public sealed partial class App : Application
   {
#if WINDOWS_PHONE_APP
      private TransitionCollection transitions;
#endif

      /// <summary>
      /// Initializes the singleton application object.  This is the first line of authored code
      /// executed, and as such is the logical equivalent of main() or WinMain().
      /// </summary>
      public App()
      {
         this.InitializeComponent();
         this.Suspending += this.OnSuspending;

         SettingsManager.Load();
      }

      /// <summary>
      /// Invoked when the application is launched normally by the end user.  Other entry points
      /// will be used when the application is launched to open a specific file, to display
      /// search results, and so forth.
      /// </summary>
      /// <param name="e">Details about the launch request and process.</param>
      protected async override void OnLaunched( LaunchActivatedEventArgs e )
      {
#if DEBUG
         if ( System.Diagnostics.Debugger.IsAttached )
         {
            this.DebugSettings.EnableFrameRateCounter = true;
         }
#endif

         Frame rootFrame = Window.Current.Content as Frame;

         // Do not repeat app initialization when the Window already has content,
         // just ensure that the window is active
         if ( rootFrame == null )
         {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            //Associate the frame with a SuspensionManager key                                
            SuspensionManager.RegisterFrame( rootFrame, "AppFrame" );

            // TODO: change this value to a cache size that is appropriate for your application
            rootFrame.CacheSize = 1;

            if ( e.PreviousExecutionState == ApplicationExecutionState.Terminated )
            {
               // Restore the saved session state only when appropriate
               try
               {
                  await SuspensionManager.RestoreAsync();
               }
               catch ( SuspensionManagerException )
               {
                  // Something went wrong restoring state.
                  // Assume there is no state and continue
               }
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
         }

         if ( rootFrame.Content == null )
         {
#if WINDOWS_PHONE_APP
            // Removes the turnstile navigation for startup.
            if ( rootFrame.ContentTransitions != null )
            {
               this.transitions = new TransitionCollection();
               foreach ( var c in rootFrame.ContentTransitions )
               {
                  this.transitions.Add( c );
               }
            }

            rootFrame.ContentTransitions = null;
            rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            Type initialPageType = typeof( LoginPage );
            try
            {
               if ( await CredentialsHelper.LoginWithSavedCredentials() )
               {
                  initialPageType = typeof( GamesPage );
               }
            }
            catch ( Exception )
            {
               CredentialsHelper.Logout();
               initialPageType = typeof( LoginPage );
            }

            if ( !rootFrame.Navigate( initialPageType, e.Arguments ) )
            {
               throw new Exception( "Failed to create initial page" );
            }
         }

         // Ensure the current window is active
         Window.Current.Activate();
      }

#if WINDOWS_PHONE_APP
      /// <summary>
      /// Restores the content transitions after the app has launched.
      /// </summary>
      /// <param name="sender">The object where the handler is attached.</param>
      /// <param name="e">Details about the navigation event.</param>
      private void RootFrame_FirstNavigated( object sender, NavigationEventArgs e )
      {
         var rootFrame = sender as Frame;
         rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
         rootFrame.Navigated -= this.RootFrame_FirstNavigated;
      }
#endif

      /// <summary>
      /// Invoked when application execution is being suspended.  Application state is saved
      /// without knowing whether the application will be terminated or resumed with the contents
      /// of memory still intact.
      /// </summary>
      /// <param name="sender">The source of the suspend request.</param>
      /// <param name="e">Details about the suspend request.</param>
      private async void OnSuspending( object sender, SuspendingEventArgs e )
      {
         var deferral = e.SuspendingOperation.GetDeferral();
         await SuspensionManager.SaveAsync();
         // TODO: Save application state and stop any background activity
         deferral.Complete();
      }

      protected override void OnActivated( IActivatedEventArgs args )
      {
         base.OnActivated( args );
#if WINDOWS_PHONE_APP
         if ( args.Kind == ActivationKind.WebAuthenticationBrokerContinuation )
         {
            ServiceHelper.MobileService.LoginComplete( args as WebAuthenticationBrokerContinuationEventArgs );
         }
#endif
      }
   }
}