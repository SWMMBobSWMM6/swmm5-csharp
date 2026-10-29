// Generated from EPA SWMM 5.2.4 (hash.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // hash.c:24
    public static int samestr(CPtr s1, CPtr s2)
    {
        int i = 0;
        for (i = 0; (((((s1[i]) >= 'a') && ((s1[i]) <= 'z')) ? ((s1[i]) & ~32) : (s1[i]))) == (((((s2[i]) >= 'a') && ((s2[i]) <= 'z')) ? ((s2[i]) & ~32) : (s2[i]))); i++)
        {
            if ((!(s1[i + 1] != 0)) && (!(s2[i + 1] != 0)))
            {
                return (1);
            }
        }
        return (0);
    }

    // hash.c:33
    public static uint hash(CPtr str)
    {
        uint sum1 = unchecked((uint)(0));
        uint check1 = 0U;
        ulong sum2 = unchecked((ulong)(0L));
        while (0 != str[0])
        {
            sum1 += unchecked((uint)((((((str[0]) >= 'a') && ((str[0]) <= 'z')) ? ((str[0]) & ~32) : (str[0])))));
            str++;
            if (unchecked((uint)(255)) <= sum1)
            {
                sum1 -= unchecked((uint)(255));
            }
            sum2 += unchecked((ulong)(sum1));
        }
        check1 = unchecked((uint)(sum2));
        check1 %= unchecked((uint)(255));
        check1 = unchecked((uint)(255)) - (sum1 + check1) % unchecked((uint)(255));
        sum1 = unchecked((uint)(255)) - (sum1 + check1) % unchecked((uint)(255));
        return (((unchecked((uint)(check1 << 8))) | sum1) % unchecked((uint)(1999)));
    }

    // hash.c:51
    public static HTentry[] HTcreate()
    {
        int i = 0;
        HTentry[] ht = new HTentry[(int)(unchecked((ulong)(1999)))];
        if (ht != null)
        {
            for (i = 0; i < 1999; i++)
            {
                ht[i] = (null);
            }
        }
        return (ht);
    }

    // hash.c:59
    public static int HTinsert(HTentry[] ht, CPtr key, int data)
    {
        uint i = S.hash(key);
        HTentry entry = null;
        if (i >= unchecked((uint)(1999)))
        {
            return (0);
        }
        entry = new HTentry();
        if (entry == null)
        {
            return (0);
        }
        entry.key = key;
        entry.data = data;
        entry.next = ht[(int)(i)];
        ht[(int)(i)] = entry;
        return (1);
    }

    // hash.c:73
    public static int HTfind(HTentry[] ht, CPtr key)
    {
        uint i = S.hash(key);
        HTentry entry = null;
        if (i >= unchecked((uint)(1999)))
        {
            return (-1);
        }
        entry = ht[(int)(i)];
        while (entry != null)
        {
            if ((S.samestr(entry.key, key) != 0))
            {
                return (entry.data);
            }
            entry = entry.next;
        }
        return (-1);
    }

    // hash.c:87
    public static CPtr HTfindKey(HTentry[] ht, CPtr key)
    {
        uint i = S.hash(key);
        HTentry entry = null;
        if (i >= unchecked((uint)(1999)))
        {
            return CPtr.Null;
        }
        entry = ht[(int)(i)];
        while (entry != null)
        {
            if ((S.samestr(entry.key, key) != 0))
            {
                return (entry.key);
            }
            entry = entry.next;
        }
        return CPtr.Null;
    }

    // hash.c:101
    public static void HTfree(HTentry[] ht)
    {
        HTentry entry = null;
        HTentry nextentry = null;
        int i = 0;
        for (i = 0; i < 1999; i++)
        {
            entry = ht[i];
            while (entry != null)
            {
                nextentry = entry.next;
                R.free(entry);
                entry = nextentry;
            }
        }
        R.free(ht);
    }

}
}
