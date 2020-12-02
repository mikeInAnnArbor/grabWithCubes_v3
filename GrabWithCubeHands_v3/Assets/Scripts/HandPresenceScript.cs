using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresenceScript : MonoBehaviour
{
    // allows display of either controller or hands!
    public bool showController = false;

    public InputDeviceCharacteristics controllerCharacteristics;

    // list of all prefabs availabel to the project
    public  List<GameObject> controllerPrefabs;

    // for displaying the hands
    public GameObject handModelPrefab;

    
    // save the one device I want to be tracking (in Valem's design)
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;

    // Start is called before the first frame update
    void Start()
    {
        // get a list of devices, then pick just the right controller
        List<InputDevice> devices = new List<InputDevice>();
// 2 lines below were  for earlier in the video. 
// comment it out and get ALL controllers, not just the Right one
//        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
//        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);


        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }      

        // being careful, check number of devices
        if (devices.Count > 0)
        {
            // then assign the first one we found to the global var 
            targetDevice = devices[0];
            Debug.Log("targetDevice=" + targetDevice.name);
            // go find the controller we are actually using
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            // if we find something, instantiate/spawn it
            // then store it using spawnedController vvar above
            if (prefab) 
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                // and just in case we didn't bring in the model
                // or the name is wrong or whatevver:
                Debug.LogError("did not find corresponding controller model");
                // use a default from thelist
                spawnedController = Instantiate(controllerPrefabs[0],transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else 
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
            }
    }

    void JunkLearningDebugCode ()
    {
        // now the whole point: check what buttons are happening
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue) 
        { 
            Debug.Log("pre button");
        }         

        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            Debug.Log("trigger value = " + triggerValue);
        }

        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Parimary touchpad val = " + primary2DAxisValue);
        }
    }
}
