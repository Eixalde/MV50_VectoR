using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGestion : MonoBehaviour
{
    PointTool pointTool;
    VectorTool vectorTool;
    ProductTools productTools;
    PlanTool planTool;
    // Start is called before the first frame update
    void Start()
    {
        pointTool = GetComponent<PointTool>();
        vectorTool = GetComponent<VectorTool>();
        productTools = GetComponent<ProductTools>();
        planTool = GetComponent<PlanTool>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deselectAllTools()
    {
        pointTool.deselectPointTool();
        vectorTool.deselectVectorTool();
        productTools.deselectProductTools();
        planTool.deselectPlaneTool();
    }
}
