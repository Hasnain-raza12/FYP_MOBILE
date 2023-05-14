using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class RoadObjectScriptV3 : MonoBehaviour
	{
		public int objectType;

		public bool displayRoad;

		public float roadWidth;

		public float indent;

		public float surrounding;

		public float raise;

		public float raiseMarkers;

		public bool OOQDOOQQ;

		public bool renderRoad;

		public bool beveledRoad;

		public bool applySplatmap;

		public int splatmapLayer;

		public bool autoUpdate;

		public float geoResolution;

		public int roadResolution;

		public float tuw;

		public int splatmapSmoothLevel;

		public float opacity;

		public int expand;

		public int offsetX;

		public int offsetY;

		public Material surfaceMaterial;

		public float surfaceOpacity;

		public float smoothDistance;

		public float smoothSurDistance;

		public bool handleInsertFlag;

		public bool ODODQQCCQC;

		public float OCQQQDCDOQ;

		public float OCOOQDDDQC;

		public int materialType;

		public string[] materialStrings;

		public MarkerScriptV3[] mSc;

		public bool ODQQCODDCQ;

		public bool[] menuStatus;

		public bool[] oldMenuStatus;

		public string[] menuStrings;

		public string[] textLayers;

		public int[] textLayersInt;

		public int OOQCQQDCCQ;

		public int ODQQODDDDC;

		public bool OOQDOCDOCO;

		public Vector3 cPos;

		public Vector3 ePos;

		public bool OOQQDDQQQC;

		public int markers;

		public GameObject ODOQDQOO;

		public bool OCCDDODQOQ;

		public bool doTerrain;

		public Transform OOQCDOCDOQ;

		public GameObject[] OOQCDOCDOQs;

		public Transform obj;

		public string ODODQCOQOD;

		public RoadObjectScriptV3 OOQCOOCQCC;

		public bool flyby;

		public Vector3 pos;

		public float fl;

		public float oldfl;

		public bool OOOOOQOOOQ;

		public bool OQODDDOQQQ;

		public bool OOODCQCDDQ;

		public Transform OOCCCQDQCC;

		public int OdQODQOD;

		public float OOQQQDOD;

		public float OOQQQDODOffset;

		public float OOQQQDODLength;

		public bool ODODDDOO;

		public int ODQDOOQO;

		public string[] ODQQQQQO;

		public string[] ODODDQOO;

		public bool[] ODODQQOD;

		public int[] OOQQQOQO;

		public int ODOQOOQO;

		public bool forceY;

		public float yChange;

		public float floorDepth;

		public float waterLevel;

		public bool lockWaterLevel;

		public float lastY;

		public string distance;

		public string markerDisplayStr;

		public string objectText;

		public bool applyAnimation;

		public float waveSize;

		public float waveHeight;

		public bool snapY;

		public TextAnchor origAnchor;

		public bool autoODODDQQO;

		public Texture2D roadTexture;

		public string[] OCQDQQDCOC;

		public string[] OQQDQQDOQQ;

		public int selectedWaterMaterial;

		public int selectedWaterScript;

		public bool doRestore;

		public bool doFlyOver;

		public Camera goCam;

		public float speed;

		public float offset;

		public bool camInit;

		public int roadPriority;

		public string[] roadPrioriyStrings;

		public Material roadMaterial;

		public Material roadSurfaceMaterial;

		public bool refreshDone;

		public bool disableMarkerIndents;

		public bool buildMesh;

		public bool startTCrossing;

		public bool endTCrossing;

		public ER3DControllerScript erController;

		public float splinePos;

		public Vector3 splinePosV3;

		public bool deadStart;

		public float deadStartRadius;

		public int deadStartSegments;

		public bool deadStartClosed;

		public float deadStartUVOffset;

		public bool deadStartRounded;

		public int deadStartRoundedDistance;

		public int deadStartRoundedSegments;

		public float deadStartOffset;

		public bool deadEnd;

		public float deadEndRadius;

		public int deadEndSegments;

		public bool deadEndClosed;

		public float deadEndUVOffset;

		public bool deadEndRounded;

		public int deadEndRoundedDistance;

		public int deadEndRoundedSegments;

		public float deadEndOffset;

		public int roadType;

		public float roundAboutRadius;

		public Vector3 roundaboutPos;

		public int roundAboutConnections;

		public List<float> mDistances;

		public List<OQCOCDCODC> OQOOQQCDQD;

		public List<Vector3> startDeadVecs;

		public List<Vector2> startDeadUVs;

		public List<int> startDeadTriangles;

		public List<Vector3> endDeadVecs;

		public List<Vector2> endDeadUVs;

		public List<int> endDeadTriangles;

		public List<Vector3> surfacesStartDeadVecs;

		public List<Vector3> surfacesEndDeadVecs;

		public List<int> surfacesStartDeadTriangles;

		public List<int> surfacesEndDeadTriangles;

		public List<Vector3> ptsArray;

		public List<Vector3> OOOQOOOODO;

		public List<Vector3> OCCDOQDDOD;

		public List<Vector2> ODCOCQOQCD;

		public List<Vector2> OOOOOODDDO;

		public List<int> OOCQQDCQDQ;

		public List<Vector3> tmpOOOQOOOODO;

		public List<Vector3> tmpOCCDOQDDOD;

		public List<float> riArray;

		public List<float> liArray;

		public List<float> rsArray;

		public List<float> lsArray;

		public List<Vector3> ODQODQODQD;
	}
}
