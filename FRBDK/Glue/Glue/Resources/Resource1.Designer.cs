﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlatRedBall.Glue.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FlatRedBall.Glue.Resources.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap broadcastFRB {
            get {
                object obj = ResourceManager.GetObject("broadcastFRB", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using FlatRedBall;
        ///using Microsoft.Xna.Framework;
        ///
        ///#if !FRB_MDX
        ///using System.Linq;
        ///#endif
        ///
        ///namespace REPLACED_NAMESPACE
        ///{
        ///	internal static class CameraSetup
        ///	{
        ///        		// Generated Code:
        ///	}
        ///}
        ///.
        /// </summary>
        public static string CameraSetupTemplate {
            get {
                return ResourceManager.GetString("CameraSetupTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap CanInterpolate {
            get {
                object obj = ResourceManager.GetObject("CanInterpolate", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap CantInterpolate {
            get {
                object obj = ResourceManager.GetObject("CantInterpolate", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap code {
            get {
                object obj = ResourceManager.GetObject("code", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap copyIcon {
            get {
                object obj = ResourceManager.GetObject("copyIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///
        ///namespace NAMESPACE___NAME
        ///{
        ///    public partial class CLASS___NAME
        ///    {
        ///
        ///
        ///
        ///    }
        ///}
        ///.
        /// </summary>
        public static string EventTemplate {
            get {
                return ResourceManager.GetString("EventTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon Glue {
            get {
                object obj = ResourceManager.GetObject("Glue", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #region Usings
        ///
        ///using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using FlatRedBall;
        ///using FlatRedBall.Input;
        ///using FlatRedBall.Instructions;
        ///using FlatRedBall.AI.Pathfinding;
        ///using FlatRedBall.Graphics.Animation;
        ///using FlatRedBall.Graphics.Particle;
        ///
        ///using FlatRedBall.Math.Geometry;
        ///using FlatRedBall.Math.Splines;
        ///using BitmapFont = FlatRedBall.Graphics.BitmapFont;
        ///using Cursor = FlatRedBall.Gui.Cursor;
        ///using GuiManager = FlatRedBall.Gui.GuiManager;
        ///
        ///#if FRB_XNA || SILVER [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GlueEntityTemplate {
            get {
                return ResourceManager.GetString("GlueEntityTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #if ANDROID || IOS
        ///#define REQUIRES_PRIMARY_THREAD_LOADING
        ///#endif
        ///
        ///using Color = Microsoft.Xna.Framework.Color;
        ///
        ///namespace FlatRedBallAddOns.Entities
        ///{
        ///	public partial class GlueEntityTemplate
        ///	{
        ///        // This is made static so that static lazy-loaded content can access it.
        ///        public static string ContentManagerName
        ///        {
        ///            get;
        ///            set;
        ///        }
        ///
        ///		// Generated Fields
        ///
        ///        public GlueEntityTemplate()
        ///            : this(FlatRedBall.Screens.ScreenManager. [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GlueEntityTemplate_Generated {
            get {
                return ResourceManager.GetString("GlueEntityTemplate_Generated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #region Usings
        ///
        ///using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using FlatRedBall;
        ///using FlatRedBall.Input;
        ///using FlatRedBall.Instructions;
        ///using FlatRedBall.AI.Pathfinding;
        ///using FlatRedBall.Graphics.Animation;
        ///using FlatRedBall.Graphics.Particle;
        ///
        ///using FlatRedBall.Math.Geometry;
        ///using FlatRedBall.Math.Splines;
        ///
        ///using Cursor = FlatRedBall.Gui.Cursor;
        ///using GuiManager = FlatRedBall.Gui.GuiManager;
        ///using FlatRedBall.Localization;
        ///
        ///#if FRB_XNA || SILVERLIGHT
        ///using Keys  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GlueScreenTemplate {
            get {
                return ResourceManager.GetString("GlueScreenTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #if ANDROID || IOS
        ///#define REQUIRES_PRIMARY_THREAD_LOADING
        ///#endif
        ///
        ///using Color = Microsoft.Xna.Framework.Color;
        ///
        ///// Generated Usings
        ///
        ///namespace FlatRedBallAddOns.Screens
        ///{
        ///	public partial class ScreenTemplate
        ///	{
        ///		// Generated Fields
        ///
        ///		public ScreenTemplate()
        ///			: base(&quot;ScreenTemplate&quot;)
        ///		{
        ///		}
        ///
        ///        public override void Initialize(bool addToManagers)
        ///        {
        ///			// Generated Initialize
        ///
        ///        }
        ///        
        ///// Generated AddToManagers
        ///
        ///
        ///		public override void Activity(bool fir [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GlueScreenTemplate_Generated {
            get {
                return ResourceManager.GetString("GlueScreenTemplate_Generated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Linq;
        ///using System.Text;
        ///
        ///namespace REPLACED_NAMESPACE
        ///{
        ///    public interface IEntityFactory
        ///    {
        ///        object CreateNew();
        ///        object CreateNew(FlatRedBall.Graphics.Layer layer);
        ///    }
        ///}
        ///.
        /// </summary>
        public static string IEntityFactory {
            get {
                return ResourceManager.GetString("IEntityFactory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    // DELEGATE START HERE
        ///    
        ///
        ///        #region IWindow methods and properties
        ///
        ///        public event FlatRedBall.Gui.WindowEvent Click;
        ///		public event FlatRedBall.Gui.WindowEvent ClickNoSlide;
        ///		public event FlatRedBall.Gui.WindowEvent SlideOnClick;
        ///        public event FlatRedBall.Gui.WindowEvent Push;
        ///		public event FlatRedBall.Gui.WindowEvent DragOver;
        ///		public event FlatRedBall.Gui.WindowEvent RollOn;
        ///		public event FlatRedBall.Gui.WindowEvent RollOff;
        ///		public event FlatRedBall.Gui.Windo [rest of string was truncated]&quot;;.
        /// </summary>
        public static string IWindowTemplate {
            get {
                return ResourceManager.GetString("IWindowTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap NeedsInterpolateVariable {
            get {
                object obj = ResourceManager.GetObject("NeedsInterpolateVariable", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///
        ///namespace REPLACED_NAMESPACE
        ///{
        ///    public class PoolList&lt;T&gt; where T : FlatRedBall.Performance.IPoolable
        ///    {
        ///        #region Fields
        ///        List&lt;T&gt; mPoolables = new List&lt;T&gt;();
        ///        int mNextAvailable = -1;
        ///        #endregion
        ///
        ///        #region Methods
        ///
        ///        public void AddToPool(T poolableToAdd)
        ///        {
        ///
        ///            int index = mPoolables.Count;
        ///
        ///            if (mNextAvailable == -1)
        ///            {
        ///               [rest of string was truncated]&quot;;.
        /// </summary>
        public static string PoolList {
            get {
                return ResourceManager.GetString("PoolList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap redball {
            get {
                object obj = ResourceManager.GetObject("redball", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}