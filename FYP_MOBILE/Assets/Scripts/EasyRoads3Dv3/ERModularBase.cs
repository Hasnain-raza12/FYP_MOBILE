using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERModularBase : MonoBehaviour
	{
		public int updateInt;

		public int toolbarInt;

		public Texture[] menuTexs;

		public GameObject cprefab;

		public Transform roadObjectsParent;

		public Transform connectionObjectsParent;

		public GameObject OQCCODDOOQ;

		public List<QDQDOOQQDQODD> roadTypes;

		public int selectedRoadType;

		public int selectedNewRoadType;

		public float roadWidth;

		public Material roadMaterial;

		public Material crossingMaterial;

		public Material roundAboutMaterial;

		public Material roundAboutConnectionMaterial;

		public Material roundAboutRoadMaterial;

		public Material sidewalkMaterial;

		public string[] roadMaterials;

		public string[] connectionMaterials;

		public int selectedMaterial;

		public int selectedConnectionMaterial;

		public int handleSelection;

		public int positionHandleSelection;

		public GameObject defaultCrossing;

		public GameObject defaultTCrossing;

		public GameObject defaultCulDeSac;

		public GameObject defaultRoundabout;

		public Texture2D tex;

		public bool showAllPrefabs;

		public bool standardPrefabsFlag;

		public bool sceneSettingsFoldOut;

		public bool sceneRoadsFoldOut;

		public bool scenePrefabsFoldOut;

		public bool terrainManagementFoldOut;

		public bool importRoadDataFoldOut;

		public bool lodGroupsFoldOut;

		public bool defaultMaterialsFoldOut;

		public bool kmlFlag;

		public bool osmFlag;

		public bool showRoadSideObjects;

		public float osmTerrainTopLon;

		public float osmTerrainBottomLon;

		public float osmTerrainLeftLat;

		public float osmTerrainRightLat;

		public float terrainMinIndent;

		public float minIndent;

		public float minSurrounding;

		public float terrainY;

		public float raise;

		public Vector3 baseVector;

		public bool mirrorCrossings;

		public string[] terrainNames;

		public Terrain[] terrainObjects;

		public string[] terrainSplatTextures;

		public Terrain activeTerrain;

		public float activeTerrainY;

		public int selectedTerrain;

		public bool terrainDone;

		public bool enableBackWithoutRestore;

		public float detailDistance;

		public float treeDistance;

		public bool doTrees;

		public bool soTrees;

		public bool doDetail;

		public Rect terrainRect;

		public List<GameObject> surfaceObjects;

		public float preserveTerrainFloat;

		public bool doTangents;

		public bool doLightmapUVs;

		public bool doLODGroups;

		public bool doSplatmaps;

		public List<Vector3> terrainHits;

		public List<Vector3> osmCrossingPoints;

		public List<CrossingCornerClass> cornerPresets;

		public List<SidewalkPresetClass> sidewalkPresets;

		public int osmMotorway;

		public int osmTrunk;

		public int osmPrimary;

		public int osmSecondary;

		public int osmTertiary;

		public int osmUnclassified;

		public int osmResidential;

		public int osmService;

		public int osmTrack;

		public int osmPath;

		public bool osmMotorwayFlag;

		public bool osmTrunkFlag;

		public bool osmPrimaryFlag;

		public bool osmSecondaryFlag;

		public bool osmTertiaryFlag;

		public bool osmUnclassifiedFlag;

		public bool osmResidentialFlag;

		public bool osmServiceFlag;

		public bool osmTrackFlag;

		public bool osmPathFlag;

		public bool lodGroups;

		public int LODLevels;

		public List<float> LODLevelValues;

		public List<float> LODLevelResolution;

		public bool embedRoadShape;

		public bool hideSurfaces;

		public bool useLightProbes;

		public bool OQODDOQDDQ;

		public bool isInBuildMode;

		[SerializeField]
		public List<SideObject> QOQDQOOQDDQOOQ;

		public string[] sideObjectNames;

		public int selSideObject;

		public string soID;

		public string sideObjectName;

		[SerializeField]
		public int sideObjectType;

		public GameObject sideObjectSource;

		public GameObject soEndObject;

		public int sideObjectTerrainVegetationInt;

		public int prefabChildHandling;

		public float sideObjectDistance;

		public int soYAxisRotation;

		public float soSidewaysDistance;

		public int soSidewaysDistanceHandling;

		public float soDensity;

		public float soOffset;

		public int soTerrainAligment;

		public bool soCombine;

		public bool soWeld;

		public int soControllerType;

		public Material soMaterial;

		public float soXPosition;

		public float soYPosition;

		public bool soMarkerActive;

		public bool enableSOHandles;

		public bool displayCriticalPoints;

		public List<GameObject> soDeformationObjects;

		public List<GameObject> soSplatmapObjects;

		public bool buildSOinEditMode;

		public bool importSideObjectsAlert;

		public bool importRoadPresetsAlert;

		public bool importCrossingPresetsAlert;

		public bool importSidewalkPresetsAlert;

		public bool updateSideObjectsAlert;

		public bool updateRoadPresetsAlert;

		public bool updateCrossingPresetsAlert;

		public bool updateSidewalkPresetsAlert;

		public float waypointDistance;

		public List<ERModularRoad> RoadObjectsSoUpdates;

		public string assetsFolderID;

		public GameObject meshSurface;

		public Collider meshTerrainCollider;

		public float markerScale;

		public bool debugFlag;
	}
}
