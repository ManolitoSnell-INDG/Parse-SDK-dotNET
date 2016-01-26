// Copyright (c) 2015-present, Parse, LLC.  All rights reserved.  This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree.  An additional grant of patent rights can be found in the PATENTS file in the same directory.

using Parse.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Parse {
  public partial class ParseInstallation : ParseObject {
    /// <summary>
    /// The device token of the installation. Typically generated by APNS or GCM.
    /// </summary>
    [ParseFieldName("deviceToken")]
    public string DeviceToken {
      get { return GetProperty<string>("DeviceToken"); }
      internal set { SetProperty<string>(value, "DeviceToken"); }
    }

    /// <summary>
    /// Sets <see cref="ParseInstallation.DeviceToken"/> with byte array provided by APNS.
    /// </summary>
    /// <remarks>
    /// This method is only useful for iOS/MacOSX platform.
    /// </remarks>
    /// <param name="deviceToken"></param>
    public void SetDeviceTokenFromData(byte[] deviceToken) {
      StringBuilder builder = new StringBuilder();
      foreach (var b in deviceToken) {
        builder.Append(b.ToString("x2"));
      }
      DeviceToken = builder.ToString();
    }

    /// <summary>
    /// iOS Badge.
    /// </summary>
    [ParseFieldName("badge")]
    public int Badge {
      get {
        if (PlatformHooks.IsIOS) {
				  PlatformHooks.RunOnMainThread(() => {
            if (UnityEngine.NotificationServices.localNotificationCount > 0) {
              SetProperty<int>(UnityEngine.NotificationServices.localNotifications[0].applicationIconBadgeNumber, "Badge");
            }
          });
        }
        return GetProperty<int>("Badge");
      }
      set {
        int badge = value;
        SetProperty<int>(badge, "Badge");
        if (PlatformHooks.IsIOS) {
          PlatformHooks.RunOnMainThread(() => {
            UnityEngine.LocalNotification notification = new UnityEngine.LocalNotification ();
            notification.applicationIconBadgeNumber = badge;
            notification.hasAction = false;
            UnityEngine.NotificationServices.PresentLocalNotificationNow(notification);
          });
        }
      }
    }
  }
}
