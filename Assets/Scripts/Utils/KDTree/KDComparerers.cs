using System;
using System.Collections.Generic;

public class KDTreeSortXComparerer<T> : IComparer<KDTreeEntry<T>>
{
    public int Compare(KDTreeEntry<T> entry1, KDTreeEntry<T> entry2)
    {
        int greater = Convert.ToInt32(entry1.position.x > entry2.position.x);
        int less = Convert.ToInt32(entry1.position.x < entry2.position.x);

        return greater - less;
    }
}

public class KDTreeSortYComparerer<T> : IComparer<KDTreeEntry<T>>
{
    public int Compare(KDTreeEntry<T> entry1, KDTreeEntry<T> entry2)
    {
        int greater = Convert.ToInt32(entry1.position.y > entry2.position.y);
        int less = Convert.ToInt32(entry1.position.y < entry2.position.y);

        return greater - less;
    }
}

public class KDTreeSortZComparerer<T> : IComparer<KDTreeEntry<T>>
{
    public int Compare(KDTreeEntry<T> entry1, KDTreeEntry<T> entry2)
    {
        int greater = Convert.ToInt32(entry1.position.z > entry2.position.z);
        int less = Convert.ToInt32(entry1.position.z < entry2.position.z);

        return greater - less;
    }
}
