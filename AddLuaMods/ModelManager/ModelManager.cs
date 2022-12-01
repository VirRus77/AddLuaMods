using System.Collections.Generic;
using AddLuaMods.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AddLuaMods.ModelManager
{
    public class ModelManager : BoxClass<global::ModelManager>
    {
        public ModelManager(global::ModelManager instance)
            : base(instance)
        {
        }

        public GameObject Load(ObjectType NewType, string Name, bool RandomVariants)
        {
            //var sb = new StringBuilder();
            //foreach (var extraModel in m_ExtraModels)
            //{
            //    sb.AppendLine($" - {extraModel.Key}");
            //}
            //Logging.LogDebug($"ModelManager.m_ExtraModels :\n{sb}");

            //Logging.LogDebug("ModelManager.Load");
            if (NewType >= ObjectType.Total && NewType < global::ObjectTypeList.m_Total)
            {
                string ModelName = "";
                return global::ModManager.Instance.IsModelUsingCustomModel(NewType, out ModelName)
                    ? global::ModManager.Instance.GetModModel(NewType)
                    : global::AssetManager.LoadModel(ModelName);
            }
            //Logging.LogDebug("ModelManager.Load (a)");

            if (NewType == global::ObjectTypeList.m_Total)
                return m_ExtraModels[Name];
            //Logging.LogDebug("ModelManager.Load (b)");
            if (!m_Models.ContainsKey(NewType))
                m_Models.Add(NewType, new ModelInfo(false));
            
            //Logging.LogDebug("ModelManager.Load (c)");
            ModelInfo model = m_Models[NewType];

            //Logging.LogDebug("ModelManager.Load (d)");
            if (!RandomVariants || model.m_VariantModels.Count <= 0)
                return m_ExtraModels[Name];

            //Logging.LogDebug("ModelManager.Load (e)");
            List<GameObject> variantModels = model.m_VariantModels;
            int index = Random.Range(0, variantModels.Count);
            //Logging.LogDebug($"ModelManager.Load (f) variantModels.Count: {variantModels.Count}, index: {index}");
            return variantModels[index];
        }

        public void AddModelType(ObjectType objectType)
        {
            InvokeMethod(nameof(AddModelType), objectType);
        }

        public void AddModel(string Name, ObjectType NewType, bool RandomVariants = false, bool ForceBuilding = false)
        {
            Logging.LogDebug($"ModelManager.AddModel \"{Name}\" {NewType} {RandomVariants} {ForceBuilding}");
            _instance.AddModel(Name, NewType, RandomVariants, ForceBuilding);
        }

        public void MakeModelList()
        {
            if (global::ObjectTypeList.Instance == null)
            {
                global::ObjectTypeList objectTypeList = new global::ObjectTypeList();
            }
            if (m_TestObject != global::ObjectTypeList.m_Total)
            {
                AddModelType(m_TestObject);
            }
            else
            {
                for (int index = 0; index < (int)ObjectType.Total; ++index)
                    AddModelType((ObjectType)index);
            }
            AddModel("Models/Buildings/BuildingAccessPoint", global::ObjectTypeList.m_Total, false, true);
            AddModel("Models/Buildings/BuildingAccessPointIn", global::ObjectTypeList.m_Total, false, true);
            AddModel("Models/Buildings/BuildingSpawnPoint", global::ObjectTypeList.m_Total, false, true);
            AddModel("Models/Buildings/BuildingBlueprintTile", global::ObjectTypeList.m_Total, false, true);
            AddModel("Models/Special/MechanicalPulley", global::ObjectTypeList.m_Total, false, true);
            AddModel("Models/Special/ResearchVessel", global::ObjectTypeList.m_Total, false, false);
        }

        public ObjectType m_TestObject
        {
            get => GetField<ObjectType>(nameof(m_TestObject), true);
            set => SetField<ObjectType>(nameof(m_TestObject), value, true);
        }

        public Dictionary<string, GameObject> m_ExtraModels
        {
            get => GetField<Dictionary<string, GameObject>>(nameof(m_ExtraModels));
        }
        public Dictionary<ObjectType, ModelInfo> m_Models
        {
            get => GetField<Dictionary<ObjectType, ModelInfo>>(nameof(m_Models));
        }
    }
}
