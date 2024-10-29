Edugaze

EduGaze is an augmented reality (AR) application that enhances the learning experience by allowing teachers to integrate interactive 3D models into their lessons. This innovative tool helps students visualize complex concepts, such as the mechanics of a lathe machine, promoting deeper engagement and understanding.

Setup and Build Guide for iOS. 
This guide assumes familiarity with Unity, Xcode, and iOS development.
Prerequisites Unity: Install the version used for this project (e.g., Unity 2021 LTS). 
Xcode: Ensure you have the latest stable version installed.
Apple Developer Account: Required to deploy the app on an iOS device.
Git: Ensure Git is installed to clone the repository.

Step-by-Step Setup
1. Clone the Repository bash Copy code git clone https://github.com/your-username/edugaze.git  edugaze
2. Open the Project in Unity. Launch Unity Hub and select the specified Unity version. Click on Open Project and navigate to the cloned folder. Allow Unity to load and compile any assets/scripts.
3. Install Required Packages. Open Package Manager (Window > Package Manager). Install any missing packages, ensuring AR Foundation, ARKit, and any required eye-tracking packages are imported.
4. Project Configuration Set Build Target: Go to File > Build Settings. Set the platform to iOS and click Switch Platform. Configure Player Settings: In Build Settings, click on Player Settings. Under Other Settings, configure: Bundle Identifier (e.g., com.yourcompany.edugaze). iOS Version: Ensure compatibility (e.g., iOS 14.0 or higher). Enable ARKit support in XR Plug-in Management for iOS.
5. Prepare Assets and Scenes. Ensure all required scenes (e.g., main gaze tracking scene) are included in the Scenes in Build section. Verify that key assets like eye-tracking UI and tutorial screens are configured and active.
6. Build the Project In Build Settings, click Build. Choose a folder to save the Xcode project (e.g., EduGaze_iOS_Build). Unity will create the Xcode workspace.
7. Open in Xcode. Open the generated Xcode project (EduGaze.xcodeproj). Check your Signing & Capabilities: Under General, set your Team to your Apple Developer account. Ensure Automatically manage signing is enabled. Verify device compatibility and deployment target under Build Settings.
8. Deploy to iOS Device. Connect your iOS device to your Mac. In Xcode, select your connected device as the target and click Run. Xcode will compile and install the app on your device.
