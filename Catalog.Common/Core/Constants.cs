namespace Catalog.Common
{
	public struct API
	{
		#region Global

		public const string URL = "https://ivanshop.ru/api/opt/v1/";

		public const string PATH = "/api/opt/v1/";

		public const string SAVE_ORDER = "sendOrder.php";

		public const string TYPE_KEY = "type";

		public const string VALUE_KEY = "v";

		#region Categories

		public const string CATEGORIES_TYPE_KEY = TYPE_KEY;

		public const string CATEGORIES_PARAM = "allcategories";

		#endregion

		#region Subcategories

		public const string SUBCATEGORIES_TYPE_KEY = TYPE_KEY;

		public const string SUBCATEGORIES_VALUE_KEY = VALUE_KEY;

		public const string SUBCATEGORIES_PARAM = "subcategorie";

		#endregion

		#region Products

		public const string PRODUCTS_TYPE_KEY = TYPE_KEY;

		public const string PRODUCTS_VALUE_KEY = VALUE_KEY;

		public const string PRODUCTS_PARAM = "categorie";

		#endregion

		#region Product

		public const string PRODUCT_TYPE_KEY = TYPE_KEY;

		public const string PRODUCT_VALUE_KEY = VALUE_KEY;

		public const string PRODUCT_PARAM = "product";

		#endregion

		#endregion

		public struct Product
		{
			public const string ID = "id";
			public const string NAME = "name";
			public const string ARTICLE_NUMBER = "artikul";
			public const string CODE = "kod";
			public const string BRAND = "brend";
			public const string PRICE = "price";
			public const string IMAGE = "image";
			public const string DESCRIPTION = "description";
			public const string STOCK1 = "sklad1";
			public const string STOCK2 = "sklad2";
			public const string PACKAGE = "upakovka";
			public const string SUB1 = "subcategory1";
			public const string SUB2 = "subcategory2";
		}

		public struct Category
		{
			public const string NAME = "category";
		}

		public struct Mail
		{
			public const string BOUNDARY = "NKdKd9Yk";

			public const string MEDIA_TYPE = "multipart/form-data";

			public const string MESSAGE_KEY = "text";

			public const string FILE_KEY = "myfile[]";
		}
	}

	public static class MESSAGE
	{
		public struct Web
		{
			public const string NO_CONNECTION = "Отсутствует подключение к сети! Повторите позже.";
		}

		public struct Mail
		{
			public const string SUBJECT = "Отправлено с ТД СБМ-Волга Каталог";
			public const string BODY = "Заказ #{0}\nID клиента: {1}\n";
			public const string ERROR = "Failed to post to {0}\nReason: {1}\nResponse: {2}";
		}

		public struct Gui
		{
			public const string COMPANY_INFO = "{0} Центр оптовой торговли ТД СБМ-Волга©, Россия, г. Нижний Новгород";
			public const string COMPANY_MAIL = "info@sbm-nn.ru";
			public const string UPDATE_STATUS = "<html>Последнее обновление: <b>{0}</b>";
			public const string ORDERS_STATUS_TEMPLATE = "<html>В корзине <b>{0}</b> позиций на сумму <b>{1}</b>";
			public const string HURRY_WARNING = "Загрузка данных еще не завершена! Пожалуйста, ждите завершения.";
			public const string CONFIRM_ACTION = "Хотите продолжить?";
			public const string EXPORT_ERROR = "Закройте файл и повтрите\n{0}\n{1}";
			public const string ON_SUCCESS_TRANSACTION = "Заказ №{0} принят.\nЖдите подтверждение оператора.";
			public const string ON_CONNECT_ERROR_TRANSACTION = "Отсутствует соединение с сетью, пожалуйста, подключитесь к сети и повторите попытку снова!";
		}

		public struct Category
		{
			public const string ADD_STATUS = "Добавление категории '{0}'";
			public const string LOADED_STATUS = "<html>Загружено категорий: <b>{0}</b>";
		}

		public struct Subcategory
		{
			public const string ADD_STATUS = "Добавление подкатегории '{0}'";
			public const string LOADED_STATUS = "<html>Загружено подкатегорий: <b>{0}</b>";
		}

		public struct Product
		{
			public const string ADD_STATUS = "Загрузка товара '{0}'";
			public const string UPD_STATUS = "Обновление товара '{0}'";
			public const string LOADED_STATUS = "<html>Загружено товаров: <b>{0}<b/>";
			public const string UPDATED_STATUS = "<html>Обновлено товаров: <b>{0}</b>";
			public const string MIXED_STATUS = "<html>Загружено/обновлено товаров: <b>{0}</b>";
			public const string MIXED_STATUS2 = "<html>Загружено/обновлено товаров: <b>{0}/{1}</b>";
		}

		public struct Photo
		{
			public const string ADD_STATUS = "Загружено изображение для {0}";
			public const string UPD_STATUS = "Обновлено изображение для {0}";
			public const string ERROR = "<html><span style=\"color:yellow;\">Файл {0} не найден в сервере или отсутствует соединение</span>";
		}
	}

	public struct CULTURE
	{
		public const string US = "en-US";
		public const string GB = "en-GB";
		public const string RU = "ru-RU";
	}

	public struct GUI
	{
		public const short COLLAPSIBLE_PANEL_MAX_WIDTH = 600;
		public const short COLLAPSIBLE_PANEL_MIN_WIDTH = 200;
		public const short DEFAULT_COLLAPSIBLE_PANEL_WIDTH = 300;
		public const short DEFAULT_COLLAPSIBLE_PANEL_MIN_WIDTH = 35;
		public const string SAVE_FILTER = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
	}
}