using System;
using System.Linq;

namespace HotelFinder
{
	public static class HotelComparer
	{
		private static readonly Category[] PreferedCategories =
		{
			Category.МиниГостиница,
			Category.ЭкономКласс,
			Category.ТриЗвезды,
			Category.БазаОтдыха,
			Category.Хостел,
			Category.ЭкономКлассМиниГостиница,
			Category.Пансионат,
			Category.ТриЗвездыМиниГостиница,
			Category.БазаОтдыхаКомплексОтдыха,
			Category.ЭкономКлассСанаторий,
			Category.МиниГостиницаЭкономКласс,
			Category.АпартаментОтель,
			Category.ЭкономКлассБазаОтдыха,
			Category.Апартаменты,
			Category.МиниГостиницаТриЗвезды,
			Category.АпартаментОтельЧастныйГостевойДом,
			Category.АпартаментОтель2,
			Category.ТриЗвездыЭкономКлассМиниГостиница,
		};

		public static Criterion CompareDistance(this Hotel current, Hotel other)
		{
			if (current.Distance < other.Distance)
				return Criterion.Better;
			if (current.Distance > other.Distance)
				return Criterion.Worse;

			return Criterion.Same;
		}

		public static Criterion CompareMaxPrice(this Hotel current, Hotel other)
		{
			//здесь проверяем на 0
			//если цена нулевая, то она просто отсутсвовала в данных
			if (Math.Abs(current.MaxPrice) < 1)
				return Criterion.Same;

			if (current.MaxPrice < other.MaxPrice)
				return Criterion.Better;
			if (current.MaxPrice > other.MaxPrice)
				return Criterion.Worse;

			return Criterion.Same;
		}

		public static Criterion CompareCategory(this Hotel current, Hotel other)
		{
			if (current.CategoryInt == other.CategoryInt)
				return Criterion.Same;

			if (current.CategoryInt == Category.Пусто)
				return Criterion.Same;

			var isPrefered = PreferedCategories.Contains(current.CategoryInt);
			var isOtherPrefered = PreferedCategories.Contains(other.CategoryInt);

			if (isPrefered && !isOtherPrefered)
				return Criterion.Better;

			if (!isPrefered && isOtherPrefered)
				return Criterion.Worse;

			return Criterion.Same;
		}
	}
}
