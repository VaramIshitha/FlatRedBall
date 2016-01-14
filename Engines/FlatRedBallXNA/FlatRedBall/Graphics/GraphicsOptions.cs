using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FlatRedBall.IO;
#if FRB_MDX
using Microsoft.DirectX.Direct3D;
#elif FRB_XNA || SILVERLIGHT
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
    #if !XBOX360 && !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
    using System.Windows.Forms;
    #endif
#endif

namespace FlatRedBall.Graphics
{
    public class GraphicsOptions
    {
        #region Fields
        TextureFilter mTextureFilter = TextureFilter.Point;

        bool mSuspendDeviceReset = false;
        int mResolutionWidth;

        int mResolutionHeight;
        #endregion

        #region Properties

        #region XML Docs
        /// <summary>
        /// Gets or sets the current texture filter
        /// </summary>
        #endregion
        public TextureFilter TextureFilter
        {
            get { return mTextureFilter; }
            set
            {
                if (true)//mTextureFilter != value)
                {
                    mTextureFilter = value;

                    if (!mSuspendDeviceReset)
                    {
                        #region If DEBUG, check the caps of the graphics card
#if DEBUG
                        ThrowExceptionIfFilterIsntSupported(value);

#endif
                        #endregion

#if FRB_MDX
                        Renderer.GraphicsDevice.SamplerState[0].MipFilter = mTextureFilter;

                        Renderer.GraphicsDevice.SamplerState[0].MinFilter = mTextureFilter;
                        Renderer.GraphicsDevice.SamplerState[0].MagFilter = mTextureFilter;
#elif XNA4 || WINDOWS_8
                        ForceRefreshSamplerState();
#elif FRB_XNA
                        Renderer.GraphicsDevice.SamplerStates[0].MinFilter = mTextureFilter;
                        Renderer.GraphicsDevice.SamplerStates[0].MagFilter = mTextureFilter;
#endif
                    }
                }
            }
        }

        private static void ThrowExceptionIfFilterIsntSupported(TextureFilter value)
        {

#if XNA4 || WINDOWS_8
            // For now do nothing, but we may want to perform some checks here against whether we're using REACH or HIDEF

#elif FRB_XNA
            // Check to see if the caps are supported
            GraphicsDevice gd = FlatRedBallServices.GraphicsDevice;

            switch (value)
            {
                case TextureFilter.Anisotropic:
                    if (!gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMagnifyAnisotropic ||
                        !gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMinifyAnisotropic)
                    {
                        throw new ArgumentException("Your graphics device does not support " + value);
                    }
                    break;
                case TextureFilter.GaussianQuad:
                    if (!gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMagnifyGaussianQuad ||
                        !gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMinifyGaussianQuad)
                    {
                        throw new ArgumentException("Your graphics device does not support " + value);
                    }
                    break;
                case TextureFilter.Linear:
                    if (!gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMagnifyLinear ||
                        !gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMinifyLinear)
                    {
                        throw new ArgumentException("Your graphics device does not support " + value);
                    }
                    break;
                case TextureFilter.None:
#if XBOX360
                                throw new ArgumentException("Your graphics device does not support " + value);
#endif
                    break;
                case TextureFilter.Point:
                    if (!gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMagnifyPoint ||
                        !gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMinifyPoint)
                    {
                        throw new ArgumentException("Your graphics device does not support " + value);
                    }
                    break;
                case TextureFilter.PyramidalQuad:
                    if (!gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMagnifyPyramidalQuad ||
                        !gd.GraphicsDeviceCapabilities.TextureFilterCapabilities.SupportsMinifyPyramidalQuad)
                    {
                        throw new ArgumentException("Your graphics device does not support " + value);
                    }

                    break;
            }
#endif
        }

        #region XML Docs
        /// <summary>
        /// Sets the width of the backbuffer and resets the device
        /// Use SetResolution() to set both width and height simultaneously
        /// </summary>
        #endregion
        public int ResolutionWidth
        {
            get { return mResolutionWidth; }
            set
            {
                mResolutionWidth = value;
#if !FRB_MDX
                ResetDevice();
#endif
            }
        }

        #region XML Docs
        /// <summary>
        /// Sets the height of the backbuffer and resets the device
        /// Use SetResolution() to set both width and height simultaneously
        /// </summary>
        #endregion
        public int ResolutionHeight
        {
            get { return mResolutionHeight; }
            set
            {
                mResolutionHeight = value;
#if !FRB_MDX
                ResetDevice();
#endif
            }
        }

        #endregion


        public event EventHandler SizeOrOrientationChanged;

#if FRB_XNA || SILVERLIGHT || WINDOWS_PHONE


        #region Fields

        #region XML Docs
        /// <summary>
        /// The texture loading color key
        /// </summary>
        #endregion
        public Color TextureLoadingColorKey = Color.Black;




        bool mIsFullScreen;

#if !XBOX360 && !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
        // For some reason setting to fullscreen can crash things but setting the border style to none helps.
        System.Windows.Forms.FormBorderStyle mWindowedBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D; 
#endif

        bool mUseMultiSampling;
        
#if !XNA4
        MultiSampleType mMultiSampleType;
#endif

        #region XML Docs
        /// <summary>
        /// Set to true to suspend device reset while loading from file
        /// </summary>
        #endregion
        private static bool IsLoading = false;

        private bool mIsInAReset = false;

        #endregion

        #region Properties

        #region XML Docs
        /// <summary>
        /// Gets or sets the background color of all cameras
        /// </summary>
        #endregion
        [XmlIgnoreAttribute()]
        public Color BackgroundColor
        {
            get { return SpriteManager.Camera.BackgroundColor; }
            set
            {
                for (int i = 0; i < SpriteManager.Cameras.Count; i++)
                {
                    SpriteManager.Cameras[i].BackgroundColor = value;
                }
            }
        }





        #region XML Docs
        /// <summary>
        /// Sets the display mode to full screen
        /// Use SetFullScreen() to set the full-screen resolution and full-screen simultaneously
        /// </summary>
        #endregion
        public bool IsFullScreen
        {
            get { return mIsFullScreen; }
            set
            {
                if (mIsFullScreen != value)
                {
                    int oldWidth = mResolutionWidth;
                    int oldHeight = mResolutionHeight;

                    mIsFullScreen = value;

					if (!FlatRedBallServices.IsInitialized)
					{
						// It's possible for someone to instantiate a GraphicsOptions and
						// set its FullScreen to true before FlatRedBall is created.  This is
						// done so that the engine starts in full screen.  If this is the case,
						// then we shouldn't do the remainder of the code in this property.
						return;
                    }

#if !XBOX360 && !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
                    if (mIsFullScreen)
                    {
                        mWindowedBorderStyle = ((Form)FlatRedBallServices.Owner).FormBorderStyle;
                        ((Form)FlatRedBallServices.Owner).FormBorderStyle = FormBorderStyle.None;
                    }
#endif

                    ResetDevice();
#if !XBOX360 && !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
                    if (!mIsFullScreen)
                    {
                        ((Form)FlatRedBallServices.Owner).FormBorderStyle = mWindowedBorderStyle;
                    }
#endif
                    if (!mIsFullScreen)
                    {
                        // When coming out of full screen the resolution is lost for some reason, so force it
                        SetResolution(oldWidth, oldHeight);
                    }
                }
            }
        }

        #region XML Docs
        /// <summary>
        /// Enables or disables multisampling
        /// </summary>
        #endregion
        public bool UseMultiSampling
        {
            get { return mUseMultiSampling; }
            set
            {
                mUseMultiSampling = value;
                ResetDevice();
            }
        }

#if !XNA4
        #region XML Docs
        /// <summary>
        /// Gets or sets the multisampling type
        /// Use SetMultiSampling() to enable multisampling and specify options simultaneously
        /// </summary>
        #endregion
        public MultiSampleType MultiSampleType
        {
            get { return mMultiSampleType; }
            set
            {
                mMultiSampleType = value;
                ResetDevice();
            }
        }
#endif

        #endregion

        #region Constructor

        public GraphicsOptions()
            : this(null, null)
        {

        }


        public GraphicsOptions(Game game, GraphicsDeviceManager graphics)
        {


#if XNA4
            if (game != null)
            {
                game.Window.ClientSizeChanged += new EventHandler<EventArgs>(HandleClientSizeOrOrientationChange);
            }

            if (graphics != null)
            {
                mResolutionWidth = graphics.PreferredBackBufferWidth;
                mResolutionHeight = graphics.PreferredBackBufferHeight;
            }

            mTextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter.Linear;
            
            mUseMultiSampling = false;

#if WINDOWS_PHONE || MONODROID
            if (graphics != null)
            {
                mIsFullScreen = graphics.IsFullScreen;
            }
#endif
            
#elif !XBOX360
            mTextureFilter = TextureFilter.Linear;
            mResolutionWidth = 800;
            mResolutionHeight = 600;

            mUseMultiSampling = false;
            mMultiSampleType = MultiSampleType.TwoSamples;
            //mMultiSampleQuality = 0;

            if(graphics != null)
            {
                mIsFullScreen = graphics.IsFullScreen;
            }
#else
            mTextureFilter = TextureFilter.Linear;
            mResolutionWidth = 1280;
            mResolutionHeight = 720;

            mUseMultiSampling = false;
            mMultiSampleType = MultiSampleType.TwoSamples;
            //mMultiSampleQuality = 0;
#endif

#if !WINDOWS_PHONE && !MONODROID
            #region Get Resolution

            SuspendDeviceReset();
#if XBOX360
            if (graphics != null)
            {
                mIsFullScreen = true;
                graphics.IsFullScreen = true;
                ResolutionWidth = graphics.GraphicsDevice.DisplayMode.Width;//graphics.PreferredBackBufferWidth;
                ResolutionHeight = graphics.GraphicsDevice.DisplayMode.Height;//graphics.PreferredBackBufferHeight;
            }
#else
            //graphicsOptions.ResolutionWidth = graphics.GraphicsDevice.DisplayMode.Width;// game.Window.ClientBounds.Width;
            //graphicsOptions.ResolutionHeight = graphics.GraphicsDevice.DisplayMode.Height;//game.Window.ClientBounds.Height;

            if (game != null)
            {

#if SILVERLIGHT

#elif WINDOWS_8
                // For some reason the W8 window reports the wrong
                // width/height if the game is started when in portrait
                // mode but the user wants to be in landscape.  But the GraphicsDevice
                // is right.  Go figure.
                ResolutionWidth = graphics.GraphicsDevice.Viewport.Width ;
                ResolutionHeight = graphics.GraphicsDevice.Viewport.Height;
#elif IOS
				ResolutionWidth = graphics.PreferredBackBufferWidth;
				ResolutionHeight = graphics.PreferredBackBufferHeight;

#else
                ResolutionWidth = game.Window.ClientBounds.Width;
                ResolutionHeight = game.Window.ClientBounds.Height;
#endif
            }


            if (graphics != null)
            {
#if !SILVERLIGHT && !XNA4 && !WINDOWS_8
                if (!graphics.GraphicsDevice.CreationParameters.Adapter.CheckDeviceMultiSampleType(
                    DeviceType.Hardware, SurfaceFormat.Color, IsFullScreen, MultiSampleType))
                {
                    MultiSampleType = MultiSampleType.None;
                    //MultiSampleQuality = 1;
                }
#endif
            }

#endif
            ResumeDeviceReset();

            #endregion
#endif

            // November 15, 2013
            // Not sure why this is
            // here.  We do this same
            // code up above when we check
            // if the game is not null.  This
            // causes the event to fire twice.
//#if WINDOWS_8
//            game.Window.OrientationChanged += new EventHandler<EventArgs>(HandleClientSizeOrOrientationChange);
//            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(HandleClientSizeOrOrientationChange);
//#endif

        }

        void HandleClientSizeOrOrientationChange(object sender, EventArgs e)
        {
            if (SizeOrOrientationChanged != null)
            {
                SizeOrOrientationChanged(this, null);
            }
        }

        #endregion

        #region Methods


#if XNA4 || WINDOWS_8
        internal void ForceRefreshSamplerState() { ForceRefreshSamplerState(0); }
        internal void ForceRefreshSamplerState(int index)
        {
            switch (Renderer.TextureAddressMode)
            {
                case Microsoft.Xna.Framework.Graphics.TextureAddressMode.Clamp:
                    if (mTextureFilter == Microsoft.Xna.Framework.Graphics.TextureFilter.Point)
                    {
                        Renderer.GraphicsDevice.SamplerStates[index] = SamplerState.PointClamp;
                    }
                    else if (mTextureFilter == Microsoft.Xna.Framework.Graphics.TextureFilter.Linear)
                    {
                        Renderer.GraphicsDevice.SamplerStates[index] = SamplerState.LinearClamp;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    break;
                case Microsoft.Xna.Framework.Graphics.TextureAddressMode.Mirror:
                    throw new NotSupportedException();
                    //GraphicsDevice.SamplerStates[index] = SamplerState.;
                    //break;
                case Microsoft.Xna.Framework.Graphics.TextureAddressMode.Wrap:
                    if (mTextureFilter == Microsoft.Xna.Framework.Graphics.TextureFilter.Point)
                    {
                        Renderer.GraphicsDevice.SamplerStates[index] = SamplerState.PointWrap;
                    }
                    else if (mTextureFilter == Microsoft.Xna.Framework.Graphics.TextureFilter.Linear)
                    {
                        Renderer.GraphicsDevice.SamplerStates[index] = SamplerState.LinearWrap;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    break;


            }

        }

#endif

        #region Setter Operations

        internal bool mHasResolutionBeenManuallySet = false;

        #region XML Docs
        /// <summary>
        /// Sets the resolution
        /// </summary>
        /// <param name="width">The new width</param>
        /// <param name="height">The new height</param>
        #endregion
        public void SetResolution(int width, int height)
        {
            mHasResolutionBeenManuallySet = true;

            mResolutionWidth = width;
            mResolutionHeight = height;
            ResetDevice();

            
            // Not sure why but the GameWindow's resolution change doesn't fire
            // That's okay, we now have a custom event for it.  Glue will generate against this:
            if (SizeOrOrientationChanged != null)
            {
                SizeOrOrientationChanged(this, null);
            }
        }

        #region XML Docs
        /// <summary>
        /// Sets the display mode to full-screen and sets the resolution
        /// </summary>
        /// <param name="width">The new width</param>
        /// <param name="height">The new height</param>
        #endregion
        public void SetFullScreen(int width, int height)
        {
            mIsFullScreen = true;
            mResolutionWidth = width;
            mResolutionHeight = height;
            ResetDevice();

            if (SizeOrOrientationChanged != null)
            {
                SizeOrOrientationChanged(this, null);
            }
        }

        //#region XML Docs
        ///// <summary>
        ///// Sets multisampling on with the specified options
        ///// </summary>
        ///// <param name="type">The type of multisampling</param>
        ///// <param name="quality">The quality of multisampling</param>
        //#endregion
        //public void SetMultiSampling(MultiSampleType type, int quality)
        //{
        //    mUseMultiSampling = (type != MultiSampleType.None);

        //    mMultiSampleType = type;
        //    mMultiSampleQuality = 0; // What a terrible option.  See:
        //    //http://windows-tech.info/5/eff2355699026ce0.php
        //    ResetDevice();
        //}

        #endregion

        #region Reset Operations

#if !SILVERLIGHT && !XNA4 && !WINDOWS_8

        #region XML Docs
        /// <summary>
        /// Resets the texture filtering mode on the graphics device
        /// </summary>
        #endregion
        public void ResetTextureFilter()
        {

            Renderer.Graphics.GraphicsDevice.SamplerStates[0].MinFilter = mTextureFilter;
            Renderer.Graphics.GraphicsDevice.SamplerStates[0].MagFilter = mTextureFilter;
            Renderer.Graphics.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            Renderer.Graphics.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            Renderer.Graphics.GraphicsDevice.SamplerStates[0].AddressW = TextureAddressMode.Wrap;

        }
#endif

        #region XML Docs
        /// <summary>
        /// Suspends the device reset when options are changed
        /// </summary>
        #endregion
        public void SuspendDeviceReset()
        {
            mSuspendDeviceReset = true;
        }

        #region XML Docs
        /// <summary>
        /// Resumes the device reset when options are changed
        /// </summary>
        #endregion
        public void ResumeDeviceReset()
        {
            mSuspendDeviceReset = false;
        }

        #region XML Docs
        /// <summary>
        /// Resets the device
        /// </summary>
        #endregion
        public void ResetDevice()
        {
            if (mIsInAReset)
            {
                return;
            }

        #region If the Renderer.Graphics is null that means the engine is not loaded yet
            if (!mSuspendDeviceReset && Renderer.Graphics == null)
            {
                throw new InvalidOperationException("Can't reset the device right now because the Renderer's Graphics are null. " +
                    "Are you attempting to change the GraphicsOption's properties prior to the creation of FlatRedBallServices? " +
                    "If so you must call SuspendDeviceReset before changing properties, then ResumeDeviceReset after the properties " +
                    "are set, but before calling FlatRedBallServices.InitializeFlatRedBall.  Otherwise, move the property-changing " +
                    "code to after FlatRedBall is initialized.");

            }
        #endregion

        #region Else, check to make sure device resetting is not suspended and the GraphicsOptions are not loading
            else if (!mSuspendDeviceReset && !GraphicsOptions.IsLoading)
            {
                mIsInAReset = true;

                // Reset the graphics device manager
                if (FlatRedBallServices.mGraphics != null)
                {
#if !WINDOWS_PHONE
                    // Set window size
                    FlatRedBallServices.mGraphics.PreferredBackBufferWidth = mResolutionWidth;
                    FlatRedBallServices.mGraphics.PreferredBackBufferHeight = mResolutionHeight;
#endif
#if !SILVERLIGHT
                    FlatRedBallServices.mGraphics.PreferMultiSampling = mUseMultiSampling 
#if !XNA4
                        && mMultiSampleType != MultiSampleType.None
#endif
                        ;
#endif    
                    FlatRedBallServices.mGraphics.IsFullScreen = mIsFullScreen;
      
#if !SILVERLIGHT && !MONOGAME && !XNA4
                    if (Renderer.UseRenderTargets)
                    {
                        PostProcessing.PostProcessingManager.RefreshPostProcessingSurfaceSizes();
                    }
#endif

                    try
                    {
                        // Victor Chelaru
                        // February 26, 2014
                        // Android doesn't have 
                        // the ability to run on 
                        // multiple resolutions, so 
                        // we won't do any checks here.
#if DEBUG && !SILVERLIGHT && !ANDROID
                        if (IsFullScreen)
                        {
                            bool foundResolution = false;

                            var supportedDisplay = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;

                            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                            {
                                if (mode.Width == mResolutionWidth && mode.Height == mResolutionHeight)
                                {
                                    foundResolution = true;
                                }
                            }
                            if (!foundResolution)
                            {
                                string message = "The resolution is not supported in full screen mode.  Supported resolutions:\n";
                                message += "(width x height)\n";

                                foreach (var value in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                                {
                                    message += value.Width + "x" + value.Height + "\n";
                                }


                                throw new NotImplementedException(message);
                            }
                        }
#endif
                        FlatRedBallServices.mGraphics.ApplyChanges();

                    }
                        // No longer needed since we are always going to use a sample quality of 0
                    //catch (Exception e)
                    //{
                    //    int qualityLevels = 0;
                    //    bool allowed = GraphicsAdapter.DefaultAdapter.CheckDeviceMultiSampleType(DeviceType.Hardware,
                    //        SurfaceFormat.Color, false, mMultiSampleType, out qualityLevels);

                    //    throw e;
                    //}


                    finally
                    {
                        mIsInAReset = false;
                    }
                }
#if !SILVERLIGHT

                // Prepare the presentation parameters
                PresentationParameters presParams = FlatRedBallServices.GraphicsDevice.PresentationParameters;

                // Sets the presentation parameters
                SetPresentationParameters(ref presParams);

                // Reset the device
                if (FlatRedBallServices.mGraphics != null)
                {
#if !MONODROID
                    while (FlatRedBallServices.mGraphics.GraphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.Lost ||
                        FlatRedBallServices.mGraphics.GraphicsDevice.GraphicsDeviceStatus == GraphicsDeviceStatus.NotReset)
                    {
                        int m = 3;
                        m += 32;
                        m /= 32;
                    }
#endif
                }

#if WINDOWS_8 || IOS || ANDROID
                // Resetting crashes monogame currently, but we can still react as if a reset happened
                FlatRedBallServices.graphics_DeviceReset(null, null);
#else
                FlatRedBallServices.GraphicsDevice.Reset(presParams);
#endif

                // When the device resets the render states could get screwed up.  Force the 
                // blend state changes in case they were changed but nothing later changes them
                // back.
                // Hm, this seems to cause a crash because the mCurrentEffect isn't set yet
                //Renderer.ForceSetColorOperation(Renderer.ColorOperation);
                Renderer.ForceSetBlendOperation();
#else
                FlatRedBallServices.graphics_DeviceReset(null, null);
#endif
                mIsInAReset = false;
            }
        #endregion
        }

        #region XML Docs
        /// <summary>
        /// Sets the presentation parameters
        /// </summary>
        /// <param name="presentationParameters">The structure to set parameters in</param>
        #endregion
        public void SetPresentationParameters(ref PresentationParameters presentationParameters)
        {
#if !SILVERLIGHT
            presentationParameters.BackBufferWidth = mResolutionWidth;
            presentationParameters.BackBufferHeight = mResolutionHeight;
            presentationParameters.IsFullScreen = mIsFullScreen;


#if XNA4 || WINDOWS_8

            //throw new NotImplementedException();
#else
            presentationParameters.MultiSampleType = mUseMultiSampling ? mMultiSampleType : MultiSampleType.None;
            presentationParameters.MultiSampleQuality = 0;
#endif


#endif
        }

        #endregion

        #region File Operations

        #region XML Docs
        /// <summary>
        /// Save the graphics options to a file
        /// </summary>
        /// <param name="fileName">The file name of the graphics options file</param>
        #endregion
        public void Save(string fileName)
        {
            FileManager.XmlSerialize<GraphicsOptions>(this, fileName);
        }

        #region XML Docs
        /// <summary>
        /// Load the graphics options from file
        /// </summary>
        /// <param name="fileName">The file name of the graphics options file</param>
        #endregion
        public static GraphicsOptions FromFile(string fileName)
        {
            GraphicsOptions options;
            try
            {
                GraphicsOptions.IsLoading = true;
                options = FileManager.XmlDeserialize<GraphicsOptions>(fileName);
            }
            catch
            {
                options = new GraphicsOptions(); // failed to open the file, oh well, make a new one.
            }
            finally
            {
                GraphicsOptions.IsLoading = false;
            }

            return options;
        }

        #endregion

        #endregion

#elif FRB_MDX
        #region Fields

        public UInt32 TextureLoadingColorKey = 0xff000000;

        #endregion

        #region Properties

        public static bool UseXnaColors
        {
            get;
            set;
        }


        public bool FullScreen { get; set; }

        #endregion

        #region Public Methods

        #region XML Docs
        /// <summary>
        /// Resets the texture filtering mode on the graphics device
        /// </summary>
        #endregion
        public void ResetTextureFilter()
        {
#if SILVERLIGHT
            throw new NotImplementedException();
#else
            Renderer.GraphicsDevice.SamplerState[0].MinFilter = mTextureFilter;
            Renderer.GraphicsDevice.SamplerState[0].MagFilter = mTextureFilter;
#endif

        }



        #endregion
        #endif

    }

}