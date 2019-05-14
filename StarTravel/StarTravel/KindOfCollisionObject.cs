using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTravel
{
    /// <summary>
    /// Перечисление видов сталкиваемых объектов
    /// </summary>
    public enum KindOfCollisionObject
    {
        Weapon,  //Оружие
        DamageSpaceObject, //Может наносить урон
        NoDamageSpaceObject, //Не может наносить урон
        Ship, //Корабль
        HealingSpaceObject //Может увеличивать запас энергии
    }
}
