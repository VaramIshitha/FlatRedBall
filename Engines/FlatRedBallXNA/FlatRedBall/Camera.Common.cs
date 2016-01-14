using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
#if FRB_MDX
using Microsoft.DirectX;
using System.Drawing;
#else
using Microsoft.Xna.Framework;

#endif

namespace FlatRedBall
{
    public partial class Camera : PositionedObject
    {
        #region Enums
        public enum CoordinateRelativity
        {
            RelativeToWorld,
            RelativeToCamera
        }

        public enum SplitScreenViewport
        {
            FullScreen,
            TopHalf,
            BottomHalf,
            LeftHalf,
            RightHalf,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        #endregion

        #region Fields


        float mYEdge;
        float mXEdge;

        float mTopDestination;
        float mBottomDestination;
        float mLeftDestination;
        float mRightDestination;

        internal Rectangle mDestinationRectangle;

        float mTopDestinationVelocity;
        float mBottomDestinationVelocity;
        float mLeftDestinationVelocity;
        float mRightDestinationVelocity;


        bool mOrthogonal;
        float mOrthogonalWidth;
        float mOrthogonalHeight;

        float mFieldOfView;
        float mAspectRatio;

        bool mDrawsWorld = true;
        bool mDrawsCameraLayer = true;
        bool mDrawsShapes = true;
        #endregion

        #region Properties

        public static Camera Main
        {
            get
            {
                return SpriteManager.Camera;
            }
        }

        #region XML Docs
        /// <summary>
        /// Gets and sets the top side of the destination rectangle (where on the window
        /// the camera will display).  Measured in pixels.  Destination uses an inverted Y (positive points down).
        /// </summary>
        #endregion
        public virtual float TopDestination
        {
            get { return mTopDestination; }
            set 
            { 
                mTopDestination = value;
                mUsesSplitScreenViewport = false;
                UpdateDestinationRectangle(); 
            }
        }

        #region XML Docs
        /// <summary>
        /// Gets and sets the bottom side of the destination rectangle (where on the window
        /// the camera will display).  Measured in pixels.   Destination uses an inverted Y (positive points down).
        /// </summary>
        #endregion
        public virtual float BottomDestination
        {
            get { return mBottomDestination; }
            set 
            { 
                mBottomDestination = value;
                mUsesSplitScreenViewport = false;
                UpdateDestinationRectangle();
            }
        }

        #region XML Docs
        /// <summary>
        /// Gets and sets the left side of the destination rectangle (where on the window
        /// the camera will display).  Measured in pixels.
        /// </summary>
        #endregion
        public virtual float LeftDestination
        {
            get { return mLeftDestination; }
            set 
            { 
                mLeftDestination = value;
                mUsesSplitScreenViewport = false;
                UpdateDestinationRectangle();
            }
        }

        #region XML Docs
        /// <summary>
        /// Gets and sets the right side of the destination rectangle (where on the window
        /// the camera will display).  Measured in pixels.
        /// </summary>
        #endregion
        public virtual float RightDestination
        {
            get { return mRightDestination; }
            set 
            { 
                mRightDestination = value;
                mUsesSplitScreenViewport = false;
                UpdateDestinationRectangle();
            }
        }

        #region XML Docs
        /// <summary>
        /// Represents the top left justified area the Camera will draw over.
        /// </summary>
        /// <remarks>
        /// This represents the area in pixel coordinates that the camera will display relative
        /// to the top left of the owning Control.  If the Control is resized, the camera should modify
        /// its DestinationRectangle to match the new area.
        /// 
        /// <para>
        /// Multiple cameras with different DestinationRectangles can be used to display split screen
        /// or picture-in-picture.
        /// </para>
        /// </remarks>
        #endregion
        public virtual Rectangle DestinationRectangle
        {
            get
            {
                return mDestinationRectangle;
            }
            set
            {
                mUsesSplitScreenViewport = false;

                mDestinationRectangle = value;

                mTopDestination = mDestinationRectangle.Top;
                mBottomDestination = mDestinationRectangle.Bottom;
                mLeftDestination = mDestinationRectangle.Left;
                mRightDestination = mDestinationRectangle.Right;

                FixAspectRatioYConstant();
            }
        }


        public float AbsoluteRightXEdgeAt(float absoluteZ)
        {
            return Position.X + RelativeXEdgeAt(absoluteZ);
        }


        public float AbsoluteLeftXEdgeAt(float absoluteZ)
        {
            return Position.X - RelativeXEdgeAt(absoluteZ);
        }


        public float AbsoluteTopYEdgeAt(float absoluteZ)
        {
            return Position.Y + RelativeYEdgeAt(absoluteZ);
        }


        public float AbsoluteBottomYEdgeAt(float absoluteZ)
        {
            return Position.Y - RelativeYEdgeAt(absoluteZ);
        }

        public bool ClearsDepthBuffer
        {
            get;
            set;
        }

        #region XML Docs
        /// <summary>
        /// The width/height of the view of the camera
        /// </summary>
        /// <remarks>
        /// This determines the ratio of the width to height of the camera.  By default, the aspect ratio is 4/3,
        /// but this should be changed for widescreen monitors or in situations using multiple cameras.  For example, if
        /// a game is in split screen with a vertical split, then each camera will show the same height, but half the width.
        /// The aspect ratio should be 2/3.
        /// </remarks>
        #endregion
        public float AspectRatio
        {
            get { return mAspectRatio; }
            set
            {
                mAspectRatio = value;
                mXEdge = mYEdge * AspectRatio;
                // The user may expect AspectRatio to work when in 2D mode
                if (mOrthogonal)
                {
                    mOrthogonalWidth = mOrthogonalHeight * mAspectRatio;
                }
            }
        }



        public bool Orthogonal
        {
            get { return mOrthogonal; }
            set 
            { 
                mOrthogonal = value; 
            }
        }



        #region XML Docs
        /// <summary>
        /// Whether the camera draws its layers.
        /// </summary>
        #endregion
        public bool DrawsCameraLayer
        {
            get { return mDrawsCameraLayer; }
            set { mDrawsCameraLayer = value; }
        }

        #region XML Docs
        /// <summary>
        /// Whether the Camera draws world objects (objects not on the Camera's Layer)
        /// </summary>
        #endregion
        public bool DrawsWorld
        {
            get { return mDrawsWorld; }
            set { mDrawsWorld = value; }
        }

        #region XML Docs
        /// <summary>
        /// Whether the Camera draws shapes
        /// </summary>
        #endregion
        public bool DrawsShapes
        {
            get { return mDrawsShapes; }
            set { mDrawsShapes = value; }
        }

        public bool DrawsToScreen
        {
            get;
            set;
        }


        public float OrthogonalWidth
        {
            get { return mOrthogonalWidth; }
            set 
            { 
#if DEBUG
                if(value < 0)
                {
                    throw new Exception("OrthogonalWidth must be positive");
                }
#endif
                mOrthogonalWidth = value; 
            }
        }


        public float OrthogonalHeight
        {
            get { return mOrthogonalHeight; }
            set
            { 
#if DEBUG
                if (value < 0)
                {
                    throw new Exception("OrthogonalHeight must be positive");
                }
#endif
                mOrthogonalHeight = value; 
            }
        }

        #endregion

        #region XML Docs
        /// <summary>
        /// Sets the aspectRatio to match the width/height of the area that the camera is drawing to.
        /// </summary>
        /// <remarks>
        /// This is usually used in applications with split screen or when on a widescreen display.
        /// </remarks>
        #endregion
        public void FixAspectRatioYConstant()
        {
            // We may have a 0-height
            // DestinationRectangle in
            // test scenarios.
            if (mDestinationRectangle.Height != 0)
            {
                this.AspectRatio = (float)mDestinationRectangle.Width / (float)mDestinationRectangle.Height;

                mOrthogonalWidth = mOrthogonalHeight * mAspectRatio;
            }
        }


        public void FixAspectRatioXConstant()
        {
            float oldWidth = mOrthogonalWidth;
            float newAspectRatio = mDestinationRectangle.Width / (float)mDestinationRectangle.Height;
            this.FieldOfView *= newAspectRatio / mAspectRatio;
            AspectRatio = newAspectRatio;

            mOrthogonalWidth = oldWidth;
            mOrthogonalHeight = mOrthogonalWidth / mAspectRatio;

        }




        public bool IsSpriteInView(Sprite sprite)
        {
            return IsSpriteInView(sprite, false);
        }

        static float mLongestDimension;
        static float mDistanceFromCamera;
        public bool IsSpriteInView(Sprite sprite, bool relativeToCamera)
        {
            switch (CameraCullMode)
            {

                case CameraCullMode.UnrotatedDownZ:
                    {
                        mLongestDimension = (float)(System.Math.Max(sprite.ScaleX, sprite.ScaleY) * 1.42f);


                        if (mOrthogonal)
                        {
                            if (relativeToCamera)
                            {
                                if (System.Math.Abs(sprite.X) - mLongestDimension > mOrthogonalWidth)
                                {
                                    return false;
                                }
                                if (System.Math.Abs(sprite.Y) - mLongestDimension > mOrthogonalHeight)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (System.Math.Abs(X - sprite.X) - mLongestDimension > mOrthogonalWidth)
                                {
                                    return false;
                                }
                                if (System.Math.Abs(Y - sprite.Y) - mLongestDimension > mOrthogonalHeight)
                                {
                                    return false;
                                }
                            }
                        }
                        else // if (camera.cull)
                        {
                            // Multiply by 1.5 to increase the range in case the Camera is rotated
                            if (relativeToCamera)
                            {
#if FRB_MDX
                                mDistanceFromCamera = (sprite.Z) / 100.0f;

#else
                                mDistanceFromCamera = (-sprite.Z) / 100.0f;
#endif
                                if (System.Math.Abs(sprite.X) - mLongestDimension > mXEdge * 1.5f * mDistanceFromCamera)
                                {
                                    return false;
                                }
                                if (System.Math.Abs(sprite.Y) - mLongestDimension > mYEdge * 1.5f * mDistanceFromCamera)
                                {
                                    return false;
                                }
                            }
                            else
                            {
#if FRB_MDX
                                mDistanceFromCamera = (sprite.Z - Z) / 100.0f;

#else
                                mDistanceFromCamera = (Z - sprite.Z) / 100.0f;
#endif
                                if (System.Math.Abs(X - sprite.X) - mLongestDimension > mXEdge * 1.5f * mDistanceFromCamera)
                                {
                                    return false;
                                }
                                if (System.Math.Abs(Y - sprite.Y) - mLongestDimension > mYEdge * 1.5f * mDistanceFromCamera)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                case CameraCullMode.None:
                    return true;
            }
            return true;

        }

        public bool IsTextInView(Text text)
        {
            if(this.CameraCullMode == Graphics.CameraCullMode.UnrotatedDownZ)
            {
                float cameraLeft = this.AbsoluteLeftXEdgeAt(text.Z);
                float cameraRight = this.AbsoluteRightXEdgeAt(text.Z);
                float cameraTop = this.AbsoluteTopYEdgeAt(text.Z);
                float cameraBottom = this.AbsoluteBottomYEdgeAt(text.Z);

                float textVerticalCenter = text.VerticalCenter;
                float textHorizontalCenter = text.HorizontalCenter;

                float longestCenterToEdge
                    = (float)(System.Math.Max(text.Width, text.Height) * 1.42f/2.0f);


                float textLeft = textHorizontalCenter - longestCenterToEdge;
                float textRight = textHorizontalCenter + longestCenterToEdge;
                float textTop = textVerticalCenter + longestCenterToEdge;
                float textBottom = textVerticalCenter - longestCenterToEdge;

                return textRight > cameraLeft &&
                    textLeft < cameraRight &&
                    textBottom < cameraTop &&
                    textTop > cameraBottom;                
            }


            return true;
        }

        public bool IsPointInView(double x, double y, double absoluteZ)
        {
            if (mOrthogonal)
            {
                return (x > Position.X - mOrthogonalWidth / 2.0f && x < Position.X + mOrthogonalWidth / 2.0f) &&
                    y > Position.Y - mOrthogonalHeight / 2.0f && y < Position.Y + mOrthogonalHeight / 2.0f;
            }
            else
            {
#if FRB_MDX
                double cameraDistance = (absoluteZ - Position.Z) / 100.0;
#else
                double cameraDistance = (Position.Z - absoluteZ) / 100.0;
#endif
                if (x > Position.X - mXEdge * cameraDistance && x < Position.X + mXEdge * cameraDistance &&
                    y > Position.Y - mYEdge * cameraDistance && y < Position.Y + mYEdge * cameraDistance)
                    return true;
                else
                    return false;
            }
        }

        #region XML Docs
        /// <summary>
        /// Determines if the X value is in view, assuming the camera is viewing down the Z axis.
        /// </summary>
        /// <remarks>
        /// Currently, this method assumes viewing down the Z axis.
        /// </remarks>
        /// <param name="x">The absolute X position of the point.</param>
        /// <param name="absoluteZ">The absolute Z position of the point.</param>
        /// <returns></returns>
        #endregion
        public bool IsXInView(double x, double absoluteZ)
        {
            if (mOrthogonal)
            {
                return (x > Position.X - mOrthogonalWidth / 2.0f && x < Position.X + mOrthogonalWidth / 2.0f);
            }
            else
            {
#if FRB_MDX
                double cameraDistance = (absoluteZ - Position.Z) / 100.0;
#else
                double cameraDistance = (Position.Z - absoluteZ) / 100.0;
#endif
                if (x > Position.X - mXEdge * cameraDistance && x < Position.X + mXEdge * cameraDistance)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Determines if the Y value is in view, assuming the camera is viewing down the Z axis.
        /// </summary>
        /// <remarks>
        /// Currently, this method assumes viewing down the Z axis.
        /// </remarks>
        /// <param name="y">The absolute Y position of the point.</param>
        /// <param name="absoluteZ">The absolute Z position of the point.</param>
        /// <returns></returns>
        public bool IsYInView(double y, double absoluteZ)
        {
            if (mOrthogonal)
            {
                return y > Position.Y - mOrthogonalHeight / 2.0f && y < Position.Y + mOrthogonalHeight / 2.0f;
            }
            else
            {
#if FRB_MDX
                double cameraDistance = (absoluteZ - Position.Z) / 100.0;
#else
                double cameraDistance = (Position.Z - absoluteZ) / 100.0;
#endif
                if (y > Position.Y - mYEdge * cameraDistance && y < Position.Y + mYEdge * cameraDistance)
                    return true;
                else
                    return false;
            }
        }


        #region XML Docs
        /// <summary>
        /// Returns the number of pixels per unit at the given absolute Z value.  Assumes
        /// that the Camera is unrotated.
        /// </summary>
        /// <remarks>
        /// If using the PixelsPerUnitAt for a rotated camera, use the overload which
        /// takes a Vector3 argument.
        /// </remarks>
        /// <param name="absoluteZ">The absolute Z position.</param>
        /// <returns>The number of pixels per world unit (perpendicular to the camera's forward vector).</returns>
        #endregion
        public float PixelsPerUnitAt(float absoluteZ)
        {
            // June 7, 2011
            // This used to use
            // width values, but
            // that means aspect ratio
            // can screw with these values
            // which we don't want.  Instead
            // we should use height, as that is
            // usually what FRB games use as their
            //// fixed dimension
            //if (mOrthogonal)
            //{
            //    return mDestinationRectangle.Width / mOrthogonalWidth;
            //}
            //else
            //{
            //    return mDestinationRectangle.Width / (2 * RelativeXEdgeAt(absoluteZ));
            //}

            if (mOrthogonal)
            {
                return mDestinationRectangle.Height / mOrthogonalHeight;
            }
            else
            {
                return mDestinationRectangle.Height / (2 * RelativeYEdgeAt(absoluteZ));
            }

        }


        public float PixelsPerUnitAt(ref Vector3 absolutePosition)
        {

            return PixelsPerUnitAt(ref absolutePosition, mFieldOfView, mOrthogonal, mOrthogonalHeight);
        }


        public float PixelsPerUnitAt(ref Vector3 absolutePosition, float fieldOfView, bool orthogonal, float orthogonalHeight)
        {
            if (orthogonal)
            {
                return mDestinationRectangle.Height / orthogonalHeight;
            }
            else
            {
#if FRB_XNA || SILVERLIGHT || WINDOWS_PHONE
                float distance = Vector3.Dot(
                    (absolutePosition - Position), RotationMatrix.Forward);
#elif FRB_MDX

            Vector3 forwardCameraVector = new Vector3(
                mRotationMatrix.M31,
                mRotationMatrix.M32,
                mRotationMatrix.M33);

            float distance = Vector3.Dot(
                (absolutePosition - Position), forwardCameraVector);

#endif
                return mDestinationRectangle.Height /
                    (2 * RelativeYEdgeAt(Position.Z + (Math.MathFunctions.ForwardVector3.Z * distance), fieldOfView, mAspectRatio, orthogonal, orthogonalHeight));
            }
        }



        public float RelativeXEdgeAt(float absoluteZ)
        {
            return RelativeXEdgeAt(absoluteZ, mFieldOfView, mAspectRatio, mOrthogonal, mOrthogonalWidth);
        }


        public float RelativeXEdgeAt(float absoluteZ, float fieldOfView, float aspectRatio, bool orthogonal, float orthogonalWidth)
        {
            if (orthogonal)
            {
                return orthogonalWidth / 2.0f;
            }
            else
            {
                float yEdge = (float)(100 * System.Math.Tan(fieldOfView / 2.0));
                float xEdge = yEdge * aspectRatio;
                return xEdge * (absoluteZ - Position.Z) / 100 * FlatRedBall.Math.MathFunctions.ForwardVector3.Z;
            }
        }


        public float RelativeYEdgeAt(float absoluteZ)
        {
            return RelativeYEdgeAt(absoluteZ, mFieldOfView, AspectRatio, Orthogonal, OrthogonalHeight);
        }


        public float RelativeYEdgeAt(float absoluteZ, float fieldOfView, float aspectRatio, bool orthogonal, float orthogonalHeight)
        {
            if (orthogonal)
            {
                // fieldOfView is ignored if it's Orthogonal
                return orthogonalHeight / 2.0f;
            }
            else
            {
                float yEdge = (float)(System.Math.Tan(fieldOfView / 2.0));

                return yEdge * (absoluteZ - Position.Z) / FlatRedBall.Math.MathFunctions.ForwardVector3.Z;
            }
        }


        #region XML Docs
        /// <summary>
        /// Sets the camera to Orthogonal, sets the OrthogonalWidth and
        /// OrthogonalHeight to match the argument values, and can move the
        /// so the bottom-left corner of the screen is at the origin.
        /// </summary>
        /// <param name="moveCornerToOrigin">Whether the camera should be repositioned
        /// so the bottom left is at the origin.</param>
        /// <param name="desiredWidth">The desired unit width of the view.</param>
        /// <param name="desiredHeight">The desired unit height of the view.</param>
        #endregion
        public void UsePixelCoordinates(bool moveCornerToOrigin, int desiredWidth, int desiredHeight)
        {
            this.Orthogonal = true;
            OrthogonalWidth = desiredWidth;
            OrthogonalHeight = desiredHeight;

            if (moveCornerToOrigin)
            {
                X = OrthogonalWidth / 2.0f;
                Y = OrthogonalHeight / 2.0f;
            }
        }

        public void UsePixelCoordinates3D(float zToMakePixelPerfect)
        {
            double distance = GetZDistanceForPixelPerfect();
            this.Z = -MathFunctions.ForwardVector3.Z * (float)(distance);
            this.FarClipPlane = System.Math.Max(this.FarClipPlane,
                (float)distance * 2);
        }

        public float GetZDistanceForPixelPerfect()
        {
            double sin = System.Math.Sin(FieldOfView / 2.0);
            double cos = System.Math.Cos(FieldOfView / 2.0f);


            double edgeToEdge = 2 * sin;
            float desiredHeight = this.DestinationRectangle.Height;

            double distance = cos * desiredHeight / edgeToEdge;
            return (float)distance;
        }


        public float WorldXAt(float screenX, float zPosition)
        {
            return WorldXAt(screenX, zPosition, this.Orthogonal, this.OrthogonalWidth);
        }

        public float WorldXAt(float screenX, float zPosition, Layer layer)
        {
            if (layer == null || layer.LayerCameraSettings == null)
            {
                return WorldXAt(screenX, zPosition, this.Orthogonal, this.OrthogonalWidth);
            }
            else
            {
                LayerCameraSettings lcs = layer.LayerCameraSettings;

                Camera cameraToUse = layer.CameraBelongingTo;

                if (cameraToUse == null)
                {
                    cameraToUse = this;
                }

                // If the orthogonal resolution per destination width/height
                // of the layer matches the Camera's orthogonal per destination, then
                // we can just use the camera.  This is the most common case so we'll just
                // use that.
                float destinationLeft = cameraToUse.DestinationRectangle.Left;
                float destinationRight = cameraToUse.DestinationRectangle.Right;


                float destinationWidth = destinationRight - destinationLeft;

                float horizontalPercentage = (screenX - destinationLeft) / (float)destinationWidth;

                float orthogonalWidthToUse = cameraToUse.OrthogonalWidth;
                if (lcs.Orthogonal && !cameraToUse.Orthogonal)
                {
                    orthogonalWidthToUse = lcs.OrthogonalWidth;
                }

                // used for adjusting the ortho width/height if the Layer is zoomed
                float layerMultiplier = 1;
                float bottomDestination = lcs.BottomDestination;
                float topDestination = lcs.TopDestination;
                if (bottomDestination == -1 || topDestination == -1)
                {
                    bottomDestination = cameraToUse.BottomDestination;
                    topDestination = cameraToUse.TopDestination;
                }

                if (lcs.Orthogonal && bottomDestination != topDestination)
                {
                    layerMultiplier = lcs.OrthogonalHeight / (float)(bottomDestination - topDestination);
                }

                float cameraMultiplier = 1;
                if (cameraToUse.Orthogonal && cameraToUse.BottomDestination != cameraToUse.TopDestination)
                {
                    cameraMultiplier = cameraToUse.OrthogonalHeight / (float)(cameraToUse.BottomDestination - cameraToUse.TopDestination);
                }
                layerMultiplier /= cameraMultiplier;

                orthogonalWidthToUse *= layerMultiplier;

                // I think we want to use the Camera's orthogonalWidth if it's orthogonal
                //return GetWorldXGivenHorizontalPercentage(zPosition, cameraToUse, lcs.Orthogonal, lcs.OrthogonalWidth, horizontalPercentage);
                return GetWorldXGivenHorizontalPercentage(zPosition, lcs.Orthogonal, orthogonalWidthToUse, horizontalPercentage);
            }
        }

        public float WorldXAt(float screenX, float zPosition, bool overridingOrthogonal, float overridingOrthogonalWidth)
        {
            float screenRelativeX = screenX;
            return WorldXAt(zPosition, overridingOrthogonal, overridingOrthogonalWidth, screenRelativeX);
        }

        public float WorldXAt(float zPosition, bool overridingOrthogonal, float overridingOrthogonalWidth, float screenX)
        {

            float horizontalPercentage = (screenX - this.DestinationRectangle.Left) / (float)this.DestinationRectangle.Width;

            return GetWorldXGivenHorizontalPercentage(zPosition, overridingOrthogonal, overridingOrthogonalWidth, horizontalPercentage);
        }

        private float GetWorldXGivenHorizontalPercentage(float zPosition, bool overridingOrthogonal, float overridingOrthogonalWidth, float horizontalPercentage)
        {
            if (!overridingOrthogonal)
            {
                float absoluteLeft = this.AbsoluteLeftXEdgeAt(zPosition);
                float width = this.RelativeXEdgeAt(zPosition) * 2;
                return absoluteLeft + width * horizontalPercentage;
            }
            else
            {
                float xDistanceFromEdge = horizontalPercentage * overridingOrthogonalWidth;
                return (this.X + -overridingOrthogonalWidth / 2.0f + xDistanceFromEdge);
            }
        }


        public float WorldYAt(float screenY, float zPosition)
        {
            return WorldYAt(screenY, zPosition, this.Orthogonal, this.OrthogonalHeight);
        }

        public float WorldYAt(float screenY, float zPosition, Layer layer)
        {
            if (layer == null || layer.LayerCameraSettings == null)
            {
                return WorldYAt(screenY, zPosition, this.Orthogonal, this.OrthogonalHeight);
            }
            else
            {
                LayerCameraSettings lcs = layer.LayerCameraSettings;

                Camera cameraToUse = layer.CameraBelongingTo;

                if (cameraToUse == null)
                {
                    cameraToUse = this;
                }


                // If the orthogonal resolution per destination width/height
                // of the layer matches the Camera's orthogonal per destination, then
                // we can just use the camera.  This is the most common case so we'll just
                // use that.
                //return WorldYAt(zPosition, cameraToUse, lcs.Orthogonal, lcs.OrthogonalHeight);
                // If we have a 2D layer ona 3D camera, then we shouldn't use the Camera's orthogonal values
                float orthogonalHeightToUse = cameraToUse.OrthogonalHeight;
                if (lcs.Orthogonal && !cameraToUse.Orthogonal)
                {
                    orthogonalHeightToUse = lcs.OrthogonalHeight;
                }

                float layerMultiplier = 1;
                float bottomDestination = lcs.BottomDestination;
                float topDestination = lcs.TopDestination;
                if (bottomDestination == -1 || topDestination == -1)
                {
                    bottomDestination = cameraToUse.BottomDestination;
                    topDestination = cameraToUse.TopDestination;
                }

                if (lcs.Orthogonal && bottomDestination != topDestination)
                {
                    layerMultiplier = lcs.OrthogonalHeight / (float)(bottomDestination - topDestination);
                }

                float cameraMultiplier = 1;
                if (cameraToUse.Orthogonal && cameraToUse.BottomDestination != cameraToUse.TopDestination)
                {
                    cameraMultiplier = cameraToUse.OrthogonalHeight / (float)(cameraToUse.BottomDestination - cameraToUse.TopDestination);
                }
                layerMultiplier /= cameraMultiplier;


                orthogonalHeightToUse *= layerMultiplier;

                return WorldYAt(screenY, zPosition, lcs.Orthogonal, orthogonalHeightToUse);
            }
        }

        public float WorldYAt(float screenY, float zPosition, bool overridingOrthogonal, float overridingOrthogonalHeight)
        {
            float screenRelativeY = screenY;

            return WorldYAt(zPosition, overridingOrthogonal, overridingOrthogonalHeight, screenRelativeY);
        }

        public float WorldYAt(float zPosition, bool orthogonal, float orthogonalHeight, float screenY)
        {
            float verticalPercentage = (screenY - this.DestinationRectangle.Top) / (float)this.DestinationRectangle.Height;

            if (!orthogonal)
            {
                float absoluteTop = this.AbsoluteTopYEdgeAt(zPosition);
                float height = this.RelativeYEdgeAt(zPosition) * 2;
                return absoluteTop - height * verticalPercentage;
            }
            else
            {
                float yDistanceFromEdge = verticalPercentage * orthogonalHeight;
                return (this.Y + orthogonalHeight / 2.0f - yDistanceFromEdge);
            }
        }






    }
    
    
}