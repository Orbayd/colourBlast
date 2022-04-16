
using ColourBlast.Grid2D;
using UnityEngine;

namespace ColourBlast.Helpers
{
    public static class DebugHelpers
    {
        public static void GizmosCameraCorners()
        {
            var camera = Camera.main;

            Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topRight, 0.1f);
            Gizmos.DrawRay(camera.transform.position, topRight - camera.transform.position);

            Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(bottomRight, 0.1f);
            Gizmos.DrawRay(camera.transform.position, bottomRight - camera.transform.position);

            Vector3 topleft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(topleft, 0.1f);
            Gizmos.DrawRay(camera.transform.position, topleft - camera.transform.position);

            Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(bottomleft, 0.1f);
            Gizmos.DrawRay(camera.transform.position, bottomleft - camera.transform.position);
        }
        public static void GizmosGrid(AnimatedBlastGrid2D<BlastItem> grid)
        {
            if(grid is null)
            {
                return;
            }
            grid.TraverseAll(x=>
            {
                var pos = grid.GridLayout.GetGridPosition(x.Row,x.Column);
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(pos, new Vector3(grid.GridLayout.CellWidth, grid.GridLayout.CellHeight, 0));
                if(x.Row== 0 && x.Column == 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(pos,0.1f);
                }
            });
        }
    }


}