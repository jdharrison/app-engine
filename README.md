# AppEngine (v0.0.2)

## Summary

Unity (5.x.x) framework for organization and development of any application.

## Features

~ State: Handles all state management in the application, only edit the respective scene for a state and the framework will take care of the rest.

~ Profiles: Provides access to light-weight basic data which is stored relative to a profile. You can choose, create, and manage individual profiles for easier development.

~ Server (TBD): Simple server management, supports HTTP and Socket servers. You can then interact with a simple interface to make HTTP requests and/or (UDP & TCP) socket communications.

~ Data (TBD): Interactive editor for managing your data collections, they can be then be interacted with at run-time. This allows you to manage heavier data that is stored in cache, and may be saved to disk.

### Set Up

1) Create a new Unity 5+ project.
2) Download the unitypackage in this repositories bin folder.
3) Once downloaded: go to Assets -> Import Package -> Custom Package then find where you saved the unitypackage.
4) Open up File -> Building Settings, then drag Initializer and all State scenes into: 'Scenes in Build'.

### How To Use

- Edit State: Navigate to Scenes/States folder. Open the scene file and edit it how desired, then save the scene.
- Play Application: Open up the Initializer scene, and hit the Play button.