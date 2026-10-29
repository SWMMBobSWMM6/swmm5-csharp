// Generated from EPA SWMM 5.2.4 (street.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // street.c:29
    public static int street_create(int nStreets)
    {
        S.Street = (null);
        S.Nobjects[S.STREET] = 0;
        S.Street = R.NewArray<TStreet>((int)(unchecked((ulong)(nStreets))));
        if (S.Street == null)
        {
            return S.ERR_MEMORY;
        }
        S.Nobjects[S.STREET] = nStreets;
        return 0;
    }

    // street.c:46
    public static void street_delete()
    {
        {
            if ((S.Street != null))
            {
                R.free(S.Street);
                S.Street = (null);
            }
        }
    }

    // street.c:58
    public static int street_readParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        int sides = 0;
        double[] x = new double[11];
        TStreet street = null;
        if (ntoks < 5)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        i = S.project_findObject(S.STREET, tok[0]);
        if (i < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        S.Street[i].ID = S.project_findID(S.STREET, tok[0]);
        for (k = 0; k <= 9; k++)
        {
            x[k] = 0.0;
        }
        for (k = 1; k <= 4; k++)
        {
            if ((!((S.getDouble(tok[k], ref x[k]) != 0))) || (x[k] <= 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[k]);
            }
        }
        if (ntoks > 5)
        {
            if ((!((S.getDouble(tok[5], ref x[5]) != 0))) || (x[5] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
        }
        if (ntoks > 6)
        {
            if ((!((S.getDouble(tok[6], ref x[6]) != 0))) || (x[6] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[6]);
            }
        }
        sides = 2;
        if (ntoks > 7)
        {
            if (((!((S.getInt(tok[7], ref sides) != 0))) || (sides < 1)) || (sides > 2))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
        }
        if (ntoks > 8)
        {
            if ((!((S.getDouble(tok[8], ref x[8]) != 0))) || (x[8] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[8]);
            }
            if (x[8] > 0.0)
            {
                if (ntoks < 11)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                for (k = 9; k <= 10; k++)
                {
                    if ((!((S.getDouble(tok[k], ref x[k]) != 0))) || (x[k] <= 0.0))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[k]);
                    }
                }
            }
        }
        street = S.Street[i];
        street.width = x[1] / S.UCF(S.LENGTH);
        street.curbHeight = x[2] / S.UCF(S.LENGTH);
        street.slope = x[3] / 100.0;
        street.roughness = x[4];
        street.gutterDepression = x[5] / S.UCF(S.LENGTH);
        street.gutterWidth = x[6] / S.UCF(S.LENGTH);
        street.sides = sides;
        street.backWidth = x[8] / S.UCF(S.LENGTH);
        street.backSlope = x[9] / 100.0;
        street.backRoughness = x[10];
        S.transect_createStreetTransect(street);
        return 0;
    }

    // street.c:141
    public static double street_getExtentFilled(int link)
    {
        int k = 0;
        int t = 0;
        double filled = 0.0;
        t = S.Link[link].xsect.transect;
        if (t < 0)
        {
            return 0.0;
        }
        if (S.RouteModel == S.DW)
        {
            filled = ((((S.Node[S.Link[link].node1].newDepth) >= (S.Node[S.Link[link].node2].newDepth)) ? (S.Node[S.Link[link].node1].newDepth) : (S.Node[S.Link[link].node2].newDepth)));
        }
        else
        {
            k = S.Link[link].subIndex;
            filled = ((((S.Conduit[k].a1) >= (S.Conduit[k].a2)) ? (S.Conduit[k].a1) : (S.Conduit[k].a2)));
        }
        return filled;
    }

}
}
