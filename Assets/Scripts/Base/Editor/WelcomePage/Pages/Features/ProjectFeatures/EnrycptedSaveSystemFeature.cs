using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class EnrycptedSaveSystemFeature : ProjectFeatures
    {
        public EnrycptedSaveSystemFeature()
        {
            Title = "Encrypted Save System";
            Description = "Encrypted Save System is a custom saving solution that converts any object into a persistable asset using a proprietary encryption algorithm.\n" +
                "It supports multiple saving methods: Encrypted JSON, Normal JSON, PlayerPrefs, and EncryptedAsyncJSON (the latter is not yet implemented in this version).\n\n" +
                "Additionally, this system features a custom Enum Creator that, when used properly, significantly simplifies data access and storage.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Base/Runtime/Save System/SerializedSaveData.cs"),
                new ReferenceToClasses("Assets/Scripts/Base/Runtime/Save System/SaveDataHelper.cs"),
                new ReferenceToClasses("Assets/Scripts/Base/Runtime/Save System/DictionarySaveType.cs"),
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/SaveSystem/GameDataSave.cs"),
                new ReferenceToClasses("Assets/Resources/Saves/GameDataSave.asset")
            };
        }
    }
}