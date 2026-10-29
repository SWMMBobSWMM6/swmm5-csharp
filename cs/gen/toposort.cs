// Generated from EPA SWMM 5.2.4 (toposort.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int[] InDegree = null;
    public static int[] StartPos = null;
    public static int[] AdjList = null;
    public static int[] Stack = null;
    public static int First = 0;
    public static int Last = 0;
    public static CPtr Examined = CPtr.Null;
    public static CPtr InTree = CPtr.Null;
    public static int[] LoopLinks = null;
    public static int LoopLinksLast = 0;

    // toposort.c:57
    public static void toposort_sortLinks(int[] sortedLinks)
    {
        int i = 0;
        int n = 0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            sortedLinks[i] = i;
        }
        if (S.RouteModel == S.DW)
        {
            S.checkDummyLinks();
            if ((S.ErrorCode != 0))
            {
                return;
            }
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                S.Node[i].degree = 0;
            }
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                n = S.Link[i].node1;
                if (S.Link[i].direction < 0)
                {
                    n = S.Link[i].node2;
                }
                if (S.Node[n].type == S.OUTFALL)
                {
                    if (S.Link[i].direction < 0)
                    {
                        n = S.Link[i].node1;
                    }
                    else
                    {
                        n = S.Link[i].node2;
                    }
                    S.Node[n].degree++;
                }
                else
                {
                    S.Node[n].degree++;
                }
            }
            return;
        }
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.InDegree = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        S.StartPos = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        S.AdjList = new int[(int)(unchecked((ulong)(S.Nobjects[S.LINK])))];
        S.Stack = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        if ((((S.InDegree == null) || (S.StartPos == null)) || (S.AdjList == null)) || (S.Stack == null))
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
        }
        else
        {
            S.createAdjList(S.DIRECTED);
            S.adjustAdjList();
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                S.InDegree[i] = 0;
            }
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                S.InDegree[S.Link[i].node2]++;
            }
            n = S.topoSort(sortedLinks);
        }
        {
            if ((S.InDegree != null))
            {
                R.free(S.InDegree);
                S.InDegree = (null);
            }
        }
        ;
        {
            if ((S.StartPos != null))
            {
                R.free(S.StartPos);
                S.StartPos = (null);
            }
        }
        ;
        {
            if ((S.AdjList != null))
            {
                R.free(S.AdjList);
                S.AdjList = (null);
            }
        }
        ;
        {
            if ((S.Stack != null))
            {
                R.free(S.Stack);
                S.Stack = (null);
            }
        }
        ;
        if ((!(S.ErrorCode != 0)) && (n != S.Nobjects[S.LINK]))
        {
            S.report_writeErrorMsg(S.ERR_LOOP, S.L0);
            S.findCycles();
        }
    }

    // toposort.c:142
    public static void createAdjList(int listType)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Node[i].degree = 0;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.Node[S.Link[j].node1].degree++;
            if (listType == S.UNDIRECTED)
            {
                S.Node[S.Link[j].node2].degree++;
            }
        }
        S.StartPos[0] = 0;
        for (i = 0; i < S.Nobjects[S.NODE] - 1; i++)
        {
            S.StartPos[i + 1] = S.StartPos[i] + S.Node[i].degree;
            S.Node[i].degree = 0;
        }
        (S.Node[S.Nobjects[S.NODE] - 1]).degree = 0;
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            i = S.Link[j].node1;
            k = S.StartPos[i] + S.Node[i].degree;
            S.AdjList[k] = j;
            S.Node[i].degree++;
            if (listType == S.UNDIRECTED)
            {
                i = S.Link[j].node2;
                k = S.StartPos[i] + S.Node[i].degree;
                S.AdjList[k] = j;
                S.Node[i].degree++;
            }
        }
    }

    // toposort.c:193
    public static void adjustAdjList()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type != S.DIVIDER)
            {
                continue;
            }
            if (S.Node[i].degree != 2)
            {
                continue;
            }
            k = S.Node[i].subIndex;
            m = S.StartPos[i];
            j = S.AdjList[m];
            if (j == S.Divider[k].link)
            {
                S.AdjList[m] = S.AdjList[m + 1];
                S.AdjList[m + 1] = j;
            }
        }
    }

    // toposort.c:225
    public static int topoSort(int[] sortedLinks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int n = 0;
        int i1 = 0;
        int i2 = 0;
        int k1 = 0;
        int k2 = 0;
        S.First = 0;
        S.Last = -1;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.InDegree[i] == 0)
            {
                S.Last++;
                S.Stack[S.Last] = i;
            }
        }
        n = 0;
        while (S.First <= S.Last)
        {
            i1 = S.Stack[S.First];
            k1 = S.StartPos[i1];
            k2 = k1 + S.Node[i1].degree;
            for (k = k1; k < k2; k++)
            {
                j = S.AdjList[k];
                sortedLinks[n] = j;
                n++;
                i2 = S.Link[j].node2;
                S.InDegree[i2]--;
                if (S.InDegree[i2] == 0)
                {
                    S.Last++;
                    S.Stack[S.Last] = i2;
                }
            }
            S.First++;
        }
        return n;
    }

    // toposort.c:284
    public static void findCycles()
    {
        int i = 0;
        S.AdjList = new int[(int)(unchecked((ulong)(2)) * unchecked((ulong)(S.Nobjects[S.LINK])))];
        S.StartPos = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        S.Stack = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        S.Examined = R.Buf((int)(unchecked((ulong)(S.Nobjects[S.NODE]))));
        S.InTree = R.Buf((int)(unchecked((ulong)(S.Nobjects[S.LINK]))));
        S.LoopLinks = new int[(int)(unchecked((ulong)(S.Nobjects[S.LINK])))];
        if ((((((S.StartPos != null) && (S.AdjList != null)) && (S.Stack != null)) && (!(S.Examined).IsNull)) && (!(S.InTree).IsNull)) && (S.LoopLinks != null))
        {
            S.createAdjList(S.UNDIRECTED);
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                S.Examined[i] = 0;
            }
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                S.InTree[i] = 0;
            }
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                if ((S.Examined[i] != 0))
                {
                    continue;
                }
                S.Last = -1;
                S.findSpanningTree(i);
            }
        }
        {
            if ((S.StartPos != null))
            {
                R.free(S.StartPos);
                S.StartPos = (null);
            }
        }
        ;
        {
            if ((S.AdjList != null))
            {
                R.free(S.AdjList);
                S.AdjList = (null);
            }
        }
        ;
        {
            if ((S.Stack != null))
            {
                R.free(S.Stack);
                S.Stack = (null);
            }
        }
        ;
        {
            if (!(S.Examined).IsNull)
            {
                R.free(S.Examined);
                S.Examined = CPtr.Null;
            }
        }
        ;
        {
            if (!(S.InTree).IsNull)
            {
                R.free(S.InTree);
                S.InTree = CPtr.Null;
            }
        }
        ;
        {
            if ((S.LoopLinks != null))
            {
                R.free(S.LoopLinks);
                S.LoopLinks = (null);
            }
        }
        ;
    }

    // toposort.c:330
    public static void findSpanningTree(int startNode)
    {
        int nextNode = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        for (m = S.StartPos[startNode]; m < S.StartPos[startNode] + S.Node[startNode].degree; m++)
        {
            k = S.AdjList[m];
            if (S.Link[k].node1 == startNode)
            {
                j = S.Link[k].node2;
            }
            else
            {
                j = S.Link[k].node1;
            }
            if (S.InTree[k] == 0)
            {
                if ((S.Examined[j] != 0))
                {
                    S.InTree[k] = 2;
                    S.evalLoop(k);
                }
                else
                {
                    S.Examined[j] = 1;
                    S.Last++;
                    S.Stack[S.Last] = j;
                    S.InTree[k] = 1;
                }
            }
        }
        if (S.Last >= 0)
        {
            nextNode = S.Stack[S.Last];
            S.Last--;
            S.findSpanningTree(nextNode);
        }
    }

    // toposort.c:385
    public static void evalLoop(int startLink)
    {
        int i = 0;
        int i1 = 0;
        int i2 = 0;
        int j = 0;
        int kount = 0;
        int isCycle = 0;
        S.LoopLinksLast = 0;
        S.LoopLinks[0] = startLink;
        i1 = S.Link[startLink].node1;
        i2 = S.Link[startLink].node2;
        if (!((S.traceLoop(i1, i2, startLink) != 0)))
        {
            return;
        }
        isCycle = 1;
        j = S.LoopLinks[0];
        i2 = S.Link[j].node2;
        for (i = 1; i <= S.LoopLinksLast; i++)
        {
            j = S.LoopLinks[i];
            i1 = S.Link[j].node1;
            if (i1 == i2)
            {
                i2 = S.Link[j].node2;
            }
            else
            {
                isCycle = 0;
                break;
            }
        }
        if ((isCycle != 0))
        {
            kount = 0;
            for (i = 0; i <= S.LoopLinksLast; i++)
            {
                if (kount % 5 == 0)
                {
                    R.fprintf(S.Frpt.file, S.L276);
                }
                kount++;
                R.fprintf(S.Frpt.file, S.L651, S.Link[S.LoopLinks[i]].ID);
                if (i < S.LoopLinksLast)
                {
                    R.fprintf(S.Frpt.file, S.L652);
                }
            }
        }
    }

    // toposort.c:440
    public static int traceLoop(int i1, int i2, int k1)
    {
        int j = 0;
        int k = 0;
        int m = 0;
        if (i1 == i2)
        {
            return 1;
        }
        for (m = S.StartPos[i1]; m < S.StartPos[i1] + S.Node[i1].degree; m++)
        {
            k = S.AdjList[m];
            if ((k == k1) || (S.InTree[k] != 1))
            {
                continue;
            }
            if (S.Link[k].node1 == i1)
            {
                j = S.Link[k].node2;
            }
            else
            {
                j = S.Link[k].node1;
            }
            if ((S.traceLoop(j, i2, k) != 0))
            {
                S.LoopLinksLast++;
                S.LoopLinks[S.LoopLinksLast] = k;
                return 1;
            }
        }
        return 0;
    }

    // toposort.c:482
    public static void checkDummyLinks()
    {
        int i = 0;
        int j = 0;
        int[] marked = null;
        marked = new int[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        if (marked == null)
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            return;
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            j = S.Link[i].node2;
            if (S.Link[i].direction < 0)
            {
                j = S.Link[i].node1;
            }
            if ((((S.Link[i].type == S.CONDUIT) && (S.Link[i].xsect.type == S.DUMMY))) || (((S.Link[i].type == S.PUMP) && (S.Pump[S.Link[i].subIndex].type == S.IDEAL_PUMP))))
            {
                if (marked[j] == 0)
                {
                    marked[j] = 1;
                }
            }
            else
            {
                marked[j] = -1;
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if ((((S.Link[i].type == S.CONDUIT) && (S.Link[i].xsect.type == S.DUMMY))) || (((S.Link[i].type == S.PUMP) && (S.Pump[S.Link[i].subIndex].type == S.IDEAL_PUMP))))
            {
                j = S.Link[i].node1;
                if (marked[j] > 0)
                {
                    S.report_writeErrorMsg(S.ERR_DUMMY_LINK, S.Node[j].ID);
                }
            }
        }
        {
            if ((marked != null))
            {
                R.free(marked);
                marked = (null);
            }
        }
        ;
    }

}
}
