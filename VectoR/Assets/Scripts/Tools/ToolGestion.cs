using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class managing tool selection
 */
public class ToolGestion : MonoBehaviour
{
    // Point tool
    PointTool pointTool;
    // Vector tool
    VectorTool vectorTool;
    // Product tools
    ProductTools productTools;
    // Plane tool
    PlaneTool planTool;


    // Start is called before the first frame update
    void Start()
    {
        pointTool = GetComponent<PointTool>();
        vectorTool = GetComponent<VectorTool>();
        productTools = GetComponent<ProductTools>();
        planTool = GetComponent<PlaneTool>();

    }

    // Desselect all tools
    public void deselectAllTools()
    {
        pointTool.deselectPointTool();
        vectorTool.deselectVectorTool();
        productTools.deselectProductTools();
        planTool.deselectPlaneTool();
    }
}
