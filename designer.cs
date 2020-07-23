using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Atechnology.DBConnections2;
using Atechnology.winDraw;
using Atechnology.winDraw.Classes;
using Atechnology.winDraw.Model;
using Atechnology.winDraw.Model.Settings;
using Atechnology.winDraw.Spreading_v2;
using Atechnology.ecad.Document.Classes;
using Atechnology.ecad.Calc.winDraw;
using HandlePositionType = Atechnology.winDraw.HandlePositionType;
using HandleType = Atechnology.winDraw.HandleType;
using HingePosition = Atechnology.winDraw.HingePosition;
using HingeType = Atechnology.winDraw.HingeType;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using Atechnology.ecad.Dictionary;
using Atechnology.ecad.Document.DataSets;
using Atechnology.winDraw.Collections;
using Atechnology.winDraw.Company;
using Atechnology.Logging.Impl;
using Atechnology.winDraw.EllipseMath;
using Atechnology.winDraw.Ellipses;
// ReSharper disable ConvertToLambdaExpression
// ReSharper disable PossibleInvalidCastExceptionInForeachLoop
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantExplicitArrayCreation

namespace Atechnology.ecad.Calc
{
	public partial class RunCalc
	{
		private clsModel Model;
		private string shtapik = "";
		private string uplotnenie_color = "";
		private const int id_action_rehau_70 = 78;
		private const int id_rehau_euro_70 = 79;

		private const string Vorne = "Фурнитура VORNE"; // Наименование фурнитурной системы
		// in line
		private const string FurnSystBEZ = "Фурнитура БЕЗ";

		private const string FurnSystDver = "Фурнитура Дверная";

		private const string SiegeniaAxxent = "Фурнитура Siegenia Axxent";
		private const string SiegeniaClassic = "Фурнитура Siegenia Classic";
		private const string SiegeniaTitan = "Фурнитура Siegenia Titan Special";
		private const string SiegeniaTitanWK1 = "Фурнитура Siegenia Titan Special WK-1";
		private const string TitanDebug = "Фурнитура Titan // debug";
		private const string Axor = "Фурнитура AXOR KOMFORT LINE K3";

		// дефолтовая оконная фурнитурная система
		private const string DefaultWindowFurnitureSystem = SiegeniaClassic;

		private const string RotoNT = "Фурнитура ROTO NT";
		private const string RotoOK = "Фурнитура ROTO OK";
		private const string RotoNTDesigno = "Фурнитура ROTO NT Designo";
		private const string GEISSE = "Фурнитура GIESSE";
		private const string FurnitureSlayding60 = "Фурнитура Слайдинг-60";
		// add	
		private const string PSK_Vorne = "Фурнитура PSK Vorne";
		private const string PSK_160_COMFORT = "Фурнитура PSK 160 COMFORT";
		private const string PSKPORTAL100 = "Фурнитура PSK PORTAL 100";
		private const string PSKPORTAL160 = "Фурнитура PSK PORTAL 160";
		private const string PSKPORTAL200 = "Фурнитура PSK PORTAL 200";

		// in line
		private const string ProfSystBEZ = "Без профиля";
		private const string Pimapen = "Pimapen";
		private const string Rehau70 = "Rehau 70";
		private const string Rehau70mm = "Rehau 70мм";
		private const string Vario = "Vario";
		private const string ThermoLock = "Thermolock (Rehau)";
		private const string Classic = "Classic";
		private const string Thermo70 = "THERMO 70";
		private const string Solar = "Solar";
		private const string RehauEuro70 = "Rehau Euro-70";
		private const string Rehau60 = "Rehau 60мм";
		private const string SystSlayding60 = "Слайдинг-60";
		private const string KP45 = "КРАСАЛ КП45";
		private const string KPT74 = "КРАСАЛ КПТ74";
		private const string EVO = "EVO";
		private const string ALUTECH62 = "ALUTECH ALT W62";
		private const string ALUTECH_48 = "ALUTECH ALT C48";
		private const string ALT_F50 = "ALT F50";
		private const string ALT_F50Cover = "ALT F50 Крышка";
		private const string ALT100 = "ALT100";
		private const string DECOR = "DECOR";
		private const string ProfSystGlass = "Стеклянные конструкции";
		private const string EuroDesign = "EURO-DESIGN";

		// клоны RBN
		private const string SCHTANDART_START = "SCHTANDART START";
		private const string SCHTANDART_COMFORT = "SCHTANDART COMFORT";
		private const string SCHTANDART_PREMIUM = "SCHTANDART PREMIUM";

		// Deceunink
		private const string Eco60 = "ECO 60";
		private const string FORWARD = "ФОРВАРД";
		private const string BAUTEK = "БАУТЕК";
		private const string BAUTEKmass = "БАУТЕК в массе";
		private const string FavoritSpace = "ФАВОРИТ СПЭЙС";

		private const string EnwinMinima = "ENWIN MINIMA";
		private const string RehauOptima = "REHAU OPTIMA";
		private const string RehauDeluxe = "REHAU DELUXE";
		private const string RehauBlitzNew = "Rehau Blitz New";
		// 70
		private const string SibDesign = "SIB-DESIGN";
		private const string RehauMaxima = "REHAU MAXIMA";
		private const string RehauGrazio = "Rehau Grazio";
		private const string Thermo76 = "THERMO 76";
		// клон T76 но штапик с обратным радиусом
		private const string Estet = "Estet";
		// 80
		private const string NEO_80 = "NEO 80";
		// Gealan
		private const string Gealan = "Gealan S 9000";

		// типы конструкций
		private const string _window = "Окно";
		private const string _balcon = "Балконная дверь";
		private const string _indoor = "Дверь межкомнатная";
		private const string _outdoor = "Дверь входная";
		private const string _moskit = "Москитная сетка";
		private const string _glass = "Стеклопакет";
		private const string _swingdoor = "Дверь маятниковая";
		private const string _wcdoor = "Служебная дверь";
		private const string _pskportal = "ПСК портал";

		private const string _nalichnik = "Наличник";
		private const string _facade = "Фасад";
		private const string _manual = "Конструкция по ручному расчёту";
		private const string _gate = "Ворота (ручной расчет)";
		private const string _wicket = "Калитка (ручной расчет)";
		private const string _balconGlazing = "Балконное остекление";

		// todo global 
		const int idddocoperKrupn = 3;
		const int idddocoperPred = 33;
		const int idddocoperOknaDa = 67;
		const int iddocoperLerua = 34;
		//59	Рекламация
		const int idddocoperRekl = 59;

		// флаг отмены ограничений // 17-12-2013 Малеванный
		protected bool restriction;

		// dewrapper
		private static bool isRbnGrazioGrey(int idseller)
		{
			object[] objects = CalcProcessor.Modules["isRbnGrazioGrey"](new object[] {idseller});
			return objects != null && objects.Length > 0 && objects[0] is bool && (bool) objects[0];
		}

		private static bool isPvh(clsModel model)
		{
			return isPvh(model.ProfileSystem.ID);
		}

		/// классификация по типу материала конструкции: неизветно; ПВХ, Алюминий, Дерево, etc. :: /// deWrapper
		private static bool isPvh(int idprofsys)
		{
			return (bool) CalcProcessor.Modules["isPvh"].Invoke(new object[] {idprofsys})[0];
		}


		// ReSharper disable once UnusedMember.Global
		public void RunSpreading(dbconn db, Atechnology.winDraw.Spreading_v2.Spreading spreading, ConnectorDefinition conn)
		{
			Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = false;
			if (spreading.Type == SpreadingType.Shpross) // только для шпроссов
			{
				if (conn != null) // проверяем не пустой ли соединитель
				{
					if (conn.MasterConture != null) // только для концевых запоров
					{
						foreach (ConnectorLeg Leg in conn.Legs)
						{
							Leg.Length = 0.01; // устанавливаем их длину = 1, 0 не надо, там траблы вылетают.
						}
					}
				}
			}
		}

		private static bool getRestriction(clsModel model)
		{
			try
			{
				OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
				DataRow[] drdocsign = order.ds.docsign.Select("sign_name = 'Заказ' and signvalue_str = 'Отмена ограничений' and deleted is NULL");
				if (drdocsign.Length > 0)
					foreach (int i in Settings.idpeoplegroup)
						if (i == 10 || i == 36 || i == 44 || i == 45 || i == 46)
							return false;
			}
			catch (Exception)
			{
			}

			return true;
		}

		private static int? getiddocoper(clsModel model)
		{
			OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
			if (order != null && !order.DocRow.IsiddocoperNull())
				return order.DocRow.iddocoper;
			else
				return null;
		}

		private static int getIdseller(clsModel model)
		{
			OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
			return order != null && order.DocRow != null && !order.DocRow.IsidsellerNull() ? order.DocRow.idseller : 0;
		}

		private const string Al = "Алюминий";
		private const string White = "Белый";

		private static bool foo_visiblity_osnova_alutech(clsModel model)
		{
			return (model.ProfileSystem.Name == ALUTECH62 || model.ProfileSystem.Name == ALUTECH_48)
				&& (model.ColorInside.GroupName != Al && model.ColorOutside.GroupName != Al)
				&& (model.ColorInside.GroupName != White && model.ColorOutside.GroupName != White)
				;
		}

		private static bool isRetail(clsModel model)
		{
			int iddocoper = getiddocoper(model) ?? 0;
			return (bool) CalcProcessor.Modules["isRetail"].Invoke(new object[] {iddocoper})[0];
		}

		public static bool isArc(clsModel model)
		{
			return isArc(model.Frame) || isArc(model.Imposts) || model.Leafs.Exists(delegate(clsLeaf leaf) { return isArc(leaf); });
		}

		public static bool isArc(colBeem colBeem)
		{
			foreach (clsBeem beem in colBeem)
			{
				if (beem.R1 > 0) return true;
			}

			return false;
		}

		// Стартовый метод
		public void Run(dbconn _db, string name, clsModel model)
		{
			try
			{
				Model = model;

				try
				{
					// не удаётся это поставить в загрузку настроек ибо эту настройку нельзя пускать включоной в шпроссинг = падает диалог :( выбора типа профиля
					// Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = name != "BeforeAddSpreading" && name != "ChangeDornmas" && name != "ChangeSteel";
					// логика следущая =  при выборе профиля - понять, при выборе стекла - опустить

					// пони бегают по кругу // пришлось вырубить при 550711 -> 554008(550000)
					Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = false;
					
					//                    Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = (name == "Draw" && model.SelectedBeems.Count >0);
					//				    Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = !(model.SelectedRegions.Count > 0);

					// выставлено в загрузке настрек
					Atechnology.winDraw.Model.Settings.Settings.ProfilePictureSize = 40;

					//
					Model.WinDraw.frmBuild.propertyGrid.LookAndFeel.UseDefaultLookAndFeel = true;
					Model.WinDraw.frmBuild.propertyGrid.LookAndFeel.UseWindowsXPTheme = false;
					Model.WinDraw.frmBuild.propertyGrid.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
					//Model.WinDraw.frmBuild.propertyGrid.UseDisabledStatePainter = false;

					// 
					// model.WinDraw.IsBigOpenType = false;

					Tools.ChangeSetting("BuilderOptions", "CallDrawEvent", true);
					Tools.ChangeSetting("BuilderSettings", "DontGroupMoskitas", true);
					// артикул / описание профиля == 1
					Tools.ChangeSetting("BuilderSettings", "ChoiceProfile", 1);

					// наследовать заполнение при добавлении/удалении створки
					Tools.ReadSetting("BuilderSettings", "ImplementGlassFill", true);
					model.WinDraw.isImplementGlassFill = true;

					Tools.ChangeSetting("Visibility", "Вид", true);
					Tools.ChangeSetting("Visibility", "Расстояние", true);
					Tools.ChangeSetting("Visibility", "Открывание", true);
					Tools.ChangeSetting("Visibility", "Тип и сторона", false);
					Tools.ChangeSetting("Visibility", "Ручки", true);
					Tools.ChangeSetting("Visibility", "Дорнмас", false);
					Tools.ChangeSetting("Visibility", "Положение ручки", false);
					Tools.ChangeSetting("Visibility", "Характеристики", true);
					Tools.ChangeSetting("Visibility", "Петли", true);
					Tools.ChangeSetting("Visibility", "Положение петель", true);
					Tools.ChangeSetting("Visibility", "Тип петель", true);
					Tools.ChangeSetting("Visibility", "МеханизмФлаг", false);
					Tools.ChangeSetting("Visibility", "Замена фурнитуры", false);
					Tools.ChangeSetting("Visibility", "Разработка", false);
					Tools.ChangeSetting("Visibility", "_arm", false);

					// это видно только если есть арка
					Tools.ChangeSetting("Visibility", "Шаблон", isArc(Model));
					// только для особо одарённых, поэтому в общем интерфейсе не светим
					Tools.ChangeSetting("Visibility", "Ручное управление цветом", false);
					Tools.ChangeSetting("Visibility", "Ручное управление порогом", false);

					// только розница
					Tools.ChangeSetting("Visibility", "Монтажный зазор", isRetail(Model));
					Tools.ChangeSetting("Visibility", "Монтаж с защитой", isRetail(Model));

					Tools.ChangeSetting("Visibility", "Армирование", Model.ProfileSystem.Name != ALUTECH62 && Model.ProfileSystem.Name != ALUTECH_48);
					Tools.ChangeSetting("Visibility", "Основа ALUTECH", foo_visiblity_osnova_alutech(Model));

					Tools.ChangeSetting("Enabled", "Системная глубина", false); // RO
					Tools.ChangeSetting("Enabled", "МеханизмФлаг", false); // RO
					Tools.ChangeSetting("Enabled", "Замена фурнитуры", false); // RO
					Tools.ChangeSetting("Enabled", "Разработка", false); // RO
					

					// GIESSE
					// todo as up.visible
					//Tools.ChangeSetting("Visibility", "Накладки на петли", Model.FurnitureSystem.Name != FurnSystGEISSE);
					//Tools.ChangeSetting("Visibility", "Микрощелевое проветривание", Model.FurnitureSystem.Name != FurnSystGEISSE);
					//Tools.ChangeSetting("Visibility", "Блокировщик откидывания", Model.FurnitureSystem.Name != FurnSystGEISSE);
				}
				catch
				{
					// ignored
				}

				/// 91->94 маразмы оно беремт id системы из system.serial, а не idsystem поубивал бы
				//				try
				//				{
				//					patch_9194((clsModel) model);
				//				}
				//				catch
				//				{
				//					// ignored
				//				}

				restriction = getRestriction((clsModel) model);

				UserProperties(); //Обработка пользовательских характеристик (Создано 08-12-2010)

				if (shtapik == "")
					shtapik = Model.UserParameters["Штапик"].StringValue;
				//				if(uplotnenie_color=="")
				uplotnenie_color = Model.UserParameters["Цвет уплотнений"].StringValue;


				//// новые параметры дверей, контролька флага и распознавание параметров, да это может упсать на парсинге name - ну дык не я это придумал
				newDoorsFlagAndRecogniserSubroutine((clsModel) model, (wdAction) Enum.Parse(typeof(wdAction), name));

				//// рулеж отображения старых параметров / RO-режим построителя
				disableEnable((clsModel) model, (wdAction) Enum.Parse(typeof(wdAction), name));

				// опции ручек
				disableEnableHandleOptions((clsModel) model);

				// добавление подставочника // todo слить в одну переменную wdAction
				wdAction wdAction = (wdAction) Enum.Parse(typeof(wdAction), name);
				if (wdAction == wdAction.AddPerimetr || wdAction == wdAction.AddConstruction)
				{
					if (model.ModelConnections.Count > 0 || model.WinDraw.ModelConstruction.GetActiveModel() == model) // || если одна нулевая
					{
						// попробовать добавить подставочник для окон
						tryAddBottomConnector(model);
					}
				}

				// топология: это Rising никто не жучит!
				foreach (clsBeem beem in model.Frame)
				{
					clsFrame frameBeem = beem as clsFrame;
					if (frameBeem != null && frameBeem.Rising > 0)
						frameBeem.Rising = 0;
				}
//				if (Settings.idpeople == 255)
//					MessageBox.Show(name);
				switch (name)
				{
					case "Draw":
						Draw(model);
						break;
					case "ChangeProfileSystem":
						ChangeProfileSystem();
						UserProperties();
						ProfileConnection();
						ChangeUserParamValue();
						ChangeLeafUserParamList();
						ChangeFill();
						ChangeFurnitureSystem();
						ChangeMarkingSteel(); //06.12.2012
						break;
					case "ChangeFurniture":
						ChangeFurnitureSystem();
						UserProperties();
						ProfileConnection();
						ChangeLeafSize();
						break;
					case "ChangeProfile":
						ChangeProfile();
						ChangeRadius();
						ChangeMarkingSteel(); //06.12.2012
						ChangeSteel(Model, wdAction.ChangeProfile);
						break;
					case "AddLeaf":
						AddLeaf();
						ChangeFill();
						ChangeProfile();
						ChangeZashchelka();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						ChangeUserParamValue();
						break;
					case "RemoveLeaf":
						RemoveLeaf();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						break;
					case "ShowModel":
						ChangeLeafUserParamList();
						ChangeZashchelka2();
						AddDoorMessageIfDoor(Model);
						break;
					case "SelectModel": // select конструкции
						//ChangeLeafUserParamList ();
						break;
					case "ChangeConstructionType":
						ChangeConstructionType();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						AddDoorMessageIfDoor(Model);
						break;
					case "AddPerimetr":
						AddPerimetr();
						ChangeProfileSystem();
						ChangeFurnitureSystem();
						ChangeUserParamValue();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						ChangeRadius(); //06.09.2013 Илюшко Д.А.
						break;
					case "AddConstruction":
						AddConstruction();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						AddDoorMessageIfDoor(Model);
						break;
					case "AddImpost":
						AddImpost();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						break;
					case "RemoveImpost":
						RemoveImpost();
						break;
					case "ChangeHandleType": //Смена типа ручки
						ChangeLeafUserParamList();
						ChangeZashchelka();
						break;
					case "ChangeOpenType":
						ChangeOpenType();
						ChangeLeafSize();
						break;
					case "ChangeOpenSide": // СОЗДАНО 2009-04-17
						ChangeOpenSide();
						break;
					case "ChangeOpenView": // СОЗДАНО 31-07-2011 (Ковалёв Николай)
						ChangeOpenView();
						break;
					case "ChangeRadius":
						ChangeRadius();
						break;
					case "ChangeHingecount": //Добавленно Илюшко Д.А. 22.12.2011
						ChangeHinge();
						break;
					case "ChangeHingePosition": //Добавленно Илюшко Д.А  22.12.2011
						ChangeHinge();
						break;
					case "ChangeHingeType": //Добавленно Илюшко Д.А  22.12.2011
						ChangeHinge();
						break;
					case "ChangeFill": // ДОБАВЛЕНО 2009-04-17
						ChangeFill();
						break;
					case "BeforeParametersForm":
						break;
					case "ChangeUserParamValue":
						//Изменение пользовательских характеристик  СОЗДАНО 18-09-2011 (Ковалёв Николай)
						ChangeUserParamValue();
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						break;
					case "ChangeInsColor":
						ChangeColor();
						break;
					case "ChangeOutColor":
						ChangeColor();
						break;
					// добавлено 19.12.2011 - Илюшко Д.А.
					case "ChangeLeafSize":
						ChangeLeafSize();
						ChangeHandlePosition();
						break;
					case "ChangeConnectionType":
						ChangeConnectionType();
						break;
					case "ChangeGabarit": //06.12.2012 Илюшко Д.А.
						ChangeMarkingSteel(); //06.12.2012 Илюшко Д.А.
						ChangeHandlePosition();
						recalcHingeCount();
						break;
					case "AddConnector": //20.12.2012 Илюшко Д.А.
						ConnectorType();
						break;
					case "ChangeHandlePosition":
						ChangeHandlePosition();
						break;
					case "ChangeHandlePositionType":
						ChangeHandlePosition();
						break;

					case "DivisionBeem": // 20-06-2014 Малеванный
						DivisionBeemProhibit();
						break;

					case "ChangeSteel": // 19-08-2015
						ChangeSteel(Model, wdAction.ChangeSteel);
						break;
				}

				/// обртимися к первоисточнику, да это может выкинуть исключение, но это уже не наша забота НЕ-вызывать потроситель со всяким мусором
				/// список событий определён и чётко очерчен в Enum wdAction
				wdAction action = (wdAction) Enum.Parse(typeof(wdAction), name);

				/// когерентность параметра ручки и типа ручки створки, 
				/// ибо направление распространения извенений зависит от вида события то на надо тут его проанализировать - поэтому два метода и тут рулим в какйю сторону, 
				/// ну или вынести это в отдельнйый метод с параметром action
				switch (action)
				{
					/// не влияет на когерентность:

					// рисование 
					case wdAction.Draw:
						// после пересчета 
					case wdAction.AfterModelCalc:
						// перед формой характеристик
					case wdAction.BeforeParametersForm:
						// добавление перимтра
					case wdAction.AddPerimetr:
						// и конструкции
					case wdAction.AddConstruction:
						break;

					/// штатно обратный ход когерентности

					// после смены пользовательских параметров
					case wdAction.ChangeUserParamValue:
						// после отмены перемены характеристик
					case wdAction.SelectModel:
						// фоновый пересчет ht < up, т.к. пользователь может поменять тип ручки в заказе ~todo + перед выходом из построителя, поэтому критически важно чтоб в UP максимально быстро оказывалось правильное значение, ибо он является гранью отсечки
					case wdAction.BeforeModelCalc:
						htup((clsModel) model);
						break;

					/// прямой ход когерентности up < ht
					/// после изменений в построителе, почти все события, или если не лень то можно перечислить 
					default:
						upht((clsModel) model);
						break;
				}

				// инварианты, перед валидатором
				switch (action)
				{
					case wdAction.Draw:
					case wdAction.AfterModelCalc:
						break;
					default:
						// инварианты
						setInvariants((clsModel) model);
						break;
				}

				// крайнее действие в череде обработки события, ибо до этого места полно и новых и старых желающих отметиться в модели classNative
				switch (action)
				{
					/// todo наличие одновремено .AddConstruction и .AddPerimeter немного конфузит, 
					/// только добавленную модель (периметр) как ба и неча проверть, но с другой стороны 
					/// если есть инварианты надобно их прогнать и применить, 
					/// например добавили периметр двери а там порог не выставлено корректно => надо пррогонять
					/// добавли конструкцию

					// не проводим валидацию при отрисовке, 
					case wdAction.Draw:
					case wdAction.AddConstruction:
					case wdAction.BeforeParametersForm:
						break;

					// выход из построителя: финальная валидаци модели и запись собщений об ошибках в SystemScript 
					case wdAction.BeforeModelCalc:
						BeforeModelCalc(model);
						break;

					// после расчёта нативной модели в расчётную
					case wdAction.AfterModelCalc:
						AfterModelCalc(model);
						break;

					// валидация модели внутри построителя, и вывод чудесных бубликов внизу ПОСТРОИТЕЛЯ!, это не выполняется при фоновом расчете без построителя
					default:
						validationProcessor((clsModel) model);
						break;
				}
			}
			catch (Exception ex)
			{
				addErrorUniq(model, string.Format("Необработанное исключение в событии построителя {2}\n{0}\n{1}", ex.Message, ex.StackTrace, name));
			}
		}

		private void disableEnableHandleOptions(clsModel model)
		{
			foreach (clsLeaf leaf in model.Leafs)
			{
				clsUserParam upWindowHandleOptions = leaf.UserParameters.GetParam("Исполнение оконной ручки");
				if (upWindowHandleOptions != null)
				{
					// BUG upWindowHandleOptions.Visible = leaf.HandleType == HandleType.Оконная  && isPVH(model);
					// этот параметр рулит недра построителя согласно таблицы пользовательских параметров, тут мы лишт накладываем более жостко отноститьно типа ручки
					upWindowHandleOptions.Visible &= leaf.HandleType == HandleType.Оконная;

					if (leaf.HandleType != HandleType.Оконная || !upWindowHandleOptions.Visible)
					{
						upWindowHandleOptions.StringValue = upWindowHandleOptions.DefaultValue.StrValue;
					}
				}
			}
		}


		#region ОСНОВНЫЕ РЕАКЦИИ НА СОБЫТИЯ

		#region запрет деления арки

		private void DivisionBeemProhibit()
		{
			foreach (clsBeem beem in Model.Frame)
			{
				if (beem.R1 > 0 && beem.Selected && restriction)
				{
					MessageBox.Show("арки не делить!");
					Model.Undo.Undo();
					break;
				}
			}
		}

		#endregion

		#region добавление подставочного профиля		

		protected void tryAddBottomConnector(clsModel model)
		{
			if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balconGlazing)
			{
				if (!isBottomFree(model))
					return;

				if (haveModelBottomThis(model))
					return;

				// ищщем подставочный профиль
				foreach (clsProfile profile in model.ProfileSystem.colProfileConnectors)
				{
					if (profile.Visible && profile.ConnectortypeName != null && profile.ConnectortypeName == "Подставочный профиль")
					{
						// добавляем его в модель
						foreach (clsBeem beem in model.Frame)
							// выделяем тока нижних
							beem.Selected = beem.PositionBeem == ItemSide.Bottom;

						model.Connectors.Add(profile);
						return;
					}
				}
			}
		}

		bool haveModelBottomThis(clsModel model)
		{
			foreach (clsModelConnection connection in model.ModelConnections)
			{
				if (connection.direction.Y > 0)
					return true;
			}

			return false;
		}

		bool isBottomFree(clsModel model)
		{
			foreach (clsBeem beem in model.Connectors)
			{
				if (beem.PositionBeem == ItemSide.Bottom)
					return false;
			}

			return true;
		}

		#endregion добавление подставочного профиля 

		protected void AddDoorMessageIfDoor(clsModel model)
		{
			if (model.ConstructionType.Name == _outdoor)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "Дверь входная строиться `Вид Изнутри`";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}

			if (model.ConstructionType.Name == _moskit)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "Москитная сетка строиться по световому проёму";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}

			// TODO REFACTOR это бардак
			if (model.ProfileSystem.Name == Gealan)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "По умолчанию в Gealan используется 2 контура уплотнения!";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}
		}

		#region Добавление периметра

		public void AddPerimetr()
		{
			// TODO remove
			// Model.UserParameters["Разработка"].StringValue = "да";
			// TODO don't remove
			setNewMech(Model, true);

			// todo refactor in FillRestriction
			// по умолчанию для Vario
			if (Model.ProfileSystem.Name == Vario || Model.ProfileSystem.Name == DECOR || Model.ProfileSystem.Name == RehauDeluxe)
				Model.UserParameters["Накладки на петли"].StringValue = "Коричневый";

			if (Model.ProfileSystem.Name != SystSlayding60 && Model.ConstructionType.Name != _swingdoor)
			{
				// todo refactor to MoskitRestriction
				if (Model.ConstructionType.Name.ToLower().Contains("москитная сетка"))
				{
					Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystBEZ);
					Model.VisibleRegions[0].Selected = true;
					Model.Leafs.Add();

					clsLeaf lf = Model.Leafs[0];
					lf.OpenType = OpenType.Глухое;
					lf.IsMoskit = IsMoskit.Есть;
					// refactored Model.VisibleRegions[0].Fill = Model.ProfileSystem.Fills["Без_стекла_и_штапика"];
					foreach (clsUserParam up in lf.UserParameters)
						if (!up.Name.Contains("оскит"))
							up.Visible = false;
				}

				if (Model.ProfileSystem.Name == "Rehau DD" || Model.ProfileSystem.Name == "Rehau SD" || Model.ProfileSystem.Name == "Rehau BRD" || Model.ProfileSystem.Name == "Rehau 70мм")
				{
					Model.UserParameters["Штапик"].StringValue = "Закруглённый";
				}
				else if (Model.ProfileSystem.Name == Solar || Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
				{
					ChangeUserParamValue(); //Изменено 18-09-2011 (Ковалёв Николай)
				}
				else
				{
					Model.UserParameters["Штапик"].StringValue = "Скошенный";
				}
			}
			else
			{
				ProfileConnection();
			}

			ChangeFill();
			ChangeColor();
		}

		#endregion

		#region Добавление конструкции

		public void AddConstruction()
		{
			// для БД Водоотводящие по умолчанию наружу
			if (Model.ConstructionType.Name == _balcon)
			{
				clsUserParam up = Model.UserParameters.GetParam("Водоотводящие");
				if (up != null)
					up.StringValue = "Наружу";
			}

			if (Model.ProfileSystem.Name == SystSlayding60 || Model.ConstructionType.Name == _swingdoor)
			{
				ChangeProfile();
				ChangeFill();
				ChangeColor();
			}
		}

		#endregion

		#region Добавление импоста

		public void AddImpost()
		{
			ProfileConnection();

			if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				int i = 0;
				foreach (clsImpost imp in Model.Imposts)
				{
					i++;
					if (i % 2 == 0)
						imp.Profile = Model.ProfileSystem.colProfileImpost[1];
					else
						imp.Profile = Model.ProfileSystem.colProfileImpost[2];
				}

				ImpostAlignment();
			}
		}

		#endregion

		#region Выравнивание импостов

		public void ImpostAlignment()
		{
			foreach (clsImpost imp in Model.Imposts)
			{
				imp.Selected = true;
			}

			Model.Imposts.Alignment(AlignType.LightOpening);
		}

		#endregion

		#region Удаление импоста

		public void RemoveImpost()
		{
			if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				ChangeFill();
			}
		}

		#endregion

		#region Пользовательские параметры створок

		void ChangeLeafUserParamList()
		{
			if (Model.ProfileSystem.Name != SystSlayding60 && Model.ConstructionType.Name != _swingdoor)
			{
				foreach (clsLeaf lf in Model.Leafs)
				{
					ChangeLeafUserParam(lf);
				}
			}
			else if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				ChangeProfile();
			}
		}

		#endregion

		#region Пользовательские параметры створок - смена защёлок

		void ChangeZashchelka()
		{
			//Обработка смены типа ручек с замками и защелками
			foreach (clsLeaf lf in Model.Leafs)
			{
				//MessageBox.Show("Пользовательские параметры створок"+lf.UserParameters["Дверные петели"].NumericValue.ToString()+lf.HandleType.ToString());
				if (
					Model.ConstructionType.Name.Contains("Дверь входная") ||
					Model.ConstructionType.Name.Contains("Дверь межкомнатная") ||
					Model.ConstructionType.Name.Contains("Дверь маятниковая")
					)
				{
					if (lf.HandleType.ToString() == "Нет_ручки")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Оконная")
					{
						MessageBox.Show("Для ''Дверь входная'' - Тип ручки ''Оконная'' - Не используется!");
						lf.HandleType = HandleType.Нет_ручки;
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Нажимной_гарнитур")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Офисная")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Роликовая";
					}
				}
			}
		}

		#endregion

		#region Пользовательские параметры створок - смена защёлок ДЛЯ СТАРЫХ ЗАКАЗОВ

		void ChangeZashchelka2()
		{
			//Обработка смены типа ручек с замками и защелками
			foreach (clsLeaf lf in Model.Leafs)
			{
				//MessageBox.Show("Пользовательские параметры створок"+lf.UserParameters["Дверные петели"].NumericValue.ToString()+lf.HandleType.ToString());
				if (
					(
					Model.ConstructionType.Name.Contains("Дверь входная") ||
					Model.ConstructionType.Name.Contains("Дверь межкомнатная") ||
					Model.ConstructionType.Name.Contains("Дверь маятниковая")
					)
					&&
					(lf.UserParameters["Тип защёлки"].StringValue != "Фалевая") &&
					(lf.UserParameters["Тип защёлки"].StringValue != "Роликовая") /*&&
					(lf.UserParameters["Тип защёлки"].StringValue!="Нет_защёлки")*/)
				{
					if (lf.HandleType.ToString() == "Нет_ручки")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Оконная")
					{
						MessageBox.Show("Для ''Дверь входная'' - Тип ручки ''Оконная'' - Не используется!");
						lf.HandleType = HandleType.Нет_ручки;
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Нажимной_гарнитур")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Фалевая";
					}

					if (lf.HandleType.ToString() == "Офисная")
					{
						lf.UserParameters["Тип защёлки"].StringValue = "Роликовая";
					}
				}
			}
		}

		#endregion

		#region Изменение пользовательских характеристик створки

		void ChangeLeafUserParam(clsLeaf lf)
		{
			// у Москитной Сетки : lf.HandleBeem == null // 22-05-2015 малеванный
			if (Model.ConstructionType.Name == "Москитная сетка")
			{
				foreach (clsUserParam up in lf.UserParameters)
					if (up.Name != "Москитная сетка")
						up.Visible = false;
			}
			else
			{
				//Для дверных створок величина дорнмаса по умолчанию равна 40 мм. ДОБАВЛЕНО 2011-07-24 (Ковалёв Николай)
				if (lf.HandleBeem.Profile.Marking == "541130" || lf.HandleBeem.Profile.Marking == "541150" || lf.HandleBeem.Profile.Marking == "541340" || lf.HandleBeem.Profile.Marking == "541350" || lf.HandleBeem.Profile.Marking == "550170-701" || lf.HandleBeem.Profile.Marking == "550170-908")
				{
					lf.UserParameters["Дверной замок"].StringValue2 = "Дорнмас 40 мм";
				}

				if (lf.HandleBeem.Profile.Marking == "550510-701" || lf.HandleBeem.Profile.Marking == "550510-908" || lf.HandleBeem.Profile.Marking == "550400-701" || lf.HandleBeem.Profile.Marking == "550400-908")
				{
					lf.UserParameters["Дверной замок"].StringValue2 = "Дорнмас 28 мм";
				}
			}


			// +DM25 для ROTO NT Малеванный 17-03-2014
			if (lf.UserParameters["Тип ручки"] != null)
				lf.UserParameters["Тип ручки"].Visible =
					(Model.FurnitureSystem.Name == RotoNT || Model.FurnitureSystem.Name == RotoNTDesigno)
					&& Model.ConstructionType.Name == "Балконная дверь"
					&& lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf
					&& Convert.ToInt32(lf.HandleBeem.Profile.A) - 40 >= 54;

			if (lf.UserParameters["Оконная ручка"] != null)
				lf.UserParameters["Оконная ручка"].Visible =
					(Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && (Convert.ToInt32(lf.HandleBeem.Profile.A) < 94 || Model.FurnitureSystem.Name == SiegeniaTitan || Model.FurnitureSystem.Name == SiegeniaTitanWK1); // 27-03-2015 // 02.04.2014 Малеванный

			if (lf.UserParameters["Нажимная гарнитура"] != null)
				lf.UserParameters["Нажимная гарнитура"].Visible = (Model.ConstructionType.Name.Contains("Дверь") && lf.HandleType == HandleType.Нажимной_гарнитур && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			if (lf.UserParameters["Офисная ручка"] != null)
				lf.UserParameters["Офисная ручка"].Visible = (Model.ConstructionType.Name.Contains("Дверь") && lf.HandleType == HandleType.Офисная && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			lf.UserParameters["Балконная ручка"].Visible = (Model.ConstructionType.Name == "Балконная дверь" && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			lf.UserParameters["Замковый цилиндр"].Visible =
				Model.ConstructionType.Name.ToLower().Contains("дверь")
				&& lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf
				&& (lf.HandleBeem.Profile.A >= 94 || Model.FurnitureSystem.Name.ToLower().Contains("алюминий"));


			lf.UserParameters["Страховочные ножницы"].Visible = (Model.ConstructionType.Name == "Окно" && lf.OpenType == OpenType.Откидное);
			lf.UserParameters["Микрощелевое проветривание"].Visible = ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.OpenType == OpenType.Поворотно_откидное);
			lf.UserParameters["Блокировщик откидывания"].Visible = ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.OpenType == OpenType.Поворотно_откидное);
			lf.UserParameters["Запирание штульповой створки"].Visible = ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf);
			lf.UserParameters["Дверной замок"].Visible = (Model.ConstructionType.Name.Contains("Дверь") && ((Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 || Model.ProfileSystem.Name.ToLower().Contains("красал")) && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			//lf.UserParameters["Многозапорный замок"].Visible = ((Model.ConstructionType.Name == "Дверь межкомнатная" || Model.ConstructionType.Name == "Дверь входная") && (Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
			{
				if (Model.ProfileSystem.Name.ToLower().Contains("красал"))
					lf.UserParameters["Дверной замок"].StringValue2 = "Дорнмас 35 мм";


				if (lf.OpenType != OpenType.Поворотное)
					lf.OpenType = OpenType.Поворотное;

				lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;

				lf.HingeType = HingeType.DoorHinge;

				if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
					lf.HingePosition = HingePosition.HingeBorderEven;
				else
					lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;
			}
		}

		#endregion

		#region Смена типа открывания

		void ChangeOpenType()
		{
			//MessageBox.Show("Смена типа открывания");
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (!Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && !Model.ConstructionType.Name.Contains("маятник"))
				{
					lf.HingeCount = (int) lf.UserParameters["Дверные петели"].NumericValue;

					if (lf.SelectedBeems.Count == 0)
						continue;

					lf.UserParameters["Микрощелевое проветривание"].Visible = lf.OpenType == OpenType.Поворотно_откидное;

					lf.UserParameters["Страховочные ножницы"].Visible = lf.OpenType == OpenType.Откидное;

					lf.UserParameters["Блокировщик откидывания"].Visible = ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.OpenType == OpenType.Поворотно_откидное);

					if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
					{
						//////Нужно закоментарить следующие две строки
						if (Model.ConstructionType.Name.Contains("маятник"))
							lf.HandleType = HandleType.Офисная;

						if (lf.OpenType != OpenType.Поворотное)
							lf.OpenType = OpenType.Поворотное;

						lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;

						lf.HingeType = HingeType.DoorHinge;

						if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
							lf.HingePosition = HingePosition.HingeBorderEven;
						else
							lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;
					}
					else
					{
						if (Model.FurnitureSystem.Name.ToLower().Contains("дверная") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная") && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG зачем менять фурнитуру при вмене открывания как функцию типа конструкции - это не правильно

						lf.UserParameters["Дверные петели"].Visible = false;

						if (Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
						{
							if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
								lf.HingePosition = HingePosition.HingeBorderEven;
							else
								lf.HingePosition = HingePosition.HingeProportional;
						}
						else
						{
							lf.HingeCount = 2;
							lf.HingePosition = HingePosition.HingeBorderEven;
						}

						lf.HingeType = HingeType.WindowHinge;
					}
				}
				else if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
				{
					lf.OpenType = OpenType.Раздвижное;
				}
			}
		}

		#endregion

		#region Смена направления окрывания створки

		// http://yt:8000/issue/dev-253
		void ChangeOpenSide()
		{
			// ДОБАВЛЕНО 2009-04-17
			//MessageBox.Show("Смена направления окрывания створки");
			if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				foreach (clsLeaf lf in Model.Leafs)
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Frame)
					{
						lf.OpenSide = (lf.OpenSide == OpenSide.Влево) ? OpenSide.Вправо : OpenSide.Влево;
					}
					else if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Impost && lf.HandleBeem.ConnectBeemLine.Beem.bType == ComponentType.Impost)
					{
						if (lf.HandleBeem.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("без"))
							lf.OpenSide = (lf.OpenSide == OpenSide.Влево) ? OpenSide.Вправо : OpenSide.Влево;
					}

					foreach (clsLeafBeem lfb in lf)
					{
						if (lf.OpenSide == OpenSide.Вправо)
						{
							//Правило соединения балок створки
							switch (lfb.PositionBeem.ToString())
							{
								case "Bottom":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.Короткое;
									break;

								case "Left":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									lfb.ConnectType = ConnectType.Длинное;
									break;

								case "Top":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.Короткое;
									break;

								case "Right":
									if (lfb.ConnectBeemLine.Beem.bType.ToString() == "Impost" && !lfb.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("без"))
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									else
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5064.07"]; // ROS0679.07
									lfb.ConnectType = ConnectType.Длинное;
									break;
							}
						}
						else if (lf.OpenSide == OpenSide.Влево)
						{
							switch (lfb.PositionBeem.ToString())
							{
								case "Bottom":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.Короткое;
									break;

								case "Left":
									if (lfb.ConnectBeemLine.Beem.bType.ToString() == "Impost" && !lfb.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("без"))
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									else
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5064.07"]; // ROS0679.07
									lfb.ConnectType = ConnectType.Длинное;
									break;

								case "Top":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.Короткое;
									break;

								case "Right":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									lfb.ConnectType = ConnectType.Длинное;
									break;
							}
						}
					}
				}
			}
		}

		#endregion

		#region Смена вида окрывания створки Наружу/Внутрь

		// именно '% T%' а не '%T%' ибо встречается 'Створка Z60 BrillianTDesign' и 'Створка Z60 AnTik'
		private const string T = " T";
		private const string Z = " Z";

		private void ChangeOpenView()
		{
			if (Model.ConstructionType.Name.ToLower().Contains("дверь входная") || Model.ConstructionType.Name.ToLower().Contains("дверь межкомнатная"))
			{
				foreach (clsLeaf leaf in Model.Leafs.SelectedLeafs)
				{
					if (leaf.OpenView == OpenView.Внутрь && leaf.HingBeem.Profile.Comment.ToUpper().Contains(T))
					{
						// меняем профиль на Z
						invertProfile(leaf);
					}

					if (leaf.OpenView == OpenView.Наружу && leaf.HingBeem.Profile.Comment.ToUpper().Contains(Z))
					{
						// меняем профиль на T
						invertProfile(leaf);
					}
				}
			}
		}

		void invertProfile(clsLeaf leaf)
		{
			try
			{
				/// TODO !! тут сносит цокль у Алютеха, некритично но не приятно
				clsProfile profile2 = getConjugateProfile(leaf.Model, leaf.HingBeem.Profile);
				foreach (clsBeem beem in leaf)
				{
					beem.Profile = profile2;
				}

				MessageBox.Show(string.Format("Открыванию {0} соответствует профиль {1} {2}", leaf.OpenView, profile2.Marking, profile2.Comment), "Автоматичеcкая смена типа профиля Z | T", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (ArgumentException)
			{
				MessageBox.Show(string.Format("К профилю {0} {1} не найдено сопряжённого для открывания {2}", leaf.HingBeem.Profile.Marking, leaf.HingBeem.Profile.Comment, leaf.OpenView), "Автоматичеcкая смена типа профиля Z | T", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Model.Undo.Undo();
			}
		}

		static clsProfile getConjugateProfile(clsModel model, clsProfile profile) // throws  NoConjugateProfileEx()
		{
			// цветовое расширение -908 или string.Empty
			string ext = getProfileMarkingExt(profile);

			// тип
			string TZ = string.Empty;
			if (profile.Comment.ToUpper().Contains(T))
				TZ = Z;
			else if (profile.Comment.ToUpper().Contains(Z))
				TZ = T;
			else
				throw new ArgumentException();

			foreach (clsProfile p in profile.ProfileSystem.colProfileLeaf)
			{
				// если есть расширение порверить его у претендента
				if (ext != string.Empty && getProfileMarkingExt(p) != ext)
					continue;

				// сравнть .A
				if (profile.RefA != p.RefA)
					continue;

				// неужели у него сопряженная буква и он входит в состав доступных для этого типа конструкций => БИНГО !
				if (p.Comment.ToUpper().Contains(TZ) && model.ConstructionType._Leaf.Contains(p))
					return p;
			}

			// увы
			throw new ArgumentException();
		}

		static string getProfileMarkingExt(clsProfile profile)
		{
			string[] strings = profile.Marking.Split('-');
			if (strings.Length > 1)
				return strings[1];
			else
				return string.Empty;
		}

		#endregion

		#region Добавление створки

		void AddLeaf()
		{
			foreach (clsLeaf lf in Model.Leafs.SelectedLeafs)
			{
				// для Vario по умолчанию цвет ручек коричневый
				if (Model.ProfileSystem.Name == Vario || Model.ProfileSystem.Name == DECOR || Model.ProfileSystem.Name == RehauDeluxe)
				{
					lf.UserParameters["Москитная сетка"].StringValue2 = "Коричневый";
					lf.UserParameters["Оконная ручка"].StringValue = "Коричневая";
					lf.UserParameters["Балконная ручка"].StringValue = "Коричневая";
					lf.UserParameters["Офисная ручка"].StringValue = "Коричневая";
					lf.UserParameters["Нажимная гарнитура"].StringValue = "Коричневая";

					lf.UserParameters[__mechanizm].StringValue2 = __brown;
				}

				if (!Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && !Model.ConstructionType.Name.Contains("маятник"))
				{
					if ((Model.ConstructionType.Name == "Дверь входная" || Model.ConstructionType.Name == "Дверь межкомнатная") && lf.OpenType != OpenType.Поворотное)
						lf.OpenType = OpenType.Поворотное;

					if (Model.ConstructionType.Name == "Дверь входная")
					{
						if (lf.HandleType == HandleType.Оконная)
							lf.HandleType = HandleType.Нажимной_гарнитур;

						if (!Model.ColorInside.ToString().ToLower().Contains("белый") || !Model.ColorOutside.ToString().ToLower().Contains("белый"))
							lf.UserParameters["Дверные петели"].StringValue2 = "Коричневый";
						else
							lf.UserParameters["Дверные петели"].StringValue2 = "Белый";
					}

					if ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.HandleType != HandleType.Оконная)
						lf.HandleType = HandleType.Оконная;

					if (Model.ConstructionType.Name == "Окно")
					{
						lf.UserParameters["Оконная ручка"].Visible = true;
						lf.UserParameters["Тип ручки"].Visible = false;
						lf.UserParameters["Замковый цилиндр"].StringValue = "Нет";
						if (lf.OpenType == OpenType.Поворотно_откидное)
							lf.UserParameters["Микрощелевое проветривание"].Visible = true;
						else
							lf.UserParameters["Микрощелевое проветривание"].Visible = false;
						if (lf.OpenType == OpenType.Откидное)
							lf.UserParameters["Страховочные ножницы"].Visible = true;
						else
							lf.UserParameters["Страховочные ножницы"].Visible = false;
					}
					else if (Model.ConstructionType.Name == "Балконная дверь")
					{
						lf.UserParameters["Замковый цилиндр"].StringValue = "Нет";
						lf.UserParameters["Оконная ручка"].Visible = false;

						if (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
						{
							lf.UserParameters["Тип ручки"].Visible = false;
							lf.UserParameters["Балконная ручка"].Visible = false;
							lf.UserParameters["Балконная ручка"].StringValue = "Нет";
						}
						else
						{
							lf.UserParameters["Тип ручки"].Visible = true;
							lf.UserParameters["Балконная ручка"].Visible = true;
							//lf.UserParameters["Балконная ручка"].StringValue = "Белая";
						}
					}
					else if (Model.ConstructionType.Name == "Дверь межкомнатная")
					{
						lf.UserParameters["Замковый цилиндр"].StringValue = "Простой";
					}
					else if (Model.ConstructionType.Name == "Дверь входная")
					{
						lf.UserParameters["Замковый цилиндр"].StringValue = "Простой";
					}
					else if (Model.ConstructionType.Name == "Москитная сетка")
					{
						if (lf.OpenType != OpenType.Глухое)
							lf.OpenType = OpenType.Глухое;
						lf.IsMoskit = IsMoskit.Есть;
						// refactored Model.VisibleRegions[0].Fill = Model.ProfileSystem.Fills["Без_стекла_и_штапика"];
					}

					if (Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == _balconGlazing || Model.ConstructionType.Name == "Москитная сетка")
						lf.UserParameters["Москитная сетка"].StringValue = "Оконная";
					else
						lf.UserParameters["Москитная сетка"].StringValue = "Дверная";

					if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
					{
						if (!Model.ProfileSystem.Name.ToLower().Contains("красал"))
						{
							if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
								Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");
						}
						else
						{
							if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || !Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
								Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная (алюминий)");
							lf.UserParameters["Дверные петели"].StringValue = "GIESSE";
						}

						if (lf.OpenType != OpenType.Поворотное)
							lf.OpenType = OpenType.Поворотное;

						if (Model.FurnitureSystem.Name.ToLower().Contains("алюминий") || Model.ConstructionType.Name.ToLower().Contains("входная"))
							lf.HingeCount = 3;
						lf.HingeType = HingeType.DoorHinge;

						if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
							lf.HingePosition = HingePosition.HingeBorderEven;
						else
							lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

						lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;

						// разные умолчания для  разных систем Alutech
						switch (lf.Model.ProfileSystem.Name)
						{
							case ALUTECH62:
								lf.UserParameters[__mechanizm].StringValue = __keykey;
								break;
							case ALUTECH_48:
								lf.UserParameters[__mechanizm].StringValue = __keykey1;
								break;
						}
					}
						// служебная дверь
					else if (Model.ConstructionType.Name == _wcdoor)
					{
						// умолчания

						// фурнитура Дверная
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystDver);

						// Петли оконные RotoNT 3 шт
						lf.HingeCount = 3;
						lf.HingePosition = HingePosition.HingeProportional;
						lf.HingeType = HingeType.WindowHinge;

						// по умолчанию Нажимной однозапорный замок ключ/ключ
						lf.HandleType = HandleType.Нажимной_гарнитур;
						lf.HandlePositionType = HandlePositionType.Центральное;
						lf.UserParameters[__mechanizm].StringValue = __keykey1;

						// только поворотной
						lf.OpenType = OpenType.Поворотное;
					}
						// только межкомнатная наверно предполагается ?
					else
					{
						if (Model.FurnitureSystem.Name.ToLower().Contains("дверная") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная") && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG тут мы добавлем допустим окно наверное и этот код какбы менят на дефолтовую оконную систему, так ?
						else if (Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");

						lf.UserParameters["Дверные петели"].Visible = false;

						if (Model.ConstructionType.Name == _indoor)
						{
							lf.HingeCount = 3;
							lf.HingePosition = HingePosition.HingeProportional;
							lf.HingeType = HingeType.WindowHinge;

							// по умолчанию Нажимной однозапорный замок ключ/ключ
							lf.HandleType = HandleType.Нажимной_гарнитур;
							lf.HandlePositionType = HandlePositionType.Центральное;
							lf.UserParameters[__mechanizm].StringValue = __keykey1;
						}
						else
						{
							lf.HingeType = HingeType.WindowHinge;
						}
					}

					ChangeLeafUserParam(lf);
				}
				else if (Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
				{
					//lf.OpenType = OpenType.Раздвижное;

					ChangeOpenType();
					ChangeOpenSide();
					ChangeFill();
					ImpostAlignment();
					ChangeProfile();
					//AtMessageBox.Show(Model.SelectedRegions.ToString());
				}
				else
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Leaf && Model.ConstructionType.Name.Contains("маятник"))
					{
						MessageBox.Show("маятник2");
						lf.Selected = true;
						Model.Leafs.Remove();
					}

					RemoveLeaf();
				}
			}

			// Добавлено 19.12.2011 - Илюшко Д.А 
			ChangeLeafSize();
			ChangeFill();
		}

		#endregion

		#region Удаление створки

		void RemoveLeaf()
		{
			if (Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && !Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				//MessageBox.Show("ВНИМАНИЕ! Данное сочетание профиля и фурнитуры не позволяет устанавливать створки.");
				foreach (clsLeaf lf in Model.Leafs)
				{
					lf.Selected = true;
				}

				Model.Leafs.Remove();
			}
			else if (Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
			{
				ImpostAlignment();
			}
		}

		#endregion

		#region Смена типов конструкций

		private void ChangeConstructionType()
		{
			// если сменились в окно - добавить подставочник
			if (Model.ConstructionType.Name == _window || Model.ConstructionType.Name == _balconGlazing)
				tryAddBottomConnector(Model);

			//			AtMessageBox.Show("Смена типов конструкций");
			ChangeProfile();

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.HandleBeem.Profile.Marking == "550170-908" || lf.HandleBeem.Profile.Marking == "550170-701")
				{
					lf.OpenView = OpenView.Наружу;
					//					MessageBox.Show("Автоматическая смена вида открывания! Для створки T соответствует открывание 'Наружу (от себя)'");
				}

				//<--12.01.2012 В.Алексей
				if (Model.ConstructionType.Name == "Дверь входная" || Model.ConstructionType.Name == "Дверь межкомнатная"
					|| Model.ConstructionType.Name == "Балконная дверь")
					lf.UserParameters["Замковый цилиндр"].StringValue = "Простой";
				//-->
				if ((Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == "Балконная дверь") && lf.HandleType != HandleType.Оконная && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
				{
					lf.HandleType = HandleType.Оконная;
				}

				if ((Model.ConstructionType.Name == "Дверь входная" || Model.ConstructionType.Name == "Дверь межкомнатная") && lf.HandleType == HandleType.Оконная)
					lf.HandleType = HandleType.Нажимной_гарнитур;

				if ((Model.ConstructionType.Name == "Дверь входная" || Model.ConstructionType.Name == "Дверь межкомнатная") && lf.OpenType != OpenType.Поворотное)
				{
					lf.OpenType = OpenType.Поворотное;
				}

				if (Model.ConstructionType.Name == "Окно" || Model.ConstructionType.Name == _balconGlazing || Model.ConstructionType.Name == "Москитная сетка")
				{
					lf.UserParameters["Москитная сетка"].StringValue = "Оконная";
				}
				else
				{
					lf.UserParameters["Москитная сетка"].StringValue = "Дверная";
				}

				if (Model.ConstructionType.Name == "Балконная дверь" && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
					lf.UserParameters["Тип ручки"].Visible = true;
				else
					lf.UserParameters["Тип ручки"].Visible = false;

				if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("красал"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || !Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная (алюминий)");
						lf.UserParameters["Дверные петели"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.Поворотное)
						lf.OpenType = OpenType.Поворотное;

					lf.UserParameters["Дверные петели"].Visible = true;
					lf.HingeCount = 3;
					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;
				}
				else
				{
					if (Model.FurnitureSystem.Name == FurnSystDver && Model.ConstructionType.Name != _indoor && Model.FurnitureSystem.Name != FurnSystBEZ)
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan);
					}
					else if (Model.ConstructionType.Name == _indoor)
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystDver);
					}

					lf.UserParameters["Дверные петели"].Visible = false;

					if (Model.ConstructionType.Name == _indoor)
					{
						lf.HingeCount = 3;
						lf.HingePosition = HingePosition.HingeProportional;
					}
					else
					{
						lf.HingeCount = 2;
						lf.HingePosition = HingePosition.HingeBorderEven;
					}

					lf.HingeType = HingeType.WindowHinge;
				}

				ChangeLeafUserParam(lf);
			}
		}

		#endregion

		#region Смена профиля

		void ChangeProfile()
		{
			/// Default ==> ThermoLock + Antik => model.shtapic = круглый  
			/// кроме тогго надо поменять на соседних чиста глухарях
			if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
			{
				foreach (clsLeaf leaf in Model.Leafs.SelectedLeafs)
				{
					if (leaf.BaseRegion.Fill.Thikness == 24)
					{
						if (leaf[0].Profile.Marking == RehauZ60AntikInvariant.__AntikMarking)
						{
							Model.UserParameters["Штапик"].StringValue = "Закруглённый";
							foreach (clsModel otherModel in Model.GetMasterConstruction().GetModelArray())
							{
								if (otherModel != Model && otherModel.Leafs.Count == 0)
								{
									otherModel.UserParameters["Штапик"].StringValue = "Закруглённый";
								}
							}
						}
						else if (!RehauZ60AntikInvariant.isAntik(Model))
						{
							Model.UserParameters["Штапик"].StringValue = "Фигурный";
							foreach (clsModel otherModel in Model.GetMasterConstruction().GetModelArray())
							{
								if (otherModel != Model && otherModel.Leafs.Count == 0)
								{
									otherModel.UserParameters["Штапик"].StringValue = "Фигурный";
								}
							}
						}
					}
				}
			}

			/// вид откывания ( in / out ) в зависимости от профиля
			/// смотрим по комментарию в системах
			foreach (clsLeaf lf in Model.Leafs.SelectedLeafs)
			{
				bool f = false;

				if (lf.HandleBeem.Profile.Comment.ToUpper().Contains(Z) && lf.OpenView != OpenView.Внутрь)
				{
					lf.OpenView = OpenView.Внутрь;
					f = true;
				}

				if (lf.HandleBeem.Profile.Comment.ToUpper().Contains(T) && lf.OpenView != OpenView.Наружу)
				{
					lf.OpenView = OpenView.Наружу;
					f = true;
				}

				if (f)
					MessageBox.Show(string.Format("Для створки {0} {1} соответствует открывание {2}", lf.HandleBeem.Profile.Marking, lf.HandleBeem.Profile.Comment, lf.OpenView), "Автоматическая смена вида открывания", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}


			if (Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && !Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60") && Model.Leafs.Count > 0)
			{
				MessageBox.Show("ВНИМАНИЕ! Данное сочетание профиля и фурнитуры не позволяет устанавливать створки.");
				RemoveLeaf();
			}

			/// разворот / выставление направлекния открытия створок при появлении штульпа,  все разбросано :((( поубивавбы
			if (!Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") && !Model.ConstructionType.Name.Contains("маятник") && Model.ConstructionType.Name != _pskportal)
			{
				foreach (clsImpost im in Model.Imposts)
				{
					if (im.bType == ComponentType.Shtulp)
					{
						if (im.ConnectedLeaf.Count < 2)
							continue;

						if (Model.ConstructionType.Name == _window || Model.ConstructionType.Name == _balcon)
						{
							if (im.ConnectedLeaf[0].ShtulpOpenType == ShtulpOpenType.NoShtulpOnLeaf)
								im.ConnectedLeaf[0].OpenType = OpenType.Поворотно_откидное;
							else
								im.ConnectedLeaf[1].OpenType = OpenType.Поворотно_откидное;
						}
						else
						{
							im.ConnectedLeaf[0].OpenType = OpenType.Поворотное;
							im.ConnectedLeaf[1].OpenType = OpenType.Поворотное;
						}
					}
				}
			}
			else
			{
				AddImpost();
				ChangeOpenType();
				ChangeOpenSide();
			}

			foreach (clsLeaf lf in Model.Leafs)
			{
				ChangeLeafUserParam(lf);
			}

			//Следующая строка добавлена 31.05.2011 - Шрамко А.В.
			ProfileConnection();
			// Добавлено 19.12.2011 - Илюшко Д.А 
			ChangeLeafSize();
			ChangeConnectionType();
		}

		#endregion

		#region Смена заполнения

		private void ChangeFill()
		{
			OrderClass order = Model.WinDraw.DocClass as OrderClass;
			bool reklamacia = order != null && order.DocRow != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 59;

			//Однозначное определение соответствия заполнения и профильной системы (Создано 02-02-2011) // это НЕ ДЕЛАЕТСЯ ПРИ СМЕНЕК СИСТЕМЫ В ЗАКАЗЕ !!! КУЛИБИН !
			if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 24 && r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("и") && !r.Fill.Marking.ToLower().Contains("top n") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("SG") && !r.Fill.Marking.Contains("4ULTRA") && !r.Fill.Marking.Contains("CGS") && !r.Fill.Marking.Contains("LifeClass"))
					{
						if (!reklamacia)
						{
							MessageBox.Show("Внимание! Можно использовать только с/п c энергоэффективным стеклом И4 или сэндвич");
							r.Fill = Model.ProfileSystem.Fills["4х16хИ4"];
							// заодно установим штапик и цвет уплотнений	
						}
					}
					else if (r.Fill.Thikness == 32 || r.Fill.Marking.ToLower().Contains("elm5047.07"))
						Model.UserParameters["Штапик"].StringValue = "Скошенный";
					else if (RehauZ60AntikInvariant.isAntik(Model))
					{
						if (Model.UserParameters["Штапик"].StringValue != "Закруглённый" && Model.UserParameters["Штапик"].StringValue != "Фигурный")
						{
							Model.UserParameters["Штапик"].StringValue = "Закруглённый";
						}
					}
					else
						Model.UserParameters["Штапик"].StringValue = "Фигурный";
				}
			}
			else if (Model.ProfileSystem.Name == RehauEuro70)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("и4") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("CGS")) /// && !r.Fill.Marking.ToLower().Contains("cgs")  
					{
						if (!reklamacia)
						{
							MessageBox.Show("Внимание! Можно использовать только с/п 32мм с энергоэффективным стеклом И4 или сэндвич");
							r.Fill = Model.ProfileSystem.Fills["4х24хИ4"];
						}
					}
				}
			}
			else if (Model.ProfileSystem.Name == Solar)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("и") && !r.Fill.Marking.ToLower().Contains("top n") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047.07") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.ToLower().Contains("sg s") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.Contains("EN") && !r.Fill.Marking.Contains("4MF") && !r.Fill.Marking.Contains("6MF") && !(r.Fill.IDGroup == 80 && r.Thickness == 32) && !r.Fill.Marking.Contains("CGS")) /// && !r.Fill.Marking.ToLower().Contains("cgs") 
					{
						if (!reklamacia)
							if (restriction)
							{
								MessageBox.Show("Можно использовать только с/п 32мм с энергоэффективным стеклом И4 или сэндвич", "Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
								r.Fill = Model.ProfileSystem.Fills["4х10х4х10хИ4"];
							}
					}
				}
			}
			else if (Model.ProfileSystem.Name == Vario)
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if (r.Fill.Thikness != 32 || (!r.Fill.Marking.ToLower().Contains("и") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.ToLower().Contains("cgs") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.ToLower().Contains("sg s") && !r.Fill.Marking.Contains("EN") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("CGS") && !r.Fill.Marking.Contains("ULTRA")))
					{
						if (!reklamacia)
							if (restriction)
							{
								MessageBox.Show("Можно использовать только с/п 32мм с энергоэффективным стеклом И4 или сэндвич", " Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
								r.Fill = Model.ProfileSystem.Fills["4х10х4х10хИ4"];
							}
					}
				}
			//			else if (Model.ProfileSystem.Name == ProfSystThermo70)
			//				foreach (clsRegion r in Model.VisibleRegions)
			//				{
			//					if (r.Fill.Thikness != 32 || (!r.Fill.Marking.ToLower().Contains("и") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.ToLower().Contains("4(sg s)") && !r.Fill.Marking.ToLower().Contains("4х10х4х10х4") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("4ULTRA")  && !r.Fill.Marking.Contains("CGS")))  /// && !r.Fill.Marking.ToLower().Contains("cgs") 
			//					{
			//						if (!reklamacia && restriction)
			//						{
			//							MessageBox.Show("Можно использовать только с/п 32мм с энергоэффективным стеклом И4 или сэндвич", " Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
			//							r.Fill = Model.ProfileSystem.Fills["4х10х4х10хИ4"];
			//						}
			//					}
			//				}

			int addthick = 0;
			//Штапик с серым уплотнением Добавлено/изменено 18-09-2011 (Ковалёв Николай)
			// if (Model.ProfileSystem.Name.ToLower().Contains("rehau") || Model.ProfileSystem.Name.ToLower().Contains("thermolock") /*||Model.ProfileSystem.Name.ToLower().Contains("solar")*/)
			if (Model.ProfileSystem.Name == Rehau60
				|| Model.ProfileSystem.Name == Rehau70
				|| Model.ProfileSystem.Name == Rehau70mm
				|| Model.ProfileSystem.Name == RehauEuro70
				|| Model.ProfileSystem.Name == ThermoLock
				|| Model.ProfileSystem.Name == Classic)
			{
				if (Model.UserParameters["Цвет уплотнений"].StringValue == "Серый")
				{
					if (Model.ProfileSystem.Name.ToLower().Contains("rehau dd")
						|| Model.ProfileSystem.Name.ToLower().Contains("rehau brd")
						|| Model.ProfileSystem.Name.ToLower().Contains("rehau sd")
						|| Model.ProfileSystem.Name == Rehau70
						|| Model.ProfileSystem.Name == Rehau70mm
						|| Model.ProfileSystem.Name == Solar)
						addthick = 8;

					if (Model.VisibleRegions[0].Fill.Thikness >= 14 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 17 + addthick)
					{
						Model.UserParameters["Штапик"].StringValue = "Закруглённый";
						shtapik = Model.UserParameters["Штапик"].StringValue;
					}
					else if (Model.VisibleRegions[0].Fill.Thikness >= 22 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 25 + addthick && Model.ProfileSystem.Name != Solar && Model.ProfileSystem.Name != Rehau70)
					{
						/// 30-03-2015 малеванный
						/// для Rehau Z60 Antik можно поставить круглый серый штапик 14.5 мм, поэтому тут меняем только не баним установку Круглого штапика
						/// для остальных все по прежнему
						if (Model.ProfileSystem.Name == Rehau60 || Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
						{
							if (Model.UserParameters["Штапик"].StringValue != "Фигурный" && Model.UserParameters["Штапик"].StringValue != "Закруглённый")
								Model.UserParameters["Штапик"].StringValue = "Фигурный";
						}
						else
						{
							Model.UserParameters["Штапик"].StringValue = "Фигурный";
						}

						shtapik = Model.UserParameters["Штапик"].StringValue;
					}
					else if (Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick && Model.ProfileSystem.Name.ToLower().Contains("rehau"))
					{
						Model.UserParameters["Штапик"].StringValue = "Скошенный";
						shtapik = Model.UserParameters["Штапик"].StringValue;
					}
					else if (Model.ProfileSystem.Name != ThermoLock && Model.ProfileSystem.Name != Classic && Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick)
					{
						MessageBox.Show("Для данной конструкции нет штапика с СЕРЫМ уплотнением! Параметры штапика и уплотнения будут установлены по умолчанию.");
						ChangeProfileSystem();
					}
				}
			}

			// Solar проверяем отдельно
			if (Model.ProfileSystem.Name == Solar)
			{
				if (Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick && Model.ProfileSystem.Name == Solar)
				{
					Model.UserParameters["Штапик"].StringValue = "Закруглённый";
					shtapik = Model.UserParameters["Штапик"].StringValue;
				}
			}
		}

		#endregion

		#region Смена профильной системы

		private void ChangeProfileSystem()
		{
			ChangeColor();

			//Однозначное определение соответствия профильной системы и фурнитуры (Создано 02-02-2011)
			if (Model.ProfileSystem.Name.ToLower().Contains("без") || Model.FurnitureSystem.Name.ToLower().Contains("без"))
				return;

			if (Model.ProfileSystem.Name == Pimapen && Model.FurnitureSystem.Name != RotoOK)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(RotoOK);
			}
			else if (isPvh(Model))
			{
				if ((Model.ConstructionType.Name == _window || Model.ConstructionType.Name == _balcon)
					&& Model.FurnitureSystem.Name != SiegeniaClassic
					&& Model.FurnitureSystem.Name != SiegeniaTitan
					&& Model.FurnitureSystem.Name != SiegeniaTitanWK1
					&& Model.FurnitureSystem.Name != SiegeniaAxxent
					&& Model.FurnitureSystem.Name != RotoNT
					&& Model.FurnitureSystem.Name != RotoOK
					&& Model.FurnitureSystem.Name != Vorne
					)
				{
					Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem);
				}
				else if (Model.ConstructionType.Name == _outdoor || Model.ConstructionType.Name == _indoor)
				{
					Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystDver);
				}
			}
			else if (Model.ProfileSystem.Name == SystSlayding60)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnitureSlayding60);
			}
			else if (Model.ProfileSystem.Name == ProfSystGlass)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура для стеклянных конструкций");
			}

			// штапики опять....  ИБАНЫЙ ГОВНОКОД !!!
			if (Model.ProfileSystem.Name == "Rehau DD" || Model.ProfileSystem.Name == "Rehau SD" || Model.ProfileSystem.Name == "Rehau BRD" || Model.ProfileSystem.Name == "Rehau 70мм")
			{
				Model.UserParameters["Штапик"].StringValue = "Закруглённый";
			}
			else if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic || Model.ProfileSystem.Name == Solar || Model.ProfileSystem.Name == Vario)
			{
				ChangeUserParamValue(); //Изменено 18-09-2011 (Ковалёв Николай)
			}
			else if (Model.ProfileSystem.Name == SCHTANDART_START || Model.ProfileSystem.Name == SCHTANDART_COMFORT || Model.ProfileSystem.Name == SCHTANDART_PREMIUM)
			{
			}
			else
			{
				Model.UserParameters["Штапик"].StringValue = "Скошенный";
				Model.UserParameters["Цвет уплотнений"].StringValue = "Черный";
			}

			foreach (clsLeaf lf in Model.Leafs)
			{
				lf.UserParameters["Запирание штульповой створки"].Visible = (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf);
				if ((Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 && Model.ConstructionType.Name.ToLower().Contains("дверь") && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
				{
					lf.UserParameters["Замковый цилиндр"].Visible = true;
				}
				else
				{
					lf.UserParameters["Замковый цилиндр"].Visible = false;
				}

				if (Model.ConstructionType.Name == "Окно")
				{
					lf.UserParameters["Замковый цилиндр"].StringValue = "Нет";
				}
				else if (Model.ConstructionType.Name == "Балконная дверь")
				{
					//lf.UserParameters["Замковый цилиндр"].StringValue = "Нет";
					if (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
					{
						lf.UserParameters["Балконная ручка"].Visible = false;
						lf.UserParameters["Балконная ручка"].StringValue = "Нет";
					}
					else
					{
						lf.UserParameters["Балконная ручка"].Visible = true;
						//lf.UserParameters["Балконная ручка"].StringValue = "Белая";
					}
				}
				else if (Model.ConstructionType.Name == "Дверь межкомнатная")
				{
					lf.UserParameters["Замковый цилиндр"].StringValue = "Простой";
				}
				else if (Model.ConstructionType.Name == "Дверь входная")
				{
					lf.UserParameters["Замковый цилиндр"].StringValue = "Простой";
				}

				if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("красал"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");
						lf.UserParameters["Дверные петели"].StringValue = "Roto";
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || !Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная (алюминий)");
						lf.UserParameters["Дверные петели"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.Поворотное)
						lf.OpenType = OpenType.Поворотное;

					lf.UserParameters["Дверные петели"].Visible = true;
					lf.HingeCount = 3;
					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;
				}
				else
				{
					if (Model.FurnitureSystem.Name.ToLower().Contains("дверная") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная") && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG зачем менять фурнитуру при смене профиля как функцию типа конструкции - это не правильно

					lf.UserParameters["Дверные петели"].Visible = false;

					if (Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
					{
						if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
							lf.HingePosition = HingePosition.HingeBorderEven;
						else
							lf.HingePosition = HingePosition.HingeProportional;
					}
					else
					{
						lf.HingeCount = 2;
						lf.HingePosition = HingePosition.HingeBorderEven;
					}

					lf.HingeType = HingeType.WindowHinge;
				}
			}

			// Добавлено 19.12.2011 - Илюшко Д.А 
			ChangeLeafSize();
			// Добавлено 31.05.2013 - Илюшко Д.А 			
			ChangeFill();
		}

		#endregion

		#region Смена системы фурнитуры

		private void ChangeFurnitureSystem()
		{
			// на всякий случай выставим осной комплект фурнитуры
			Model.UserParameters["Комплектация фурнитуры"].StringValue = "Основная";

			//Однозначное определение соответствия профильной системы и фурнитуры (Создано 02-02-2011)
			if (Model.FurnitureSystem.Name.ToLower().Contains("без"))
				return;

			if (Model.ProfileSystem.Name == Rehau70)
			{
				if (Model.FurnitureSystem.Name != FurnSystDver)
				{
					if (Model.ConstructionType.Name.ToLower().Contains("дверь входная") || Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");
					}
				}
			}

			if (Model.ProfileSystem.Name == Vario)
			{
				if (Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != SiegeniaTitanWK1 && Model.FurnitureSystem.Name != FurnSystDver && Model.FurnitureSystem.Name != RotoNTDesigno && Model.FurnitureSystem.Name != SiegeniaAxxent)
				{
					if (Model.ConstructionType.Name == _outdoor || Model.ConstructionType.Name == _indoor)
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystDver);
					}
					else if (Model.ConstructionType.Name == _pskportal)
					{
					}
					else
					{
						if (Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != SiegeniaTitanWK1 && Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != RotoNTDesigno && Model.FurnitureSystem.Name != SiegeniaAxxent)
						{
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan);
						}
					}
				}
			}

			if (Model.ProfileSystem.Name == Solar)
			{
				if (Model.FurnitureSystem.Name != SiegeniaTitanWK1 && Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != RotoNTDesigno && Model.FurnitureSystem.Name != SiegeniaAxxent)
				{
					Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan);
				}
			}

			if (Model.ProfileSystem.Name == Thermo70)
			{
				if (Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != SiegeniaClassic && Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != RotoNTDesigno && Model.FurnitureSystem.Name != SiegeniaAxxent)
				{
					if (restriction)
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem);
					}
				}
			}

			if (Model.ProfileSystem.Name == RehauEuro70 && Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != RotoOK && Model.FurnitureSystem.Name != SiegeniaClassic && Model.FurnitureSystem.Name != SiegeniaTitan)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem);
			}
			else if (Model.FurnitureSystem.Name.ToLower().Contains("фурнитура для стеклянных конструкций"))
			{
				Model.ProfileSystem = Model.WinDraw.ProfileSystems.FromName("Стеклянные конструкции");
			}
			else if (Model.ProfileSystem.Name == Pimapen && Model.FurnitureSystem.Name != RotoOK /*&& Model.FurnitureSystem.Name != FurnSystRotoNT*/) // todo remove NT
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(RotoOK);
			}
			else if ((Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic) && Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != RotoOK && Model.FurnitureSystem.Name != Vorne && Model.FurnitureSystem.Name != SiegeniaClassic && Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != SiegeniaAxxent)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem);
			}
			else if (Model.ProfileSystem.Name == Solar && Model.FurnitureSystem.Name != SiegeniaTitan && Model.FurnitureSystem.Name != SiegeniaTitanWK1 && Model.FurnitureSystem.Name != RotoNT && Model.FurnitureSystem.Name != RotoNTDesigno && Model.FurnitureSystem.Name != SiegeniaAxxent)
			{
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan);
			}
            /*
            else if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial || Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial)
                && Model.ProfileSystem.Name != ProfSystSolar 
                && Model.ProfileSystem.Name != ProfSystRehau70 
                && Model.ProfileSystem.Name != ProfSystRehau70mm 
                && Model.ProfileSystem.Name != ProfSystVario
                && Model.ProfileSystem.Name != ProfSystThermo70
                && Model.ProfileSystem.Name != ProfSystEuro70
                && Model.ProfileSystem.Name != ProfSystEVO
                
                && Model.ProfileSystem.Name != ProfSystSibDesign
                && Model.ProfileSystem.Name != ProfSystRehauMaxima
                //&& Model.ProfileSystem.Name != ProfSystRehau60
                )
            {
                if(restriction)
                {
                    MessageBox.Show("Фурнитуру Siegenia Titan можно использовать только в профильной системе 70мм", "Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystRotoNT); 
                }
            }
            */

            //			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial || Model.FurnitureSystem.Name == FurnSystSigeniaTitanWK1) && Model.UserParameters["Комплектация фурнитуры"].StringValue != "Противовзломная")
            //			{
            //				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";
            //			}
            //
            //			if (Model.ProfileSystem.Name == ProfSystVario && (Model.FurnitureSystem.Name == FurnSystRotoNTDesigno || Model.FurnitureSystem.Name == FurnSystRotoNT))
            //			{
            //				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";
            //			}
		}

		#endregion

		#region ИЗМЕНЕНИЕ ПОЛЬЗОВАТЕЛЬСКИХ ХАРАКТЕРИСТИК

		//Создано 18-09-2011 (Ковалёв Николай)
		void ChangeUserParamValue()
		{
			//MessageBox.Show("Цвет уплотнения - " + uplotnenie_color);
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (Model.ConstructionType.Name.Contains("Балконная дверь") && lf.UserParameters["Запирание штульповой створки"].StringValue == "Штульповой механизм")
				{
					MessageBox.Show("На штульповой балконной двери с штульповым механизмом, балконная защелка будет отсутствовать!!!", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
				}

				// todo refactor как инвариант ибо работает только в одну стророну
				if (Model.ProfileSystem.Name == SystSlayding60)
				{
					if (lf.UserParameters["Москитная сетка"].StringValue != "Слайдинг")
					{
						lf.UserParameters["Москитная сетка"].StringValue = "Слайдинг";
						// 20180606 // MessageBox.Show("Установлен тип москитной сетки - Слайдинг!", "Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					}
				}
			}

			//Шрамко А.В. 29.10.12			
			if (Model.ProfileSystem.ID == id_action_rehau_70 || Model.ProfileSystem.ID == id_rehau_euro_70)
			{
				//Для этих систем штапик всегда скошенный, уплотнение всегда черное
				UserProprietesAction();
				ChangeFill();
			}

			if (Model.ProfileSystem.Name == Vario)
			{
				Model.UserParameters["Штапик"].StringValue = "Закруглённый";
				Model.UserParameters["Цвет уплотнений"].StringValue = "Черный";
			}

			if (Model.ProfileSystem.Name == Thermo70 || Model.ProfileSystem.Name == Thermo76 || Model.ProfileSystem.Name == NEO_80)
			{
				if (Model.UserParameters["Штапик"].StringValue != "Фигурный" || Model.UserParameters["Цвет уплотнений"].StringValue != "Серый")
				{
					Model.UserParameters["Штапик"].StringValue = "Фигурный";
					Model.UserParameters["Цвет уплотнений"].StringValue = "Серый";
				}
			}

			if (Model.ProfileSystem.Name == Estet)
			{
				if (Model.UserParameters["Штапик"].StringValue != "Обратный радиус" || Model.UserParameters["Цвет уплотнений"].StringValue != "Серый")
				{
					Model.UserParameters["Штапик"].StringValue = "Обратный радиус";
					Model.UserParameters["Цвет уплотнений"].StringValue = "Серый";
				}
			}

			if (Model.ProfileSystem.Name != Solar && Model.ProfileSystem.Name != ThermoLock && Model.ProfileSystem.Name != Classic)
			{
				if (shtapik != Model.UserParameters["Штапик"].StringValue)
				{
					shtapik = Model.UserParameters["Штапик"].StringValue;
					ChangeFill();
				}
				else if (uplotnenie_color != Model.UserParameters["Цвет уплотнений"].StringValue)
				{
					uplotnenie_color = Model.UserParameters["Цвет уплотнений"].StringValue;
					ChangeFill();
				}
			}
			else
			{
				if (shtapik != Model.UserParameters["Штапик"].StringValue)
				{
					Model.UserParameters["Цвет уплотнений"].StringValue = "Серый"; //Для Solar и Thermolock цвет уплотнений всегда СЕРЫЙ
					uplotnenie_color = Model.UserParameters["Цвет уплотнений"].StringValue;
					shtapik = Model.UserParameters["Штапик"].StringValue;
					ChangeFill();
				}

				if (uplotnenie_color != "Серый")
				{
					Model.UserParameters["Цвет уплотнений"].StringValue = "Серый"; //Для Solar и Thermolock цвет уплотнений всегда СЕРЫЙ
					uplotnenie_color = Model.UserParameters["Цвет уплотнений"].StringValue;
					ChangeFill();
				}
			}

			//			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanWK1) && Model.UserParameters["Комплектация фурнитуры"].StringValue != "Противовзломная")
			//			{
			//				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";
			//			}
			//
			//			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial) && Model.UserParameters["Комплектация фурнитуры"].StringValue == "Противовзломная")
			//			{
			//				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Основная";
			//			}
			//			
			//			if ((Model.FurnitureSystem.Name == FurnSystRotoNT || Model.FurnitureSystem.Name == FurnSystRotoNTDesigno) && Model.ProfileSystem.Name == ProfSystVario && Model.UserParameters["Комплектация фурнитуры"].StringValue != "Противовзломная")
			//			{
			//				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";
			//			}
			//
			//			if (Model.FurnitureSystem.Name == ProfSystSlayding60)
			//			{
			//
			//			}
			//
			//			if (Model.ProfileSystem.Name == ProfSystVario && Model.FurnitureSystem.Name == FurnSystRotoNTDesigno && Model.UserParameters["Комплектация фурнитуры"].StringValue != "Противовзломная")
			//			{
			//				Model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";
			//			}
		}

		#endregion

		#region Пользовательские характеристики для системы Акция

		void UserProprietesAction()
		{
			Model.UserParameters["Штапик"].StringValue = "Скошенный";
			Model.UserParameters["Цвет уплотнений"].StringValue = "Черный";
		}

		#endregion

		#region Обработка пользовательских характеристик

		//Создано 08-12-2010
		void UserProperties()
		{
			// legacy кусок рулит что-то про стеклянные маятниковые двери, в целом = бред
			if (Model.ConstructionType.Name.Contains("маятник"))
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if (r.Fill.Marking.ToString().ToLower().Contains("без"))
						r.Fill = Model.ProfileSystem.Fills["Без_стекла_и_штапика"];
					else
					{
						if (Model.ProfileSystem.ToString().ToLower().Contains("стеклянные конструкции"))
							r.Fill = Model.ProfileSystem.Fills["M1 10"];
					}
				}
			}

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.UserParameters["Москитная сетка"] != null)
					lf.UserParameters["Москитная сетка"].Visible = !Model.ConstructionType.Name.Contains("маятник");
				if (lf.UserParameters["Дверные петели"] != null)
					lf.UserParameters["Дверные петели"].Visible = !Model.ConstructionType.Name.Contains("маятник") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная");
				if (lf.UserParameters["Доводчик"] != null)
					lf.UserParameters["Доводчик"].Visible = Model.ConstructionType.Name.Contains("Дверь") && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf && !Model.ConstructionType.Name.Contains("маятник");

				//Маятниковая дверь
				if (Model.ConstructionType.Name.Contains("маятник"))
				{
					lf.HingeCount = 0;
					lf.OpenType = OpenType.Маятниковое;
				}
			}
		}

		#endregion

		#region Обработка угловых соединений

		// Создано 21-12-2010 // 27-05-2019 http://yt:8000/issue/dev-253
		void ProfileConnection()
		{
			//Рама
			foreach (clsFrame fr in Model.Frame)
			{
				if (Model.ProfileSystem.Name.ToLower().Contains("стеклянные конструкции") && Model.ConstructionType.Name.Contains("маятник"))
				{
					switch (fr.PositionBeem.ToString())
					{
						case "Bottom":
							fr.ConnectType = ConnectType.Длинное;
							break;

						case "Left":
							fr.ConnectType = ConnectType.Короткое;
							break;

						case "Top":
							fr.ConnectType = ConnectType.Длинное;
							break;

						case "Right":
							fr.ConnectType = ConnectType.Короткое;
							break;
					}
				}
				else if (Model.ProfileSystem.Name.ToLower().Contains("слайдинг-60") || Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
				{
					if (!Model.FurnitureSystem.Name.ToLower().Contains("слайдинг-60"))
					{
						fr.Profile = Model.ProfileSystem.colProfileFrame["ROS0681.07"]; // ROS0681.07
						fr.ConnectType = ConnectType.Равное;
					}
					else
					{
						switch (fr.PositionBeem.ToString())
						{
							case "Bottom":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2277.07"]; // ROS0676.07
								fr.ConnectType = ConnectType.Короткое;
								break;

							case "Left":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2278.07"]; // ROS0677.07
								fr.ConnectType = ConnectType.Длинное;
								break;

							case "Top":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2276.07"]; // ROS0675.07
								fr.ConnectType = ConnectType.Короткое;
								break;

							case "Right":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2278.07"]; // ROS0677.07
								fr.ConnectType = ConnectType.Длинное;
								break;
						}
					}
				}
				else
				{
					if (fr.bType == ComponentType.Porog)
						fr.ConnectType = ConnectType.Короткое;
					else if (fr.Beem1.bType != ComponentType.Porog)
						fr.Connect1 = ConnectType.Равное;
					else if (fr.Beem2.bType != ComponentType.Porog)
						fr.Connect2 = ConnectType.Равное;
				}
			}
		}

		#endregion

		#region Изменение радиуса арки / импоста / створки

		public void ChangeRadius()
		{
			foreach (clsRegion r in Model.VisibleRegions)
			{
				#region Для триплексов

				if (r.Fill.Marking.Contains("3+3"))
				{
					//Рама
					foreach (clsFrame fr in Model.Frame)
					{
						if (r.IsConnectBeem(fr) && (fr.R1 != 0 || fr.R2 != 0))
						{
							MessageBox.Show("Построение арочных конструкций с использованием триплексов 3+3 запрещено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							fr.R1 = 0;
							fr.R2 = 0;
						}
					}

					// Импост
					foreach (clsImpost imp in Model.Imposts)
					{
						if (r.IsConnectBeem(imp) && (imp.R1 != 0 || imp.R2 != 0))
						{
							MessageBox.Show("Построение арочных конструкций с использованием триплексов 3+3 запрещено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							imp.R1 = 0;
							imp.R2 = 0;
						}
					}
				}

				#endregion
			}

			if (Model.ProfileSystem.Name == Pimapen || Model.ProfileSystem.Name == RehauEuro70)
			{
				//MessageBox.Show("Обработка угловых соединений");
				//Рама
				foreach (clsFrame fr in Model.Frame)
				{
					if (fr.R1 != 0 || fr.R2 != 0)
					{
						MessageBox.Show("Гнутие профилей в данной профильной системе запрещено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						fr.R1 = 0;
						fr.R2 = 0;
					}
				}

				// Импост
				foreach (clsImpost imp in Model.Imposts)
				{
					if (imp.R1 != 0 || imp.R2 != 0)
					{
						MessageBox.Show("Гнутие профилей в данной профильной системе запрещено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						imp.R1 = 0;
						imp.R2 = 0;
					}
				}
			}
		}

		#endregion

		#region Изменение цвета конструкции

		public void ChangeColor()
		{
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (Model.ConstructionType.Name == "Дверь входная")
				{
					if (!Model.ColorInside.ToString().ToLower().Contains("белый") || !Model.ColorOutside.ToString().ToLower().Contains("белый"))
						lf.UserParameters["Дверные петели"].StringValue2 = "Коричневый";
					else
						lf.UserParameters["Дверные петели"].StringValue2 = "Белый";
				}
			}
		}

		#endregion

		#region Изменение размера створки

		public void ChangeLeafSize()
		{
			ChangeFill();
			recalcHingeCount();
		}

		#endregion

		#region Изменение петель

		public void ChangeHinge()
		{
			// спарва проверим створки методом исползуемым при создании створки
			AddLeaf();

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.HingeCount > 4)
				{
					MessageBox.Show("Максимальное количество петель - 4", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
					lf.HingeCount = 4;
				}

				lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;

				if (Model.ConstructionType.Name.Contains("Дверь") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("красал"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная");
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("дверная") || !Model.FurnitureSystem.Name.ToLower().Contains("алюминий")) && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("Фурнитура Дверная (алюминий)");
						lf.UserParameters["Дверные петели"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.Поворотное)
						lf.OpenType = OpenType.Поворотное;

					lf.UserParameters["Дверные петели"].Visible = true;

					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;
				}
				else
				{
					if (Model.FurnitureSystem.Name.ToLower().Contains("дверная") && !Model.ConstructionType.Name.ToLower().Contains("межкомнатная") && !Model.FurnitureSystem.Name.ToLower().Contains("без"))
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG зачем менять фурнитуру при смене петель или их количества

					lf.UserParameters["Дверные петели"].Visible = false;

					if (Model.ConstructionType.Name.ToLower().Contains("межкомнатная") || Model.ConstructionType.Name.ToLower().Contains("балконная"))
					{
						lf.HingePosition = HingePosition.HingeProportional;
						if (lf.OpenType != OpenType.Поворотное && lf.HingeCount > 2)
						{
							MessageBox.Show("более 2х петль возможно только в поворотном типе открывания", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
							lf.HingeCount = 2;
							lf.HingePosition = HingePosition.HingeProportional;
						}
					}

					if (lf.HingeType != HingeType.WindowHinge)
					{
						//MessageBox.Show("Невозможно установить дверные петли!", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
						lf.HingeType = HingeType.WindowHinge;
					}
				}

				// на фрамугах только пропорциональные петли
				if ((lf.OpenType == OpenType.Откидное || lf.OpenType == OpenType.Подвесное) && lf.HingePosition != HingePosition.HingeProportional)
					lf.HingePosition = HingePosition.HingeProportional;
			}

			recalcHingeCount();
		}

		#endregion

		#region изменение типа соединения

		public void ChangeConnectionType()
		{
			foreach (clsFrame fr in Model.Frame)
			{
				// также имеются инварианты, короче этот аспект требует жёсткого рефауторинга с наведением порядка в легаси коде, шатно эот код не отрабатывает в случае подъёма старой изделии и вообще инварианты рулят
				if (fr.bType == ComponentType.Porog && (Model.ProfileSystem.Name == Pimapen || Model.ProfileSystem.Name == FORWARD || Model.ProfileSystem.Name == BAUTEK || Model.ProfileSystem.Name == BAUTEKmass))
					fr.ConnectType = ConnectType.Длинное;

				if (fr.bType == ComponentType.Porog && (Model.ProfileSystem.Name.ToLower().Contains("rehau 60") || Model.ProfileSystem.Name.ToLower().Contains("красал")))
					fr.ConnectType = ConnectType.Короткое;
			}

			// 25-06-2015 
			foreach (clsBeem beem in Model.Imposts)
			{
				clsImpost impost = beem as clsImpost;
				if (impost != null && impost.Selected)
				{
					if (impost.ConnectType != ConnectType.ImpostShort)
						impost.ConnectType = ConnectType.ImpostShort;
				}
			}
		}

		#endregion

		#region Изменение типа положения ручки

		public void ChangeHandlePosition()
		{
			//  запрет произвольных ручек
			foreach (clsLeaf leaf in Model.Leafs)
			{
				if (leaf.HandlePositionType == HandlePositionType.Произвольное)
				{
					leaf.HandlePositionType = HandlePositionType.Центральное;
				}
			}

			// обработаем константные привода для фурнитуры SigeniaTitan
			if (Model.FurnitureSystem.Name == SiegeniaTitanWK1 || Model.FurnitureSystem.Name == SiegeniaTitan || Model.FurnitureSystem.Name == TitanDebug)
			{
				foreach (clsLeaf leaf in Model.Leafs)
				{
					if (leaf.HandlePositionType == HandlePositionType.Фиксированное)
					{
						if (!getRestriction(leaf.Model))
							break;

						leaf.HandlePosition.IsAutomat = false;

						if (leaf.HandleBeem.LineE.Length > 0 && leaf.HandleBeem.LineE.Length <= 600)
							leaf.HandlePosition.HandlePosition = 180;
						else if (leaf.HandleBeem.LineE.Length > 600 && leaf.HandleBeem.LineE.Length <= 800)
							leaf.HandlePosition.HandlePosition = 300;
						else if (leaf.HandleBeem.LineE.Length > 800 && leaf.HandleBeem.LineE.Length <= 1000)
							leaf.HandlePosition.HandlePosition = 400;
						else if (leaf.HandleBeem.LineE.Length > 1000 && leaf.HandleBeem.LineE.Length <= 1200)
							leaf.HandlePosition.HandlePosition = 500;
						else if (leaf.HandleBeem.LineE.Length > 1200 && leaf.HandleBeem.LineE.Length <= 1400)
							leaf.HandlePosition.HandlePosition = 600;
						else if (leaf.HandleBeem.LineE.Length > 1400 && leaf.HandleBeem.LineE.Length <= 1600)
							leaf.HandlePosition.HandlePosition = 700;
						else if (leaf.HandleBeem.LineE.Length > 1600 && leaf.HandleBeem.LineE.Length <= 1800)
							leaf.HandlePosition.HandlePosition = 700;
						else if (leaf.HandleBeem.LineE.Length > 1800)
							leaf.HandlePosition.HandlePosition = 1000;
					}
				}
			}
				// ROTO NT константные запоры
				// положение ручки как функция от длинны балки ручки и возможно позиции ручки еслт хочется ее опустить, поднять выше максимального для этой ВСФ не даст
			else if (Model.FurnitureSystem.Name == RotoNT || Model.FurnitureSystem.Name == RotoNTDesigno)
			{
				foreach (clsLeaf leaf in Model.Leafs)
				{
					if (leaf.HandlePositionType == HandlePositionType.Фиксированное)
					{
						leaf.HandlePosition.IsAutomat = false;

						if (leaf.HandleBeem.LineE.Length <= 800 && leaf.HandlePosition.HandlePosition > 263)
							leaf.HandlePosition.HandlePosition = 263;
						else if (leaf.HandleBeem.LineE.Length <= 1000 && leaf.HandlePosition.HandlePosition > 413)
							leaf.HandlePosition.HandlePosition = 413;
						else if (leaf.HandleBeem.LineE.Length <= 1200 && leaf.HandlePosition.HandlePosition > 513)
							leaf.HandlePosition.HandlePosition = 513;
						else if (leaf.HandleBeem.LineE.Length <= 1800 && leaf.HandlePosition.HandlePosition > 563)
							leaf.HandlePosition.HandlePosition = 563;
						else if (leaf.HandleBeem.LineE.Length <= 2400 && leaf.HandlePosition.HandlePosition > 1000)
							leaf.HandlePosition.HandlePosition = 1000;
					}
				}
			}

			switch (Model.ConstructionType.Name)
			{
				case _indoor:
				case _outdoor:
				case _swingdoor:
				case _wcdoor:
					foreach (clsLeaf leaf in Model.Leafs)
						if (leaf.HandlePositionType != HandlePositionType.Центральное)
							leaf.HandlePositionType = HandlePositionType.Центральное;
					break;
			}
		}

		#endregion

		#region Изменение армирования

		public void ChangeMarkingSteel()
		{
			string furn_komplekt = Model.UserParameters["Комплектация фурнитуры"].StringValue;
			//Проверяем раму и в зависмости от длины балок выставляем армирование
			foreach (clsFrame fr in Model.Frame)
			{
				if (fr.Lenght > 2000 || furn_komplekt == "Противовзломная")
				{
					if (fr.MarkingSteel.ToString() == "245536")
						fr.MarkingSteel = "239583";
						//Акция
					else if (fr.MarkingSteel.ToString() == "245536_")
						fr.MarkingSteel = "239583_";
				}
				else if (fr.MarkingSteel.ToString() == "239583")
					fr.MarkingSteel = "245536";
					//Акция
				else if (fr.MarkingSteel.ToString() == "239583_")
					fr.MarkingSteel = "245536_";
			}

			//Проверяем ИМПОСТА
			foreach (clsImpost imp in Model.Imposts)
			{
				if (imp.Lenght > 2000 || furn_komplekt == "Противовзломная")
				{
					if (imp.MarkingSteel.ToString() == "245536")
						imp.MarkingSteel = "239583";
						//Акция
					else if (imp.MarkingSteel.ToString() == "245536_")
						imp.MarkingSteel = "239583_";
				}
				else if (imp.MarkingSteel.ToString() == "239583")
					imp.MarkingSteel = "245536";
					//Акция
				else if (imp.MarkingSteel.ToString() == "239583_")
					imp.MarkingSteel = "245536_";
			}

			// Ищем петлевую балку на раме для усиленния армирования в двери
			if (Model.ConstructionType.Name.ToLower().Contains("дверь"))
				foreach (clsLeaf lf in Model.Leafs)
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel.ToString() == "245536")
						lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel = "239583";
						//Акция
					else if (lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel.ToString() == "245536_")
						lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel = "239583_";
				}
		}

		#endregion

		#region Соединители и расширители

		public void ConnectorType()
		{
			/// маразмуз крепкус
			/// всё значительно прощще
			/// Model.Connectors[0].Profile= Model.ProfileSystem.colProfileConnectors.FromArticle("561570")


			bool DelConnect = false;
			string guid = "";

			foreach (clsFrame fr in Model.Frame)
				if (fr.PositionBeem.ToString() == "Bottom")
				{
					guid = fr.Guid;
				}

			foreach (clsConnector con in Model.Connectors)
			{
				if (con.ConnectBeem.Guid.ToString() == guid && con.Selected == true)
				{
					//удаляем то что нашли
					con.Selected = true;
					Model.Connectors.Remove(con);
					Model.ClearSelected();
					DelConnect = true;
					break;
				}
			}

			// а теперь поставим то что требуется если удали то что не требуется :)
			if (DelConnect)
			{
				foreach (clsFrame f in Model.Frame)
					if (f.Guid.ToString() == guid)
					{
						f.SelPointCenter.Selected = true;
						foreach (clsProfile conn in Model.WinDraw.ProfileSystems.FromName(Model.ProfileSystem.Name).colProfileConnectors)
						{
							if (conn.ConnectortypeName != null && conn.ConnectortypeName == "Подставочный профиль")
								Model.Connectors.Add(conn);
						}
					}
			}
		}

		#endregion

		#endregion

		public static void BeforeModelCalc(clsModel model)
		{
			// кусок инвариантности
			if (model.ProfileSystem.Name == Vario && model.FurnitureSystem.Name == RotoNTDesigno)
				model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";

			foreach (clsLeaf lf in model.Leafs)
			{
				lf.UserParameters["Дверные петели"].NumericValue = lf.HingeCount;
			}
			//////

			foreach (Invalidance invalidance in Validator.validateAndFix(model))
			{
				if (invalidance is Warning)
					continue;

				Invariant invariant = invalidance as Invariant;
				if (invariant != null && invariant.@fixed)
				{
				}
				else
				{
					string message = invalidance.message();
					addErrorUniq(model, message);
					// if (!SystemScript.RunCalc.errors.Contains(message))
					//     SystemScript.RunCalc.errors.Add(message);
				}
			}

			// апгрейд артикулов слайдинг-60
			if (model.ProfileSystem.Name == SystSlayding60)
			{
				foreach (clsBeem beem in model.Frame)
				{
					if (slaydingUpgradeDictionary.ContainsKey(beem.Profile.Marking))
					{
						clsProfile profile = model.ProfileSystem.GetFrameProfileByMarking(slaydingUpgradeDictionary[beem.Profile.Marking], false);
						if (profile != null)
							beem.Profile = profile;
						else
							throw new ArgumentException(string.Format("{0} не найден аналог", beem.Profile.Marking));
					}
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (slaydingUpgradeDictionary.ContainsKey(beem.Profile.Marking))
						{
							clsProfile profile = model.ProfileSystem.GetLeafProfileByMarking(slaydingUpgradeDictionary[beem.Profile.Marking], false);
							if (profile != null)
								beem.Profile = profile;
							else
								throw new ArgumentException(string.Format("{0} не найден аналог", beem.Profile.Marking));
						}
					}
				}
			}
		}

		private static readonly Dictionary<string, string> slaydingUpgradeDictionary = new Dictionary<string, string>();

		static RunCalc()
		{
			slaydingUpgradeDictionary.Add("ROS0677.07", "ROS2278.07");
			slaydingUpgradeDictionary.Add("ROS0675.07", "ROS2276.07");
			slaydingUpgradeDictionary.Add("ROS0676.07", "ROS2277.07");
			slaydingUpgradeDictionary.Add("ROS0678.07", "ROS5063.07");
			slaydingUpgradeDictionary.Add("ROS0680.07", "ROS5065.07");
			slaydingUpgradeDictionary.Add("ROS0679.07", "ROS5064.07");
		}

		/// Вызывается в том числе и из заказа при смене пользовательских характеристик, цвета, etc...
		public void AfterModelCalc(clsModel model)
		{
			/// поправка размера МС Оконных и Дверных
			/// самостоятельные конструкции МС тоже правим, Оконная М/С как и ранее сртроится по проёму
			foreach (Moskit moskit in model.Construction.MoskitList)
			{
				if (moskit.Stvorka == null)
					continue;

				UserParam up = moskit.Stvorka.GetUserParam("Москитная сетка");
				if (up != null && (up.StrValue == "Оконная" || up.StrValue == "Дверная"))
				{
					/// ~todo XXX этот размер взаимосвязан со скриптом расчёта
					/// оконные +50 по габариту к проёмуб дверные +40 по габариту к проёму
					/// это зависит от используемого профиля
					/// профиль конешно стоит описать в системах 
					/// но пока нет системы Москитные сетки
					int gap = up.StrValue == "Оконная" ? 50 : 40;
					moskit.Height += gap;
					moskit.Width += gap;
					moskit.Perimetr = 2 * (moskit.Height + moskit.Width);
					moskit.Sqr = moskit.Height * moskit.Width;
					// moskit.SqrSetka = площадь проёма, может использовацца как флаг
				}
			}


			/// лечим баги АТ на лету
			/// при смене системы АТ НЕ ПРОСТАВЛЯЕТ ИНФОРМАЦИЮ О ШТУЛЬПЕ В ШТУЛЬПОВУЮ СТВОРКУ !!!! УРОДЫ
			/// лечится проходом по ободу створки 
			if (model.Construction != null)
			{
				foreach (Stvorka stvorka in model.Construction.StvorkaList)
				{
					if (stvorka.ShtulpExist == ShtulpExist.Exist && stvorka.Shtulp == null)
					{
						foreach (StvorkaItem si in stvorka)
						{
							if (si.Impost != null && si.Impost.ImpostType == ImpostType.Shtulp)
								stvorka.Shtulp = si.Impost;
						}
					}
				}
			}

			// продолжаем лечить баги AT
			// fix неправильного определения формы заполнения, про стандартные часто неапрвильно ставит нестандарт ибо допуск по углам стандартно 0,001 градуса что нереально жостко 
			fixGlassFormType(model);

			// рассчитать clsModel.Nalichnik -> Construction.Nalichnik
			NalichnikRestriction.calc(model);

			// некоторые элемент рамы заведомо будут крепиться к ссоеднему , добавим здесь что-то полезное на что потом может ориентироваться расчет монтажных материалов
			markConjuctionRamaItem(model);

			// 04-07-2018 размер подставочного профиля всегда на 100 мм меньше чем указано в нативной модели, относительно "всегда" спрашивал специально, далее по ходу пьесы инвормацию о типе соединителя вытаскивать сложнее
			fixPodstavochikThick(model);

			// пишем дату расчёта в конструкцию (class)
			// ибаные колхозники, пишут в это поле не при создании модели а при её сериализации, БЛДЪ!! но писать туда щас , нах, достаточно будет .DateModyfied >= .creat
			// Model.Construction.DateCreate = Model.CreateTime;
			model.Construction.DateModyfied = DateTime.Now;

			// эти проверки мы можем делать (пока) только на уже рассчитанной модели поэтому они тут а не в цикле человеколюбивих бубликов 
			{
				// на служебных дверях нельзя ставить импост в створку от 920 до 1120 // 900 - 1170
				if (model.ConstructionType.Name == _wcdoor)
				{
					// нет понятия положение импоста в clsModel
					const int x0 = 900;
					const int x1 = 1170;

					foreach (Stvorka stvorka in model.Construction.StvorkaList)
					{
						foreach (StvorkaItem si in stvorka.HandleBeems)
						{
							for (int i = 0; i < si.ImpostList.Count; i++)
							{
								Impost impost = si.ImpostList[i];
								int x = si.Side == ItemSide.Left ? si.Lenght - si.ImpostPositionList[i] : si.ImpostPositionList[i];
								int a = (int) Math.Ceiling(impost.A);

								if (x0 < x - a && x - a < x1 || x0 < x + a && x + a < x1)
									if (restriction)
										addErrorUniq(model, string.Format("В конструкции {0} импост створки не должен присоединяться к балке ручки на высоте от {1} до {2} мм, (с учетом толщины импоста {4}мм). Текущее положение {3}мм.",
											model.ConstructionType.Name, x0, x1, x, a * 2));
							}
						}
					}
				}

				// нельзя из импоста делать ласточкин хвост :( но инфрмация об углах реза появляется только после расчёта модели
				foreach (Impost impost in model.Construction.ImpostList)
				{
					const int delta = 12;
					if (impost.BalkaEnd1 != impost.BalkaEnd2 && Math.Abs(impost.CutAngleEnd1 - impost.CutAngleEnd2) > delta || impost.BalkaStart1 != impost.BalkaStart2 && Math.Abs(impost.CutAngleStart1 - impost.CutAngleStart2) > delta)
					{
						if (restriction)
							addErrorUniq(model, string.Format("Импост {0} - две грани реза у одного из концов, передвинте конец в положение с одной гранью реза", impost.ID));
					}
				}
			}

			/// проставим параметр пользовательский параметр Профилезависимые как ключ для профилезависимых компонентов систем
			{
				const string ProfileDependent = "Профилезависимые";
				/// TODO LESS UGLY
				clsUserParam profileDependentUserParam = model.UserParameters.GetParam(ProfileDependent);
				if (profileDependentUserParam != null)
				{
					switch (model.ProfileSystem.Name)
					{
						case Gealan:
							profileDependentUserParam.StringValue = "Gealan";
							break;
						case BAUTEK:
						case BAUTEKmass:
						case FORWARD:
						case FavoritSpace:
							profileDependentUserParam.StringValue = "Deceuninck";
							break;
						default:
							profileDependentUserParam.StringValue = isPvh(model) ? "Rehau" : null;
							break;
					}
				}
			}
		}

		private static void addErrorUniq(clsModel model, string message)
		{
			// M$ SUCKS
			// https://stackoverflow.com/questions/386122/correct-way-to-escape-characters-in-a-datatable-filter-expression
			// приходится вырезать спецсимволы руакми, M$ уроды
			message = message.Replace("'", string.Empty);


			// if (!SystemScript.RunCalc.errors.Contains(message))
			//     SystemScript.RunCalc.errors.Add(message);
			// CalcProcessor.Modules["addError"](new object[] { model.dr_model, message });

			// стандартный способ получения класса заказа
			OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];

			ds_order.orderitemRow orderitemRow = model.dr_model as ds_order.orderitemRow;

			if (orderitemRow != null)
			{
				// уникальность текста ошибки для этой позиции
				if (order.ds.ordererror.Select(string.Format("idorderitem = {0} and addinfo = '{1}'", orderitemRow.idorderitem, message)).Length == 0)
				{
					ds_order.ordererrorRow ordererrorRow = order.ds.ordererror.CreateRow(order.idorder, orderitemRow.idorderitem);
					ordererrorRow.addinfo = message;
				}
			}
			else
			{
				// уникальность текста ошибки для закзаа
				if (order.ds.ordererror.Select(string.Format("idorderitem IS NUL and addinfo = '{0}'", message)).Length == 0)
				{
					ds_order.ordererrorRow ordererrorRow = order.ds.ordererror.CreateRow(order.idorder);
					ordererrorRow.addinfo = message;
				}
			}
		}

		private static void fixPodstavochikThick(clsModel model)
		{
			const int delta = -10;
			foreach (Soedinitel so in model.Construction.SoedinitelList)
			{
				// 1	Подставочный профиль
				if (so.IDConnectorType == 1)
				{
					so.Lenght += delta;
					so.LengthDouble += delta;
				}
			}
		}

		private void markConjuctionRamaItem(clsModel model)
		{
			foreach (clsModelConnection connection in model.ModelConnections)
			{
				// не свой коннетор тогда пишем ri.W
				if (connection.connectors != null && connection.connectors.Count > 0 && connection.connectors[0].Model != model)
				{
					if (connection.firstModel == model)
					{
						markConcreteBeam(connection.FirstFrame, getCommonLength(connection.FirstFrame, connection.connectors[0]));
					}

					if (connection.secondModel == model)
					{
						markConcreteBeam(connection.SecondFrame, getCommonLength(connection.SecondFrame, connection.connectors[0]));
					}
				}
			}
		}

		// теперь в ri.W у нас будет длинна части балки сопряженной с соединителм из соседней модели, что мы будем учитывать при расчётах в дальнейшем, по месту хранения принимаются предложенияS
		private void markConcreteBeam(clsFrame beem, double length)
		{
			Balka balka = beem.Model.Construction.GetBalkaByID(beem.VisibleID, ModelPart.RamaItem);
			if (balka != null)
				balka.W = Math.Round(length);
		}

		// TODO
		private double getCommonLength(clsFrame beem, clsConnector connector)
		{
			// вычисление общая длина активной рамы и соединителя
			double c = connector.Lenght;
			double r = connector.GetFrameRoot().Lenght;
			if (connector.Position < 0)
				c += connector.Position;
			else if (connector.Position > 0)
				r -= connector.Position;

			double common = Math.Min(c, r);

			// вычисление общая длина общей на приедыдущем шаге и пассивной рамы 
			double p = beem.Lenght;
			double mp = beem.Model.ConstructionOffset;

			if (mp < 0)
				p += mp;
			else if (mp > 0)
				common -= mp;

			double result = Math.Min(p, common);

			return result;
		}

		// я тебя научу - родину любить - как // ставить на стандартные стекла флажок стандарт
		public static void fixGlassFormType(clsModel model)
		{
			if (model.Construction != null)
			{
				foreach (Atechnology.winDraw.Model.Glass glass in model.Construction.GlassList)
				{
					// меняем только нестандарт -> стандарт, если нестандарт установлено ошибочно
					if (glass.glassFormType != GlassFormType.Square && glass.Count == 4)
					{
						bool square = true;

						foreach (GlassItem gi in glass)
						{
							const decimal TOLERANCE = 0.05m;
							if (gi.Radius1 != 0 || gi.Radius2 != 0 || Math.Abs(gi.Ang1 - 90) > TOLERANCE || Math.Abs(gi.Ang2 - 90) > TOLERANCE)
							{
								square = false;
								break;
							}
						}

						if (square)
						{
							glass.glassFormType = GlassFormType.Square;
						}
					}
				}
			}
		}

		#region Отрисовка

		private static void Draw(clsModel model)
		{
			try
			{
				// отрисовка наличника только в построителе
				//if (model.WinDraw.frmBuild == null)
				//	return;

				// отрисовать наличник если есть
				NalichnikRestriction.draw(model);

				// створка профиль / апримровние / вклкйка
				if (Settings.idpeople == 255)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						float x = (float) leaf.BaseRegion.MinX(0);
						float y = (float) leaf.BaseRegion.MinY(0);

						clsUserParam up1 = leaf.UserParameters["Вклейка Стеклопакета"];
						clsUserParam up2 = leaf.UserParameters["_arm"];

						string s = string.Format("{0} [{1}:{4}] {2}|{3} ", leaf[0].Profile.Marking, leaf[0].MarkingSteel, up1.StringValue, up1.StringValue2, up2.StringValue);
						model.Canvas.DrawString(s, myFont, myBrush, new RectangleF(x + 100, y, 800, 32), null);
					}
				}

				// Lisec expirement
				if (Settings.idpeople == 255)
				{
					foreach (clsRegion region in model.VisibleRegions)
					{
						if (region.IsSquare())
							continue;

						float x = (float) (region.MinX(0) + region.MaxX(0)) / 2;
						float y = (float) (region.MinY(0) + region.MaxY(0)) / 2;

						LisecShape shape = Lisec.GetShape(region);

						StringBuilder sb = new StringBuilder();

						foreach (LisecParam lisecParam in Enum.GetValues(typeof(LisecParam)))
						{
							if (lisecParam == LisecParam.None)
								continue;

							try
							{
								double value = shape[lisecParam];
								if (value > 0)
								{
									sb.Append(string.Format(" {0}={1:F0}", lisecParam, value));
								}
							}
							catch
							{
								sb.Append(string.Format(" {0}:E", lisecParam));
							}
						}

						string index;
						try
						{
							index = shape.Index.ToString();
						}
						catch
						{
							index = "EE";
						}

						string s = string.Format("{0}) {1}", index, sb.ToString());

						model.Canvas.DrawString(s, myFont, myBrush, new RectangleF(x - 300, y - 200, 600, 400), null);
					}
				}

				// цвет м/с
				try
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.Нет)
						{
							clsUserParam up = leaf.UserParameters.GetParam("Москитная сетка");
							if (up != null && up.StringValue2 != up.DefaultValue.StrValue2)
							{
								double x = (leaf.MinX + leaf.MaxX) / 2;
								double y = (leaf.MinY + leaf.MaxY) / 2;
								model.Canvas.DrawString("м/с: " + up.StringValue2, myFont, myBrush, new RectangleF((float) x - 60, (float) y - 110, 200, 100), null);
							}
						}
					}
				}
				catch
				{
					// ignore
				}

				// цвет ручек створок                 // цвет петель
				foreach (clsLeaf leaf in model.Leafs)
				{
					clsUserParam upMechanizm = leaf.UserParameters.GetParam(__mechanizm);
					if (upMechanizm != null)
					{
						switch (upMechanizm.StringValue2)
						{
							case __brown:
								leaf.HandleColor = Color.SaddleBrown;
								break;
							// todo :
							case __whitebrown:
								leaf.HandleColor = Color.FromArgb(64, 192, 96, 0);
								break;
							case __bronze:
								leaf.HandleColor = Color.Orange;
								break;
							case __silver:
								leaf.HandleColor = Color.Silver;
								break;
							case __gold:
								leaf.HandleColor = Color.Yellow;
								break;
							case __darksilver:
								leaf.HandleColor = Color.DimGray;
								break;
							default:
								leaf.HandleColor = Color.White;
								break;
						}
					}

					/// BUG !
					/// if (upMechanizm != null && model.FurnitureSystem.Name == FurnSystDver)
					/// так оно конечно логинчно но цвет петель для входной двери определяется петялми которые определяются ручкой, 
					/// а для всего прочего - цветом накладок
					if (model.ConstructionType.Name == _outdoor && upMechanizm != null)
					{
						switch (upMechanizm.StringValue2)
						{
							case __brown:
								leaf.HingeColor = Color.SaddleBrown;
								break;
							// todo :
							case __whitebrown:
								leaf.HingeColor = Color.FromArgb(64, 192, 96, 0);
								break;
							default:
								leaf.HingeColor = Color.FromArgb(224, 224, 224, 224);
								break;
						}
					}

					clsUserParam upNP = model.UserParameters.GetParam("Накладки на петли");
					if (model.ConstructionType.Name != _outdoor && upNP != null /*&& (model.FurnitureSystem.Name.ToLower().Contains("roto") || model.FurnitureSystem.Name.ToLower().Contains("siegenia"))*/)
					{
						switch (upNP.StringValue)
						{
							case "Коричневый":
								leaf.HingeColor = Color.SaddleBrown;
								break;
							case "Серебро":
								leaf.HingeColor = Color.LightGray;
								break;
							case "Золото матовое":
							case "Золото блестящее":
								leaf.HingeColor = Color.Yellow;
								break;
							case "Бронза средняя":
								leaf.HingeColor = Color.Orange;
								break;
							case "Без накладок":
								leaf.HingeColor = Color.DimGray;
								break;
							case "Белый":
							default:
								leaf.HingeColor = Color.FromArgb(224, 224, 224, 224);
								break;
						}
					}
				}
			}
			catch
			{
				// ignored 
			}
		}

		static readonly Font myFont = new Font("Arial", 8.0f);
		static readonly Brush myBrush = Brushes.Blue;

		#endregion

		// расчет количества петель по ширине фрамуги
		private void recalcHingeCount()
		{
			foreach (clsLeaf leaf in Model.Leafs)
			{
				// нас интересуют только откидные и подвесные фрамуги
				if (leaf.OpenType == OpenType.Подвесное || leaf.OpenType == OpenType.Откидное)
				{
					switch (leaf.Model.FurnitureSystem.Name)
					{
						case SiegeniaTitan:
						case SiegeniaTitanWK1:
						case SiegeniaClassic:
							if (leaf.HingBeem.LineE.Length <= 800)
								leaf.HingeCount = 2;
							else if (leaf.HingBeem.LineE.Length <= 1800)
								leaf.HingeCount = 3;
							else
								leaf.HingeCount = 4;
							break;

						case RotoNT:
							if (leaf.HingBeem.LineE.Length <= 1600)
								leaf.HingeCount = 2;
							else
								leaf.HingeCount = 3;
							break;

						case RotoNTDesigno:
							leaf.HingeCount = 0;
							break;
					}
				}
					// Designo kill them all
				else if (leaf.Model.FurnitureSystem.Name == RotoNTDesigno || leaf.Model.FurnitureSystem.Name == SiegeniaAxxent)
				{
					leaf.HingeCount = 0;
				}
			}
		}

		/// валидация модели внутри построителя, и вывод чудесных бубликов внизу ПОСТРОИТЕЛЯ!, 
		/// это не выполняется при фоновом расчете без построителя
		private static void validationProcessor(clsModel model)
		{
			// делаем копию списка сообщений
			List<WinDrawMessage> tempList = new List<WinDrawMessage>(model.WinDraw.Messages);

			// очищаем список ведра
			List<WinDrawMessage> shadow = (List<WinDrawMessage>) model.WinDraw.Messages;
			shadow.Clear();

			// оставляем там всех FixedInvariantWinDrawMessage, 
			// тут мы не рулим их жизненным циклом, а лишь сохраняем их персистетнтность
			foreach (WinDrawMessage message in tempList)
				if (message is FixedInvariantWinDrawMessage)
					shadow.Add(message);

			bool isChange = false;
			// инварианты
			foreach (Invalidance invalidance in Validator.validateAndFix(model))
			{
				Invariant invariant = invalidance as Invariant;

				// если тихий инвариант то не добавляем сообщение
				if (invariant is SilentInvariant)
					continue;

				if (invariant != null && invariant.@fixed)
				{
					if (!string.IsNullOrEmpty(invariant.message()))
					{
						FixedInvariantWinDrawMessage message = new FixedInvariantWinDrawMessage();
						message.ID = invariant.uniq();
						message.Message = string.Format("          {0} // Исправлено автоматически", invariant.message()); // todo на то-то то-то ибо бывает более одного варианта
						message.Model = model;

						Control helpPanel = model.WinDraw.frmBuild.Controls.Find("HelpPanel", true)[0];
						Color saveColor = helpPanel.BackColor;
						helpPanel.BackColor = Color.FromArgb(192, 255, 192);
						model.WinDraw.AddMessage(message);
						helpPanel.BackColor = saveColor;
					}
				}
				else
				{
					WinDrawMessage message = new WinDrawMessage();
					message.ID = invalidance.uniq();
					message.Message = string.Format("          {0}", invalidance.message());
					message.Model = model;
					model.WinDraw.AddMessage(message);
				}

				isChange = true;
			}

			// сносим ушедших
			foreach (WinDrawMessage message in tempList)
			{
				FixedInvariantWinDrawMessage invariantMessage = message as FixedInvariantWinDrawMessage;

				if (invariantMessage != null)
				{
					if (!invariantMessage.isLive() && isChange)
					{
						model.WinDraw.RemoveMessage(invariantMessage);
					}
				}
				else
				{
					if (!shadow.Contains(message) && message.ID != "door")
					{
						model.WinDraw.RemoveMessage(message);
					}
				}
			}
		}

		/// долгоживущее сообщение
		public class FixedInvariantWinDrawMessage : WinDrawMessage
		{
			private const int TTL = 10;
			private readonly DateTime dateTime = DateTime.Now;

			public bool isLive()
			{
				return (DateTime.Now - dateTime).TotalSeconds < TTL;
			}
		}

		internal static class Validator
		{
			/// возвращает исправленных инвариантов и ошибок
			public static IEnumerable<Invalidance> validateAndFix(clsModel model)
			{
				// накопитель инвариантов которых мы могли попытацца пофиксить
				List<Invalidance> invariants = new List<Invalidance>();

				// просто переменная
				List<Invalidance> invalidances = null;

				int i = 0;
				const int max_i = 12;
				// цикл валидации
				bool fix = true;
				while (fix)
				{
					// сброс флажка
					fix = false;

					// валидация, если не будет инвариантов то на выход с этим списком
					invalidances = Validator.validate(model);

					// попытка пофиксить имеющиеся инварианты, что может покоцать модель до невалидности и не гарантирует появления неприятностей
					foreach (Invalidance invalidance in invalidances)
					{
						Invariant invariant = invalidance as Invariant;
						if (invariant != null)
						{
							try
							{
								invariant.fix();
								fix = true;
							}
							catch
							{
								invariant.@fixed = false;
							}

							invariants.Add(invariant);
						}
					}

					if (i++ > max_i)
					{
						invalidances.Add(new FixInvalidance(model, "достигнут предел количества попыток валидации модели"));
						break;
					}
				}

				invalidances.AddRange(invariants);

				return invalidances;
			}


			private static List<Invalidance> validate(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>();
				// габариты
				list.Add(GabaritRestriction.test(model));
				// тесты по всем системам
				list.Add(OppositeOpenView.test(model));
				list.AddRange(TopologyRestriction.test(model));
				//list.Add(WindowsFurniturePorogInvalidance.test(model));				
				list.Add(OknaDaSystemRestriction.test(model));
				list.Add(WoodLineRestriction.test(model));
				list.AddRange(DoorInvalidance.test(model));
				list.AddRange(DecorRestriction.test(model));
				list.AddRange(ClassicRestriction.test(model));
				list.AddRange(DeceuninckRestriction.test(model));
				list.AddRange(GealanRestriction.test(model));
				list.AddRange(EuroSibOptimaMaximaRestriction.test(model));
				list.AddRange(RuhauGrazioRestriction.test(model));
				list.AddRange(RuhauBlitzNewRestriction.test(model));
				list.AddRange(SchtandartRestriction.test(model));
				list.AddRange(Thermo76Restriction.test(model));
				list.AddRange(Neo80Restriction.test(model));

				list.AddRange(OrderTypeRestriction.test(model));
				list.Add(NalichnikRestriction.test(model));

				list.AddRange(OneImpostOneHinge.test(model));

				/// ограничение рото ок, AHTUNG !! AHTUNG !! , 
				/// проверка меняет сосотояние model.userParam["Замена фурнитуры"] 
				/// в случе не прохождения ограничений ROTO OK => 1 // Замена фурнитуры:Фурнитура ROTO NT
				/// и должна вседа сбрасывать первым делом ибо пользователь может сменить фурнитуру => 0 Замена фурнитуры:String.Empty
				list.AddRange(RotoOkRestriction.test(model));
				list.AddRange(RotoNTRestriction.test(model));
				list.AddRange(GiesseRestriction.test(model));
				list.AddRange(SiegeniaClassicRestriction.test(model));
				list.AddRange(TitanRestriction.test(model));
				list.AddRange(SiegeniaAxxentRestriction.test(model));
				list.AddRange(AxorRestriction.test(model));

				// створки
				foreach (clsLeaf leaf in model.Leafs)
				{
					list.Add(TZInvalidance.test(leaf));
					list.Add(DoorPassiveDependanceInvariant.test(leaf));

					HingeInvariant.test(leaf);
				}

				// заполнения
				foreach (clsRegion region in model.VisibleRegions)
				{
					list.Add(Shpros16Restriction.test(region));
					list.AddRange(FillRestriction.test(region));
				}

				list.AddRange(SpreadingRestriction.test(model));

				// тесты по EVO
				if (model.ProfileSystem.Name == EVO)
				{
					// общие параметры модели
					// list.Add(EvoColor.test(model));
					list.Add(GeneoShtapikInvariant.test(model));
					list.Add(EvoRotoNT.test(model));
					list.AddRange(EvoProfileRestriction.test(model));

					// створки
					foreach (clsLeaf leaf in model.Leafs)
					{
						list.Add(EvoImpostInZ64.test(leaf));
						list.AddRange(EvoLeafRestriction.test(leaf));
					}

					// заполнения
					foreach (clsRegion region in model.VisibleRegions)
					{
						list.Add(EvoFill44.test(region));
					}

					// балки не арки и нет косых углов
				}


				///// ALUTECH
				if (model.ProfileSystem.Name == ALUTECH62 || model.ProfileSystem.Name == ALUTECH_48)
				{
					// инварианты, могет быть более одно балки порога
					list.AddRange(AlutechPorogInvariant.test(model));
					list.AddRange(AlutechHeterogenLeafInvariant.test(model));
					list.AddRange(AlutechLeafFrameConjunctionInvariant.test(model));

					// створочный заход
					foreach (clsLeaf leaf in model.Leafs)
					{
						list.AddRange(AlutechSocleInvariant.test(leaf));
					}

					/// todo не тут конечно но, допустим fix() поменяет модель так что она уже не будет проходитьь по ограничениям прогонять еще раз можно , но накладно
					/// надо сначала прогонять по инвариантам фиксить их потом прогонять по ограничениям


					// ограничения
					list.AddRange(AlutechProfileRestriction.test(model));
				}

				// m/c
				list.Add(MoskitInvalidance.test(model));

				// цвета
				list.Add(ColorRestriction.test(model));

				// common PVH Profile restriction
				list.AddRange(RehauProfileRestriction.test(model));

				// Portal
				list.AddRange(PortalRestriction.test(model));

				// Слайдинг
				list.AddRange(SlidingRestriction.test(model));

				// Accesories Color Automat
				list.AddRange(AccessoriesColorAutomat.test(model));

				// маркетинг
				list.AddRange(ProductDefault.test(model));

				//list.RemoveAll(invalidance => invalidance == null);
				// ReSharper disable once ConvertToLambdaExpression
				list.RemoveAll(delegate(Invalidance invalidance) { return invalidance == null; });

				return list;
			}
		}

		private class FixInvalidance : Invalidance
		{
			private readonly string _message;

			public FixInvalidance(clsModel model, string message) : base(model)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}
		}

		public abstract class Invalidance
		{
			// субъект проблемы, засада но Stvorka (clsLeaf) не имеет ничего общего например с clsFrame, хотя оба это части clsModel
			protected object subject;

			// требуем проблему в студию - жостко
			protected Invalidance(object subj)
			{
				subject = subj;
			}

			/// todo потенциально можно изобразить некоторе хранилищице табличку  Map { Invalidance.typeName ; String Error }
			/// и оптом их оттудова тырить, но при этом может терятья привязка к месту 
			/// с другой стороны можна %1 {0} использовать, потом  может что-то решим
			public abstract string message();

			/// оставим .hash() всевышнему
			/// групировщик уникальности 
			/// позволяет например для штульповых створок выдавать только одно предупреждение в построителе
			/// не плодить однотипные безсмыссленные сообщения
			public virtual string uniq()
			{
				return GetType().Name;
			}

			// типа библиотечная функция // todo refactor EVO нах оно _всем_ не надо.
			protected static bool isZ64(clsLeaf leaf)
			{
				return leaf.Model.ProfileSystem.Name == EVO && leaf.HingBeem.Profile.A == 64;
			}

			protected static bool getRestriction(clsModel model)
			{
				try
				{
					OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
					DataRow[] drdocsign = order.ds.docsign.Select("sign_name = 'Заказ' and signvalue_str = 'Отмена ограничений' and deleted is NULL");
					if (drdocsign.Length > 0)
						foreach (int i in Settings.idpeoplegroup)
							if (i == 10 || i == 36 || i == 44 || i == 45 || i == 46)
								return false;
				}
				catch (Exception)
				{
				}

				return true;
			}


			protected static clsBeem getSkewOrArcBeem(clsModel model)
			{
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0 || beem.R2 > 0 || beem.PositionBeem == ItemSide.Other)
						return beem;
				}

				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0 || beem.R2 > 0 || beem.PositionBeem == ItemSide.Other)
						return beem;
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0 || beem.R2 > 0 || beem.PositionBeem == ItemSide.Other)
							return beem;
					}
				}

				return null;
			}

			protected static clsBeem getArcBeem(clsModel model)
			{
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
						return beem;
				}

				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
						return beem;
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0 || beem.R2 > 0)
							return beem;
					}
				}

				return null;
			}
		}

		// не ошибка и не фикситься - просто предупреждение пользователю
		private class Warning : Invalidance
		{
			private readonly string _message;

			public Warning(object subj, string message) : base(subj)
			{
				this._message = message;
			}

			public override string message()
			{
				return _message;
			}
		}

		// фикситься с мессаджем
		private abstract class Invariant : Invalidance
		{
			/// todo для совместимости с унаследованным кодом по умолчанию пофиксено, поправить позже
			public bool @fixed = true;

			protected Invariant(object subj) : base(subj)
			{
			}

			public abstract void fix();
		}

		// фиксситься безшумно
		private abstract class SilentInvariant : Invariant
		{
			protected SilentInvariant(object subj) : base(subj)
			{
			}

			public override string message()
			{
				return string.Empty;
			}
		}

		// запрет импостов в створке EVO.Z64 
		private class EvoImpostInZ64 : Invalidance
		{
			protected EvoImpostInZ64(clsLeaf leaf)
				: base(leaf)
			{
			}

			public override string message()
			{
				return "Импост в створке L";
			}

			public static Invalidance test(clsLeaf leaf)
			{
				if (isZ64(leaf))
				{
					foreach (clsImpost impost in leaf.Imposts)
					{
						return new EvoImpostInZ64(leaf);
					}
				}

				return null;
			}
		}

		// только серое уплотнение
		private class GeneoShtapikInvariant : Invariant
		{
			protected const string value = "Серый";

			// можливо будут грузиться абы откуда
			public GeneoShtapikInvariant(object subj)
				: base(subj)
			{
			}

			public override string message()
			{
				return "EVO: только серое уплотнение";
			}

			public static Invalidance test(clsModel model)
			{
				if (model.ProfileSystem.Name == EVO && model.UserParameters["Цвет уплотнений"].StringValue != value)
				{
					return new GeneoShtapikInvariant(model);
				}

				return null;
			}

			public override void fix()
			{
				clsModel model = subject as clsModel;
				if (model != null)
				{
					model.UserParameters["Цвет уплотнений"].StringValue = value;
				}
			}
		}

		private class EvoFill44 : Invariant
		{
			protected EvoFill44(clsRegion region)
				: base(region)
			{
			}

			public override string message()
			{
				return "Система EVO заполнение только 44 мм";
			}

			public static Invalidance test(clsRegion region)
			{
				if (region.Fill.Thikness != 44 && getRestriction(region.Model))
					return new EvoFill44(region);

				return null;
			}

			public override void fix()
			{
				clsRegion region = subject as clsRegion;
				if (region != null)
				{
					region.Fill = region.Model.ProfileSystem.Fills["4x16x4x16xИ4"];
				}
			}
		}

		private class EvoRotoNT : Invalidance
		{
			private static readonly List<string> furnSystems = new List<string>(new string[] {SiegeniaTitan, SiegeniaAxxent});

			public static Invalidance test(clsModel model)
			{
				if (!furnSystems.Contains(model.FurnitureSystem.Name) && model.FurnitureSystem.Name != FurnSystBEZ)
					return new SimpleInvariant(model, string.Format("{0} только фурнтиуры {1}", model.ProfileSystem.Name, string.Join(",", furnSystems.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan); });

				return null;
			}

			public EvoRotoNT(object subj) : base(subj)
			{
			}

			public override string message()
			{
				return null;
			}
		}

		private class EvoProfileRestriction : Invalidance
		{
			private readonly string _message;

			public override string message()
			{
				return _message;
			}

			public EvoProfileRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// рама
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0)
						list.Add(new EvoProfileRestriction(beem, string.Format("{0} не гнется, акри недопустимы", beem.Name)));

					if (beem.PositionBeem == ItemSide.Other)
						if (getRestriction(model))
							list.Add(new EvoProfileRestriction(beem, string.Format("{0} косые углы недопустимы", beem.Name)));
				}

				// створки
				foreach (clsLeaf leaf in model.Leafs)
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0)
							list.Add(new EvoProfileRestriction(beem, string.Format("Балка створки {1} {0} не гнется, акри недопустимы", beem.Name, leaf.Name)));

						if (beem.PositionBeem == ItemSide.Other)
							list.Add(new EvoProfileRestriction(beem, string.Format("Балка створки {1} {0} косые углы недопустимы", beem.Name, leaf.Name)));
					}

				// импосты
				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0)
						list.Add(new EvoProfileRestriction(beem, string.Format("Балка импоста {0} не гнется, акри недопустимы", beem.Name)));

					if (beem.PositionBeem == ItemSide.Other)
						list.Add(new EvoProfileRestriction(beem, string.Format("Балка импоста {0} косые углы недопустимы", beem.Name)));
				}

				// только  скошенный штапик
				const string Скошенный = "Скошенный";
				clsUserParam up = model.UserParameters.GetParam("Штапик");
				if (up != null && up.StringValue != Скошенный)
				{
					list.Add(new SimpleInvariant(model, string.Format("только {0} штапик", Скошенный), delegate { up.StringValue = Скошенный; }));
				}

				// не ставить фальш на створку Z64
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.SpreadingV2 != null && region.SpreadingV2.Type == SpreadingType.Falsh && region._Leaf != null && isZ64(region._Leaf))
					{
						list.Add(new EvoProfileRestriction(region._Leaf, string.Format("Фальш не ставится на створку {0}", region._Leaf[0].Profile.Marking)));
					}
				}

				return list;
			}
		}

		// todo можно наделать для разных систем с одним предком
		private class EvoLeafRestriction : Invalidance
		{
			private const int minHeight = 400;
			private const int maxHeight = 2600;

			private const int minWidth = 290;
			private const int maxWidth = 1600;

			private const int minHeightZ64 = 400;
			private const int maxHeightZ64 = 2000;

			private const int minWidthZ64 = 290;
			private const int maxWidthZ64 = 900;


			private readonly string _message;

			protected EvoLeafRestriction(clsLeaf leaf, string message)
				: base(leaf)
			{
				_message = message;
			}

			public override string message()
			{
				return string.Format("{0} {1}", ((clsLeaf) subject).Name, _message);
			}

			public override string uniq()
			{
				clsLeaf leaf = (clsLeaf) subject;
				return base.uniq() + _message;
			}

			public static IEnumerable<Invalidance> test(clsLeaf leaf)
			{
				List<Invalidance> list = new List<Invalidance>(0);
				// пока только только прямоугольные конструкции
				foreach (clsBeem beem in leaf)
				{
					if (beem.R1 > 0 || beem.PositionBeem == ItemSide.Other)
						list.Add(new EvoLeafRestriction(leaf, "Допустимы только прямоугольные створки"));
				}

				/// todo там вообще весьма разноцветные картинки
				/// еще и в зависимости от армирования 
				/// и еще там есть косой угол на который просто забили, хотя можна и сделать

				/// или можно еще просчитать модеь и бегать по ней, вермя однако жалка
				/// int wf = (int)(leaf.Width - 2 * leaf.HingBeem.Profile.RefE); // ширина - 2 фальца
				/// int hf = (int)(leaf.Height - 2 * leaf.HingBeem.Profile.RefE); // высота - 2 фальца

				int minh;
				int maxh;
				int minw;
				int maxw;

				switch (leaf.OpenType)
				{
					case OpenType.Откидное:
					case OpenType.Подвесное:
						if (isZ64(leaf))
						{
							minw = minHeightZ64;
							maxw = maxHeightZ64;
							minh = minWidthZ64;
							maxh = maxWidthZ64;
						}
						else
						{
							minw = minHeight;
							maxw = maxHeight;
							minh = minWidth;
							maxh = maxWidth;
						}

						break;
					case OpenType.Поворотное:
					case OpenType.Поворотно_откидное:
					default:
						if (isZ64(leaf))
						{
							minh = minHeightZ64;
							maxh = maxHeightZ64;
							minw = minWidthZ64;
							maxw = maxWidthZ64;
						}
						else
						{
							minh = minHeight;
							maxh = maxHeight;
							minw = minWidth;
							maxw = maxWidth;
						}

						break;
				}

				// ширина
				if ((leaf.Width < minw || leaf.Width > maxw))
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("ширина створки {0:#} за пределами допустимого от {1} до {2}", leaf.Width, minw, maxw)));

				// высота
				if ((leaf.Height < minh || leaf.Height > maxh))
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("высота створки {0:#} за пределами допустимого от {1} до {2}", leaf.Height, minh, maxh)));

				/// длинна запирающей балки, 
				/// зависит от возможностей фуринтуры рото DM8, 
				/// место этому коду конечно в ограничениях на рото, 
				/// но стоит тут т.к. пока DM8 используется только в EVO
				const int minHandleF = 800;
				const int maxHandleF = 2000;
				int lenHandleFalce = (int) leaf.HandleBeem.LineE.Length;
				if (isZ64(leaf) && (lenHandleFalce < minHandleF || lenHandleFalce > maxHandleF))
					if (getRestriction(leaf.Model)) /// УБРАТЬ НАХ!!!!!
						list.Add(new EvoLeafRestriction(leaf, string.Format("длинна балки c ручкой по фальцу {0:#} за пределами допустимого от {1} до {2}", lenHandleFalce, minHandleF, maxHandleF)));

				/// Evo c L64 створкой - только центральная ручка, ибо нет константных запоров DM8 // bug ГДЕ НЕТ ? это не вопрос профиля а фурнитуры и не EVO а DM=8
				if (isZ64(leaf) && leaf.HandlePositionType != HandlePositionType.Центральное)
					list.Add(new EvoLeafRestriction(leaf, string.Format("для профиля {0} возможно только центральное положение ручки", leaf.HandleBeem.Profile.Marking)));

				if (isZ64(leaf) && leaf.ShtulpOpenType != ShtulpOpenType.NoShtulp && leaf.Model.FurnitureSystem.Name != RotoNT && leaf.Model.FurnitureSystem.Name != RotoNTDesigno)
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("створки {0} не строятся в штульповом исполнении, используйте створку Z57", leaf.HandleBeem.Profile.Marking)));

				return list;
			}
		}

		// контроль Z | T  <=> сторона открывания
		private class TZInvalidance : Invalidance
		{
			public TZInvalidance(object subj) : base(subj)
			{
			}

			public override string message()
			{
				clsLeaf leaf = (clsLeaf) subject;
				return string.Format("Створка {0} профиль {1} {2} не соответствует открыванию {3}", leaf.Name, leaf.HingBeem.Profile.Marking, leaf.HingBeem.Profile.Comment, leaf.OpenView);
			}

			public static TZInvalidance test(clsLeaf leaf)
			{
				// 2015-04-21 порталы не так просты как кажется // TODO something
				if (!getRestriction(leaf.Model))
					return null;

				switch (leaf.OpenType)
				{
					case OpenType.Поворотно_откидное:
					case OpenType.Поворотное:
					case OpenType.Откидное:
					case OpenType.Подвесное:
						if (leaf.HingBeem.Profile.Comment != null && ((leaf.HingBeem.Profile.Comment.ToUpper().Contains(Z) && leaf.OpenView != OpenView.Внутрь) || (leaf.HingBeem.Profile.Comment.ToUpper().Contains(T) && leaf.OpenView != OpenView.Наружу)))
							return new TZInvalidance(leaf);

						break;
				}

				return null;
			}
		}

		// ограничения по системам в Окнах ДА
		private class OknaDaSystemRestriction : Invalidance
		{
			static readonly List<string> forbiden = new List<string>(new string[] {Solar, Vario, Thermo70, EVO, RehauEuro70, ThermoLock, Classic});

			public OknaDaSystemRestriction(object subj) : base(subj)
			{
			}

			public override string message()
			{
				return string.Format("Профильная система {0} недоступна в данном типе заказа", ((clsModel) subject).ProfileSystem.Name);
			}

			public static OknaDaSystemRestriction test(clsModel model)
			{
				try
				{
					// почему-то при пересчете в заказе это поле не выставлено идем другим путем
					OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];

					if (order != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 67 && forbiden.Contains(model.ProfileSystem.Name))
						return new OknaDaSystemRestriction(model);
				}
				catch (Exception)
				{
				}

				return null;
			}
		}

		/// <summary>
		/// ограничения заполнений с танцульками и преферансом
		/// </summary>
		public class FillRestriction : Invalidance
		{
			private readonly string _message;

			public FillRestriction(clsRegion subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			public static IEnumerable<Invalidance> test(clsRegion region)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				clsModel model = region.Model;

				bool restriction = getRestriction(model);

				#region эти типы конструкций бывают толькот только Без_стекла_и_штапика

				switch (model.ConstructionType.Name)
				{
					case _facade:
					case _manual:
					case _gate:
					case _wicket:
					case _moskit:
						clsFill fill = model.ProfileSystem.Fills["Без_стекла_и_штапика"];
						if (region.Fill != fill)
							list.Add(new SimpleInvariant(model, string.Format("только заполнение {0}", fill), delegate { region.Fill = fill; }));
						break;
				}

				#endregion

				#region доступность заполнений по системам todo как-то более цивильно и массово

				if (restriction)
				{
					if (model.ProfileSystem.Name == Thermo70 || model.ProfileSystem.Name == Thermo76 || model.ProfileSystem.Name == Estet)
					{
						const int thikness = 32;
						if (region.Fill.Thikness != thikness && region.Fill.Thikness != 0)
							list.Add(new FillRestriction(region, string.Format("в {0} можно использовать только заполнения {1}мм", model.ProfileSystem.Name, thikness)));

						if (region.Fill.FillType == FillType.GlassPack)
						{
							string marking = region.Fill.Marking.ToLower();
							if (!marking.Contains("и") && !marking.Contains("cos") && !marking.Contains("stneo") && !marking.Contains("4(sg rb)") && !marking.Contains("4(sg s)") && !marking.Contains("4х10х4х10х4") && !marking.Contains("mf") && !marking.Contains("4ultra") && !marking.Contains("cgs"))
							{
								list.Add(new FillRestriction(region, string.Format("в {0} можно использовать только заполнения c энергоэффективным стеклом (И4, MF, ULTRA, etc.)", model.ProfileSystem.Name)));
							}
						}
					}
					else if (model.ProfileSystem.Name == NEO_80)
					{
						const int thikness = 42;
						const string defaultGlass = "4x16x4x14xИ4"; // todo seek through glasses by thikness and order by numpos
						if (region.Fill.Thikness != thikness && region.Fill.Thikness != 0)
							list.Add(new SimpleInvariant(model, string.Format("в {0} можно использовать только заполнения {1}мм", model.ProfileSystem.Name, thikness), delegate { region.Fill = model.ProfileSystem.Fills[defaultGlass]; }));

						const int camernost = 2;
						if (region.Fill.FillType == FillType.GlassPack && region.Fill.Camernost < camernost)
							list.Add(new SimpleInvariant(model, string.Format("в {0} можно использовать только двухкамерные заполнения", model.ProfileSystem.Name, camernost), delegate { region.Fill = model.ProfileSystem.Fills[defaultGlass]; }));
					}
				}

				#endregion

				#region прочие мелкие ограничения

				// метелюкс лист не более 2550 x 1605
				// метелюкс, диам, кризет лист не более 2100 x 1600 // @ 27-06-2016 малеванный 
				// *диаман *кризет
				if (region.Fill.Marking.ToLower().Contains("мателюкс") || region.Fill.Marking.ToLower().Contains("диаман") || region.Fill.Marking.ToLower().Contains("di") || region.Fill.Marking.ToLower().Contains("кризет"))
				{
					double h, w;
					// поставить вверх
					if (region.RegionHeight >= region.RegionWidth)
					{
						h = region.RegionHeight;
						w = region.RegionWidth;
					}
					else
					{
						w = region.RegionHeight;
						h = region.RegionWidth;
					}

					const int maxh = 2100;
					const int maxw = 1600;

					if (h > maxh || w > maxw)
					{
						if (restriction)
							list.Add(new FillRestriction(region, string.Format("Заполнение {0} макс размер {1} x {2}", region.Fill.Marking, maxh, maxw)));
					}
				}

					// стеклопакеты с триплексом максимальный размер листа 2250 х 3210
				else if (region.Fill.Marking.ToLower().Contains("+"))
				{
					double h, w;
					// поставить вверх
					if (region.RegionHeight >= region.RegionWidth)
					{
						h = region.RegionHeight;
						w = region.RegionWidth;
					}
					else
					{
						w = region.RegionHeight;
						h = region.RegionWidth;
					}

					const int maxh = 3210;
					const int maxw = 2250;

					if (h > maxh || w > maxw)
					{
						list.Add(new FillRestriction(region, string.Format("Заполнение {0} макс размер {1} x {2}", region.Fill.Marking, maxh, maxw)));
					}
				}

				if (region.Fill.FillType == FillType.Sandwich)
				{
					// все
					if (region.RegionHeight > 3000 || region.RegionWidth > 3000 || region.RegionHeight > 1500 && region.RegionWidth > 1500)
					{
						list.Add(new FillRestriction(region, "Размеры сэндвич-панели превышают максимально допустимые 3000x1500"));
					}

					// ламинация
					if ((ColorRestriction.isLam(model.ColorInside) && isWidthBig(region, model.ColorInside)) || ColorRestriction.isLam(model.ColorOutside) && isWidthBig(region, model.ColorOutside))
					{
						if (restriction)
							list.Add(new FillRestriction(region, "Ширина ламиннированной сэндвич-панели превышают максимально допустимое значение зависящее от цвета: 1320/660 для Hornschuch, 1150/650 для Renolit"));
					}
				}

				if (region.Fill.Marking.Contains("3+3(StNeo)") /*|| region.Fill.Marking.ToLower().Contains("криз")*/)
				{
					if (restriction)
						list.Add(new FillRestriction(region, string.Format("Заполнение {0} не доступно", region.Fill.Marking)));
				}

				if (Settings.isDealer && model.ConstructionType.Name != _moskit && model.ConstructionType.Name != _nalichnik)
				{
					if (region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown)
						list.Add(new FillRestriction(region, string.Format("Заполение {0} недоступно", region.Fill.Name)));
				}

				// пложенио чтоб _стекло_ было выпуклое
				if (region.Fill.FillType == FillType.GlassPack || region.Fill.FillType == FillType.PuzzleFill)
				{
					colLine lines = region.Lines[0];
					for (int i = 0; i < lines.Count; i++)
					{
						// ошибки округления бздят про корректные конструкции, лечение округлить до целого
						int angle = (int) Math.Round(lines[i].AngelBeetwenLines360(lines[(i + 1) % lines.Count]));
						// 05-03-2018 отрицательный радиус можно
						if (angle > 180 && restriction)
						{
							list.Add(new FillRestriction(region, string.Format("{0} ({1}) - невыпуклый многоугольник, невозможно изготовить стеклопакет такой формы!", region.Name, region.Fill.FillType.ToString())));
							break;
						}
					}
				}

				if (restriction && (region.Fill.FillType == FillType.GlassPack || region.Fill.FillType == FillType.PuzzleFill) && (region.RegionHeight / region.RegionWidth > 7 || region.RegionWidth / region.RegionHeight > 7))
				{
					list.Add(new FillRestriction(region, String.Format("{0} {1}x{2} соотношение сторон не более 7:1", region.Part, region.RegionWidth, region.RegionHeight)));
				}

				#endregion

				#region ограничения на площадь и размеры относительно маркировки

				if (restriction && region.Fill.Camernost > 0)
				{
					const string claim = ", обратитесь в тех. отдел для принятия решения о возможности изготовления";

					List<int> spanList = new List<int>();

					Qualifier[] qualifiers = Qualifier.of(region.Fill, ref spanList);

					if (spanList.Count > 1 && spanList[0] < spanList[1])
					{
						list.Add(new FillRestriction(region, string.Format("{0} первая (со стороны улицы) меж-стекольная камера должна быть больше, или равняться второй", region.Fill.Marking)));
					}

					if (qualifiers != null && qualifiers.Length > 0)
					{
						string[] qualifierss = new string[qualifiers.Length];
						for (int i = 0; i < qualifiers.Length; i++)
						{
							qualifierss[i] = qualifiers[i].ToString();
						}

						decimal max = 0;
						Qualifier q = null;
						foreach (Qualifier qualifier in qualifiers)
						{
							decimal m = Qualifier.getMax(qualifier);

							if (m == 0)
							{
								list.Add(new FillRestriction(region, string.Format("{1} не найдено значение площади для квалификатора {2}", region.Part, region.Fill.Marking, qualifier)));
							}

							if (m < max || max == 0)
							{
								max = m;
								q = qualifier;
							}
						}

						// мм2 - > m2
						decimal sqr = (decimal) (region.Lines[4].Sqr / 1000000);

						if (sqr > max)
							list.Add(new FillRestriction(region, string.Format("{1}, {0} площадь {2}м2 , (ограничение {3}м2 ({5}) для данной формулы стеклопакета ) {4}", region.Part, region.Fill.Marking, sqr, max, string.Join(", ", qualifierss), q != null ? q.ToString() : "#")));
					}
					else
					{
						list.Add(new FillRestriction(region, string.Format("СП {0} не смог быть квалифицирован {1}", region.Fill.Name, claim)));
					}
				}

				#endregion

				// ограничение радиусности триплекса которое раньше не проверялось ибо висело на событии построителя не проходимом при смене заполнения в окне заказа - кулибины...
				if (restriction && isTriplex(region.Fill.Marking) && isArc(region))
					list.Add(new FillRestriction(region, string.Format("{0} использование триплекса ({1}) в арочном заполнении запрещено", region.Part, region.Fill.Marking)));

				// теплая рамка только прямоугольная // 16-06-2014 не использовать TH и TP в арках
				if (restriction && isWarmFrame(region.Fill.Marking) && !region.IsSquare())
					list.Add(new FillRestriction(region, string.Format("{0} использование теплой рамки ({1}) в не прямоугольном заполнении запрещено", region.Part, region.Fill.Marking)));

				// http://yt:8000/issue/dev-246
				// Убрать возможность выбора в АТ менеджерами и дилерами формул с рамкой "Chromatech" кроме рамки 24CHR
				string troubleMakerSpan;
				if (restriction && isChromatech(region.Fill.Marking, out troubleMakerSpan))
					list.Add(new FillRestriction(region, string.Format("{0} использование рамки {1} запрещено, доступна только 24CHR", region.Part, troubleMakerSpan)));

				return list;
			}

			// дикость - жевать формулы на каждый чих
			private static bool isChromatech(string marking, out string troubleMakerSpan)
			{
				// заменить ха на икс
				const char ha = 'х';
				const char x = 'x';
				marking = marking.Replace(ha, x);

				string[] strings = marking.Split(x);
				for (int i = 0; i < strings.Length; i++)
				{
					string mark = strings[i];
					bool isGlass = i % 2 == 0;
					if (!isGlass)
					{
						const string chromatech = "CHR";
						const string approvedChromatech = "24CHR";
						if (mark.ToUpper().Contains(chromatech) && mark.ToUpper() != approvedChromatech)
						{
							troubleMakerSpan = mark;
							return true;
						}
					}
				}

				troubleMakerSpan = null;
				return false;
			}

			// todo перенести в более общее поле видимости
			private static bool isArc(clsRegion region)
			{
				foreach (clsLine line in region.FillConture)
					if (line.R1 > 0 || line.R2 > 0)
						return true;

				return false;
			}

			private static bool isTriplex(String marking)
			{
				// немного колхозно, правильно - разпарсить стекло и проверять стекло отдельно, рамки отдельно иначе будут  ложные срабатывания
				return marking.Contains("+") || marking.Contains(".4") || marking.Contains(".3");
			}

			private static bool isWarmFrame(String marking)
			{
				// немного колхозно, правильно - разпарсить стекло и проверять стекло отдельно, рамки отдельно иначе будут  ложные срабатывания
				return marking.Contains("TH") || marking.Contains("TP");
			}

			private static bool isWidthBig(clsRegion region, clsConstructionColor color)
			{
				//                int maxWidth = (color.ColorShortName != null && color.ColorShortName.StartsWith("F4")) ? (isStandart(color) ? 1320 : 660) : colors1150.Contains(color.ColorShortName) ? 1150 : 650;
				int maxWidth = (color.ColorShortName != null && color.ColorShortName.StartsWith("F4")) ? (isStandart(color) ? 1320 : 660) : (isStandart(color) ? 1150 : 650);
				return region.RegionWidth > maxWidth;
			}

			private static bool isStandart(clsConstructionColor color)
			{
				// 17	СТАНДАРТНЫЕ ЦВЕТА
				const int idcolorroupStandart = 17;
				return color.IDColorGroup == idcolorroupStandart;
			}

			// более широкие цвета 1150 мм, остатне по  650мм
			private static readonly List<string> colors1150 = new List<string>(new string[] {"2178001", "2065021", "2052089", "3202001", "3149008"});

			/// <summary>
			/// квалификатор связки стекло + рамки при нём => макс площадь
			/// </summary>
			public class Qualifier
			{
				// типаж стёкол, в рамках парсинга формулы будем пытацца прийти к оным параметрам
				public enum Glass
				{
					_4,
					_3_3,
					_4закал,
					_5,
					_5закал,
					_6,
					_4_4,
					_6закал,
					_8,
					_10
				}

				public readonly Glass glass;
				public readonly int span;

				/// <summary>
				/// понтять (разобрать) заполнение по полям квалификатора, по новой модели нелься сделать неперед однозначного соответстявия ибо 
				/// целевой индикатор площадь не имеет строго положительной первой производной, ситуацию осложняет ненормируемый параметр стекло
				/// поетому результатом будет сонм квалиферовв из которых квалификатор выберет минимальый который собстно можно тут и вернуить ибо мв можем их сравнивать по целевому параметру
				/// </summary>
				/// <param name="fill">заполение</param>
				/// <param name="spanList">список приёмник ширин рамок, колхозненько конечно</param>
				public static Qualifier[] of(clsFill fill, ref List<int> spanList)
				{
					string marking = fill.Marking;
					// заменить ха на икс
					const char ha = 'х';
					const char x = 'x';
					marking = marking.Replace(ha, x);

					List<Glass> glasses = new List<Glass>();
					List<int> spans = new List<int>();

					string[] strings = marking.Split(x);
					for (int i = 0; i < strings.Length; i++)
					{
						string mark = strings[i];
						string digits = firstDigits.Match(mark.Trim()).Groups[0].Value;
						bool isGlass = i % 2 == 0;
						if (isGlass)
						{
							// (по колхозному) 
							// приводим строку к данной ограниченному множеству (enum) стёкол ибо 3+3 это и не 4 и не 5 и не 6 а именно 3+3.
							// при ненахождении поставит 4

							// nullity
							if (string.IsNullOrEmpty(mark))
							{
								glasses.Add(Glass._4);
							}
								// сначала вырезаем триплексы которые _нельзя_ понимать по их толшине, 
								// потом закал по толщине парсингом строкового представления очищенного от букв кроме + и закал БЛЪДЪ!,
								// потом прочее по толщине парсингом строкового представления, смысла int.parse нет ибо int ни к чему ни прикрутишь в enum
							else if (digits.Contains("3+3"))
								glasses.Add(Glass._3_3);
							else if (digits.Contains("4+4"))
								glasses.Add(Glass._4_4);
							else if (mark.Contains("закал"))
							{
								switch (digits)
								{
									case "4":
										glasses.Add(Glass._4закал);
										break;
									case "5":
										glasses.Add(Glass._5закал);
										break;
									case "6":
										glasses.Add(Glass._6закал);
										break;
									case "8":
										glasses.Add(Glass._8); // не предоставлено сведений для 8 закал
										break;
									case "10":
										glasses.Add(Glass._10);
										break;
									default:
										glasses.Add(Glass._4закал);
										break;
								}
							}
							else
							{
								switch (digits)
								{
									case "4":
										glasses.Add(Glass._4);
										break;
									case "5":
										glasses.Add(Glass._5);
										break;
									case "6":
										glasses.Add(Glass._6);
										break;
									case "8":
										glasses.Add(Glass._8);
										break;
									case "10":
										glasses.Add(Glass._10);
										break;
									default:
										glasses.Add(Glass._4); // DEFAULT
										break;
								}
							}
						}
						else
						{
							int span;
							if (int.TryParse(digits, out span))
							{
								spanList.Add(span);

								// приводим рамки к ряду значений: 6, 8, 12, что всё равно приводит к квалификаторам отсутствующим в таблице
								if (span >= 12)
									span = 12;
								else if (span >= 8)
									span = 8;
								else
									span = 6;

								spans.Add(span);
							}
						}
					}

					// осмысленный контроль нештатных ситуаций
					if (glasses.Count < 2 || glasses.Count > 3 || spans.Count < 1 || spans.Count > 2 || glasses.Count - 1 != spans.Count)
						throw new ArgumentException("некоректная формула СП: " + fill.Marking);

					// собираем сочетания, но будет не логично их тут сортировать ибо в них не будет целевого параметра - это работа другого метода
					Qualifier[] qualifers = new Qualifier[2 * spans.Count];

					for (int i = 0, j = 0; i < spans.Count; i++)
					{
						qualifers[j++] = new Qualifier(glasses[i], spans[i]);
						qualifers[j++] = new Qualifier(glasses[i + 1], spans[i]);
					}

					return qualifers;
				}

				// \d+\+*\d*\+*\d*
				private static readonly Regex firstDigits = new Regex(@"\d+\+?\d*");

				private Qualifier(Glass glass, int span)
				{
					this.glass = glass;
					this.span = span;
				}

				public override string ToString()
				{
					return string.Format("{0}|{1}", glass, span);
				}

				protected bool Equals(Qualifier other)
				{
					return glass == other.glass && span == other.span;
				}

				public override bool Equals(object obj)
				{
					if (ReferenceEquals(null, obj)) return false;
					if (ReferenceEquals(this, obj)) return true;
					if (obj.GetType() != this.GetType()) return false;
					return Equals((Qualifier) obj);
				}

				public override int GetHashCode()
				{
					unchecked
					{
						return ((int) glass * 397) ^ span;
					}
				}

				// таблица максимальных значений, 
				public static readonly Dictionary<Qualifier, decimal> max = new Dictionary<Qualifier, decimal>();

				// логично не хранить это именн здесь но так прощще держать все ф одной куче, ну да потенциалбно можно в настройки вывалить н пока правила меняются чаще чем их значения - не резон
				static Qualifier()
				{
					max.Add(new Qualifier(Glass._4, 06), 1.7m);
					max.Add(new Qualifier(Glass._4, 08), 2.5m);
					max.Add(new Qualifier(Glass._4, 12), 3.17m);

					max.Add(new Qualifier(Glass._3_3, 06), 1.7m);
					max.Add(new Qualifier(Glass._3_3, 08), 2.7m);
					max.Add(new Qualifier(Glass._3_3, 12), 3.5m);

					max.Add(new Qualifier(Glass._4закал, 06), 2.1m);
					max.Add(new Qualifier(Glass._4закал, 08), 2.9m);
					max.Add(new Qualifier(Glass._4закал, 12), 3.9m);

					max.Add(new Qualifier(Glass._5, 08), 3.8m);
					max.Add(new Qualifier(Glass._5, 12), 5.0m);

					max.Add(new Qualifier(Glass._5закал, 08), 4.0m);
					max.Add(new Qualifier(Glass._5закал, 12), 5.5m);

					max.Add(new Qualifier(Glass._6, 08), 4.6m);
					max.Add(new Qualifier(Glass._6, 12), 6.2m);

					max.Add(new Qualifier(Glass._4_4, 08), 4.6m);
					max.Add(new Qualifier(Glass._4_4, 12), 6.0m);

					max.Add(new Qualifier(Glass._6закал, 08), 5.2m);
					max.Add(new Qualifier(Glass._6закал, 12), 7.0m);

					max.Add(new Qualifier(Glass._8, 08), 8.0m);
					max.Add(new Qualifier(Glass._8, 12), 9.0m);

					max.Add(new Qualifier(Glass._10, 08), 8.0m);
					max.Add(new Qualifier(Glass._10, 12), 9.0m);
				}

				public static decimal getMax(Qualifier qualifier)
				{
					return max.ContainsKey(qualifier) ? max[qualifier] : 0;
				}
			}
		}

		private class WoodLineRestriction : Invalidance
		{
			// доступные системы
			static readonly List<String> approvedProfSysList = new List<string>(new string[] {Rehau70, Rehau60, DECOR, RehauDeluxe});

			// группа ламинации / бывает еще и окрашенность а не ламинация
			private const int approvedColorGroup = 1;

			private const string __woodline = "WoodLine";

			private readonly string _message;

			private WoodLineRestriction(object subj, string _m)
				: base(subj)
			{
				_message = _m;
			}

			public static WoodLineRestriction test(clsModel model)
			{
				try
				{
					clsUserParam up = model.UserParameters[__woodline];

					if (up != null && up.StringValue != "нет")
					{
						// требуем систему
						if (!approvedProfSysList.Contains(model.ProfileSystem.Name))
							return new WoodLineRestriction(model, string.Format("Опция WoodLine доступна только в системах {0}", string.Join(",", approvedProfSysList.ToArray())));

						// требуем ламинации снаружи
						if (model.ColorOutside.IDColorGroup != approvedColorGroup && model.ColorOutside.IDParentColorGroup != approvedColorGroup)
						{
							return new WoodLineRestriction(model, string.Format("Опция WoodLine доступна только для ламинированных снаружи конструкций"));
						}

						// требуем углы 90 на рамах
						foreach (clsBeem beem in model.Frame)
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
								return new WoodLineRestriction(model, string.Format("Опция WoodLine доступна только прямоугольных конструкций"));

						// и створках, импосты лесом
						foreach (clsLeaf leaf in model.Leafs)
							foreach (clsBeem beem in leaf)
								if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
									return new WoodLineRestriction(model, string.Format("Опция WoodLine доступна только прямоугольных конструкций"));

						// каждый периметр полпериметра >= 1050 && большая сторона >= 650
						if (!test1050(model.Frame.Width, model.Frame.Height))
							if (getRestriction(model))
								return new WoodLineRestriction(model, string.Format("Рама {0}x{1} не проходит ограничения WoodLine: половина париметра >1050, большая сторона >650, меньшая >400 ", model.Frame.Width, model.Frame.Height));

						foreach (clsLeaf leaf in model.Leafs)
							if (!test1050(leaf.Width, leaf.Height))
								if (getRestriction(model))
									return new WoodLineRestriction(model, string.Format("{2}  {0}x{1} не проходит ограничения WoodLine: половина париметра >1050, большая сторона >650, меньшая >400", leaf.Width, leaf.Height, leaf.Name));
					}
				}
				catch
				{
				}

				return null;
			}

			private static bool test1050(double w, double h)
			{
				if (w + h < 1050) return false;

				if (w >= h && (w < 650 || h < 400)) return false;

				if (h >= w && (h < 650 || w < 400)) return false;

				return true;
			}

			public override string uniq()
			{
				return base.uniq() + message();
			}

			public override string message()
			{
				return _message;
			}
		}

		// ограничения Roto OK
		private class RotoOkRestriction : Invalidance
		{
			private static readonly List<string> profSystemsToUse = new List<string>(new string[] {Rehau60, Pimapen, ProfSystBEZ, ThermoLock, RehauEuro70, Classic /*, ProfSystRehau70*/});

			private readonly string _message;

			protected RotoOkRestriction(clsLeaf leaf, string message) : base(leaf)
			{
				_message = message;
			}

			protected RotoOkRestriction(clsModel model, string message) : base(model)
			{
				_message = message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				return leaf != null ? string.Format("{0} {1}", leaf.Name, _message) : string.Format("{0}", _message);
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			private const string _changeParameterName = "Замена фурнитуры";

			// 20-02-2016  ++наноокна
			private static readonly List<int> lyapins = new List<int>(new int[] {291, 304, 319, 325, 360, 370, 371, 372, 498});

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// накопитель необходимости замены
				bool change = false;

				// ограничения
				bool restriction = RunCalc.getRestriction(model);

				// очистка перед всего ибо человеки меняю все что им угодно поменять
				// if (upChange != null)
				//	upChange.StringValue = string.Empty;

				// родная фурнитурная система
				if (model.FurnitureSystem.Name == RotoOK)
				{
					// родная профильная стситема
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new RotoOkRestriction(model, string.Format("{0} не доступна в профильной системе {1}", RotoOK, model.ProfileSystem.Name)));

					if (model.ProfileSystem.Name == RehauEuro70)
					{
						// Euro70 недоступна кроме Ляпина, даже на заводе, поэтому дилеристость не проверяем
						OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
						if (order != null && order.DocRow != null && !order.DocRow.IsidsellerNull() && lyapins.Contains(order.DocRow.idseller))
						{
						}
						else
						{
							if (Settings.idpeople != 255)
								list.Add(new RotoOkRestriction(model, string.Format("{0} не доступна в профильной системе {1}", RotoOK, model.ProfileSystem.Name)));
						}
					}

					// пробег по створкам
					foreach (clsLeaf leaf in model.Leafs)
					{
						// токо прямоуг = roto OK, если не канает тогда делаем ззамену на roto NT, но в модели написано что это Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								// list.Add(new RotoOkRestriction(leaf, string.Format("{0} только прямогуольные створки", FurnSystRotoOK)));
								// если не проканывает по геометрии то ставим флаг подмены фурнитуры, тогда дальше расчет пойдет по рельсам Roto NT ( скритпы
								change = true;
								break;
							}
						}

						// нельзя поставить на T створку вследствие толлько DM15
						if (leaf.OpenView == OpenView.Наружу)
							list.Add(new RotoOkRestriction(model, string.Format("{0} не ставиться на створки T", model.FurnitureSystem.Name)));

						/// todo тут как бы имеея на руках флаг смены системы по створке [leafChange] можно далее не проверять по ограничениям Roto OK
						/// leafChange снесен ! оставлен тока change на модеь в целом
						/// но в плане защиты от багов и чтоб не расслаблялись - пусть проверит и не даст большшего чем есть по Roto OK

						// размеры и проч
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						switch (leaf.OpenType)
						{
							case OpenType.Поворотное:
								switch (leaf.ShtulpOpenType)
								{
									// не штульповая
									case ShtulpOpenType.NoShtulp:
										if (w_F > 800)
										{
											if (w_F < 300 || w_F > 1300 || h_F < 533 || h_F > 2400)
												if (restriction)
													list.Add(new RotoOkRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
														w_F, h_F, 300, 1300, 533, 2400)));
										}
										else
										{
											if (w_F < 300 || w_F > 1300 || h_F < 300 || h_F > 2400)
												if (restriction)
													list.Add(new RotoOkRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
														w_F, h_F, 300, 1300, 300, 2400)));
										}

										break;

									// штульповая Активная
									case ShtulpOpenType.NoShtulpOnLeaf:
										list.Add(new RotoOkRestriction(leaf, "Активная штульповая створка должна быть поворотно-откидной"));
										break;

									// штульповая пассивная
									case ShtulpOpenType.ShtulpOnLeaf:
										if (w_F < 300 || w_F > 800 || h_F < 360 || h_F > 2400)
											list.Add(new RotoOkRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
												w_F, h_F, 300, 800, 360, 2400)));

										break;
								}

								break;

							case OpenType.Поворотно_откидное:
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
									list.Add(new RotoOkRestriction(leaf, "Пассивная штульповая створка должна быть поворотной"));

								if (w_F < 410 || w_F > 1300 || h_F < 360 || h_F > 2400)
									list.Add(new RotoOkRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотно-откидной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
										w_F, h_F, 410, 1300, 360, 2400)));

								break;

							case OpenType.Откидное:
								if (w_F < 300 || w_F > 1300 || h_F < 300 || h_F > 800)
									if (restriction)
										list.Add(new RotoOkRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры откидной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
											w_F, h_F, 410, 1300, 360, 2400)));


								break;
							default:
								list.Add(new RotoOkRestriction(leaf, string.Format("{0} тип открывания {1} не предусмотрен", RotoOK, leaf.OpenType)));
								break;
						}

						/// Параметры уровня створки

						// тольо центральное положение ручки, инвариантом не делать - может захотят поменять систему
						if (leaf.HandlePositionType != HandlePositionType.Центральное)
							list.Add(new RotoOkRestriction(leaf, "доступно только Центральное положение ручки"));

						// тольо оконная ручка в модели и в userParam // todo check after DOORS here
						clsUserParam ht = leaf.UserParameters.GetParam("Тип ручки");
						if (leaf.HandleType != HandleType.Оконная && leaf.HandleType != HandleType.Нет_ручки)
							list.Add(new RotoOkRestriction(leaf, "доступны только Оконные ручки"));

						if (ht != null && ht.StringValue != "Оконная ручка")
							list.Add(new RotoOkRestriction(leaf, "доступны только Оконные ручки"));

						// более 2 петель только на откидных и поворотных
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное)
							list.Add(new RotoOkRestriction(leaf, "Более 2 петель можно установить толька на Поворотные и Откидные створки"));
					}

					// остатки уровня модели

					// --WK1
					clsUserParam wk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
					if (wk1 != null && wk1.StringValue != "Основная")
						list.Add(new RotoOkRestriction(model, string.Format("{0}, доступна только Основная Комплектация фурнитуры", RotoOK)));

					// только белый цвет накладок
					clsUserParam dekas = model.UserParameters.GetParam("Накладки на петли");
					if (dekas != null && dekas.StringValue != "Белый" && dekas.StringValue != "Без накладок")
						list.Add(new RotoOkRestriction(model, string.Format("{0}, доступны только Белые Накладки на петли", RotoOK)));
				}

				/// TODO ГАРБЛЯ перенесено в BMC- отрихтовать 
				/// в построителе проверки ведуться ДО расчёта модели, посему изменения в clsModel канают,
				/// каким то ЛЯДОМ при незаходе в построитель я сделал валидацию  в AMC а не в BMC, не помню почему но так було надо зачемта, может ошибки штоб в Construction писать - 
				/// не поню, но так оно работает чудесненько пока Валидатор не пишет в clsModel, тут случается такой хм косячок что
				/// в AMC писать в clsModel уже бестоляк (до следущего пересчета или захода в построитель), 
				/// посему чтоб 1) флаг был данные были доступен посля AMC придётся писать в Construction тоже если он есть, ну или разрираться какого ЛЯДА валидатор делает в AMC
				/// 
				/// место хранения смены фурнитуры пишется вседга ибо могут поменять фурн. систему
				try
				{
					model.UserParameters[_changeParameterName].StringValue = change ? RotoNT : "нет";
					// todo too strange
					if (model.Construction != null)
					{
						UserParam upChangeInConstruction = model.Construction.GetUserParam(_changeParameterName);
						if (upChangeInConstruction != null)
							upChangeInConstruction.StrValue = change ? RotoNT : "нет";
					}
				}
				catch (Exception ex)
				{
					list.Add(new RotoOkRestriction(model, string.Format("отсутствует параметр {0} {1} {2}", _changeParameterName, ex.Message, ex.StackTrace)));
				}

				return list;
			}
		}

		private class OneImpostOneHinge : Invalidance
		{
			// мин половина ширины импоста от которой включается ограничение, на шировких умещается по две петли, на узкх нет, конкретное значение как обычно с потолка
			private const int minA = 59;

			private OneImpostOneHinge(clsImpost impost) : base(impost)
			{
			}

			public override string message()
			{
				return string.Format("{0} несёт более одной створки пересекающиеся при открывании, разнесите петлевые балки створок по разным балкам или по разным частям импоста или используйте более широкий профиль импоста", ((clsImpost) subject).Name);
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				if (model.ProfileSystem.Name != SystSlayding60)
				{
					foreach (clsImpost impost in model.Imposts)
					{
						if (impost.Profile.GetA() > 0 && impost.Profile.GetA() < minA && impost.ConnectedLeafBeem.Count > 1)
						{
							foreach (clsLeafBeem beem1 in impost.ConnectedLeafBeem)
							{
								if (beem1.IsHinge)
								{
									foreach (clsLeafBeem beem2 in impost.ConnectedLeafBeem)
									{
										if (beem2 != beem1 && beem2.IsHinge)
										{
											if (beem1.LGabarit.DistancePointToSegment(beem2.Point1) > 0 || beem1.LGabarit.DistancePointToSegment(beem2.Point2) > 0
												|| beem2.LGabarit.DistancePointToSegment(beem1.Point1) > 0 || beem2.LGabarit.DistancePointToSegment(beem1.Point2) > 0)
											{
												if (getRestriction(model))
													list.Add(new OneImpostOneHinge(impost));
											}
										}
									}
								}
							}
						}
					}
				}

				return list;
			}
		}

		// органичение по шпроссам в с/п с рамкой > 16мм и теплопакетах
		//    program, 24.11.2017 11:35:58:
		//    скорее всего для KPG = 12 , а для  SPGF  =16  ??
		//
		//    Леонтьев Григорий, 11:44:20:
		//    от 12
		//
		//    Леонтьев Григорий, 11:44:35:
		//    все влезет
		private class Shpros16Restriction : Invalidance
		{
			const char ha = 'х';
			const char x = 'x';
			private readonly string _message;
			private static readonly Regex _regex = new Regex(@"\D+");

			private const string TP = "TP";
			private const string TR = "ТР";

			private const int min = 10;


			public Shpros16Restriction(clsRegion subj, string message) : base(subj)
			{
				_message = message;
			}

			//			public override string uniq()
			//			{
			//				return base.uniq()+message();
			//			}

			public override string message()
			{
				return string.Format("Стеклопакет {0} {1}. {2}", ((clsRegion) subject).Part, ((clsRegion) subject).Fill, _message);
			}

			public static Invalidance test(clsRegion region)
			{
				try
				{
					// кладем на неограниченных
					if (!getRestriction(region.Model))
						return null;

					// кладем на стеклопакеты без камер
					if (region.SpreadingV2 != null && region.SpreadingV2.Type == SpreadingType.Shpross && region.Fill.Camernost > 0)
					{
						string[] parts = region.Fill.Marking.Replace(ha, x).Split(x);

						if (parts.Length >= 3)
						{
							// нас интересуют ПРОМЕЖУТКИ между стёклами
							// List<int> ramkaList = new List<int>();

							bool f = false;
							for (int i = 1; i < parts.Length; i++, i++)
							{
								// ширна рамки не более 16
								string r = _regex.Replace(parts[i], string.Empty);
								if (int.Parse(r) >= min)
									f = true;

								// в теплую рамку тоже не ставим TP или ТР русскими
								if (parts[i].Contains(TP) || parts[i].Contains(TR))
									return new Shpros16Restriction(region, parts[i]);
							}

							if (!f)
								return new Shpros16Restriction(region, string.Format("Невозможно изготовить пакет со шпроссом, используйте СП с рамкой не менее {0}мм", min));
						}
					}
				}
				catch (Exception ex)
				{
					return new InvalidanceException(ex, region);
				}

				return null;
			}
		}

		private class InvalidanceException : Invalidance
		{
			private readonly Exception ex;

			public InvalidanceException(Exception ex, object subj) : base(subj)
			{
				this.ex = ex;
			}

			public override string uniq()
			{
				return base.uniq() + message();
			}

			public override string message()
			{
				return string.Format("{0} {1} @ {2}", ex.Message, ex.StackTrace, subject);
			}
		}

		private class RehauZ60AntikInvariant : Invalidance
		{
			public const string __AntikMarking = "554026";
			private readonly string _message;

			public RehauZ60AntikInvariant(clsModel subj, string s) : base(subj)
			{
				_message = s;
			}

			public override string message()
			{
				return _message;
			}

			public static bool isAntik(clsModel model)
			{
				foreach (clsLeaf leaf in model.Leafs)
					if (leaf[0].Profile.Marking == __AntikMarking)
						return true;

				return false;
			}

			public static void fix(clsModel model)
			{
				// немного разные условия для систем
				if (model.ProfileSystem.Name == Rehau60)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf[0].Profile.Marking == __AntikMarking)
						{
							model.UserParameters["Цвет уплотнений"].StringValue = "Серый";

							int t = leaf.BaseRegion.Fill.Thikness;
							if (t >= 22 && t <= 25)
								model.UserParameters["Штапик"].StringValue = "Закруглённый";
							if (t >= 31 && t <= 33)
								model.UserParameters["Штапик"].StringValue = "Скошенный";

							return;
						}
					}
				}
				else if (model.ProfileSystem.Name == ThermoLock || model.ProfileSystem.Name == Classic)
				{
					bool antik = isAntik(model);

					/// если не нашлось створок с Антиком в этой конструкции, 
					/// и это чиста глухарь, то возможно они есть в соседнй 
					/// аесли не глухарь тада, только то что есть
					if (!antik && model.Leafs.Count == 0 && model.ModelConnections.Count > 0)
					{
						ModelConstruction masterConstruction = model.GetMasterConstruction();
						foreach (clsModel otherModel in masterConstruction.GetModelArray())
						{
							if (otherModel != model && isAntik(otherModel))
							{
								antik = true;
								break;
							}
						}
					}

					// без вариантов
					model.UserParameters["Цвет уплотнений"].StringValue = "Серый";

					// fix
					if (antik)
					{
						/// на 24 заполнениях дефолтово круглый 550090-615, но можно и фигурный 560607-715
						/// на 32 заполнениях только скошенный 560580-615
						/// BUG выставляемся по 0 заполнению ! а что будет если будут разные заполнения ?
						int t = model.VisibleRegions[0].Fill.Thikness;
						if (t >= 22 && t <= 25 && model.UserParameters["Штапик"].StringValue != "Закруглённый" && model.UserParameters["Штапик"].StringValue != "Фигурный")
							model.UserParameters["Штапик"].StringValue = "Закруглённый";
						if (t >= 31 && t <= 33)
							model.UserParameters["Штапик"].StringValue = "Скошенный";
					}
					else
					{
						/// на 24 заполнениях дефолтово фигурный 560607-715
						/// на 32 заполнениях только скошенный 560580-615
						// BUG выставляемся по 0 заполнению ! а что будет если будут разные заполнения ?
						int t = model.VisibleRegions[0].Thickness;
						if (t >= 22 && t <= 25)
							model.UserParameters["Штапик"].StringValue = "Фигурный";
						if (t >= 31 && t <= 33)
							model.UserParameters["Штапик"].StringValue = "Скошенный";
					}
				}
			}
		}

		private class DecorInvariant : SilentInvariant
		{
			private readonly string _value;

			public DecorInvariant(object subj, string value) : base(subj)
			{
				_value = value;
			}

			public override string message()
			{
				clsUserParam up = subject as clsUserParam;
				// todo
				return up != null ? string.Format("пареметр {1} используется только {2}", DECOR, up.Name, _value) : this.ToString(); // todo more compact
			}

			public override void fix()
			{
				// фиксим 
				clsUserParam up = subject as clsUserParam;
				if (up != null)
				{
					up.StringValue = _value;
				}
			}
		}

		private class DecorRestriction : Invalidance
		{
			private readonly string _message;

			private DecorRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsRegion region = subject as clsRegion;
				return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// свой профиль
				if (model.ProfileSystem.Name != DECOR && model.ProfileSystem.Name != RehauDeluxe)
					return list;

				// фурнитура === RotoNT | Дверная
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon)
					{
						if (model.FurnitureSystem.Name != RotoNT && model.FurnitureSystem.Name != SiegeniaClassic && model.FurnitureSystem.Name != SiegeniaTitan && model.FurnitureSystem.Name != SiegeniaTitanWK1 && model.FurnitureSystem.Name != SiegeniaAxxent)
							list.Add(new DecorRestriction(model, string.Format("В профсистеме {0} используется только фурнитура {1}", model.ProfileSystem.Name, SiegeniaClassic)));
					}
					else if (model.ConstructionType.Name == _pskportal)
					{
					}
					else
					{
						if (model.FurnitureSystem.Name != FurnSystDver)
							list.Add(new DecorRestriction(model, string.Format("Для дверей используется только {0}", FurnSystDver)));
					}
				}

				List<int> thicknesses = new List<int>();
				/// + собираем штапики
				/// толщина заполнения === 24 | 32
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
					{
						list.Add(new DecorRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения 24мм или 32мм", model.ProfileSystem.Name)));
					}
					else
					{
						if (!thicknesses.Contains(region.Fill.Thikness))
							thicknesses.Add(region.Fill.Thikness);
					}
				}

				// ставим на всю модель по первому которое попалось, скрипты все равно должны считать по конкретному заполнению
				if (thicknesses.Count > 0)
				{
					if (thicknesses.Count > 1)
					{
						// todo как то сообшщить что разные толщины заполнений
						WinDrawMessage message = new WinDrawMessage();
						message.CanClose = true;
						message.ID = typeof(DecorRestriction).Name;
						message.Message = string.Format("            В изделии используются заполнения разных толщин, параметры будут выставленны по заполнению {0}", thicknesses[0]);
						message.Model = model;
						model.WinDraw.AddMessage(message);
					}

					clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
					clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");
					// !! по первому попавшемуся, вариантов нет
					switch (thicknesses[0])
					{
						case 24:
							if (upShtap != null && upShtap.StringValue != "Закруглённый")
								list.Add(new DecorInvariant(upShtap, "Закруглённый"));

							if (upRubberColor != null && upRubberColor.StringValue != "Черный")
								list.Add(new DecorInvariant(upRubberColor, "Черный"));

							break;
						case 32:
							if (upShtap != null && upShtap.StringValue != "Скошенный")
								list.Add(new DecorInvariant(upShtap, "Скошенный"));

							if (upRubberColor != null && upRubberColor.StringValue != "Черный")
								list.Add(new DecorInvariant(upRubberColor, "Черный"));

							break;
					}
				}

				return list;
			}
		}

		private class ClassicRestriction : Invalidance
		{
			private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, SiegeniaAxxent});

			private readonly string _message;

			private ClassicRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsRegion region = subject as clsRegion;
				return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// свой профиль
				if (model.ProfileSystem.Name != Classic)
					return list;

				bool restriction = getRestriction(model);
				// фурнитура === RotoNT | Дверная
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new ClassicRestriction(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				List<int> thicknesses = new List<int>();

				/// + собираем штапики
				/// толщина заполнения === 24 | 32
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
					{
						list.Add(new ClassicRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения 24мм или 32мм", model.ProfileSystem.Name)));
					}
					else
					{
						if (!thicknesses.Contains(region.Fill.Thikness))
							thicknesses.Add(region.Fill.Thikness);
					}
				}

				// ставим на всю модель по первому которое попалось, скрипты все равно должны считать по конкретному заполнению
				if (thicknesses.Count > 0)
				{
					if (thicknesses.Count > 1)
					{
						// todo как то сообшщить что разные толщины заполнений
						WinDrawMessage message = new WinDrawMessage();
						message.CanClose = true;
						message.ID = typeof(DecorRestriction).Name;
						message.Message = string.Format("            В изделии используются заполнения разных толщин, параметры будут выставленны по заполнению {0}", thicknesses[0]);
						message.Model = model;
						model.WinDraw.AddMessage(message);
					}

					clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
					clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");
					// !! по первому попавшемуся, вариантов нет
					switch (thicknesses[0])
					{
						case 24:
							if (upShtap != null && upShtap.StringValue != "Закруглённый")
								list.Add(new DecorInvariant(upShtap, "Закруглённый"));

							if (upRubberColor != null && upRubberColor.StringValue != "Серый")
								list.Add(new DecorInvariant(upRubberColor, "Серый"));

							break;
						case 32:
							if (upShtap != null && upShtap.StringValue != "Скошенный")
								list.Add(new DecorInvariant(upShtap, "Скошенный"));

							if (upRubberColor != null && upRubberColor.StringValue != "Серый")
								list.Add(new DecorInvariant(upRubberColor, "Серый"));

							break;
					}
				}

				return list;
			}
		}

		private class Thermo76Restriction : Invalidance
		{
			private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, RotoNT, RotoNTDesigno, SiegeniaAxxent});

			private readonly string _message;

			private Thermo76Restriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			// todo global
			private const string Серый = "Серый";
			private const string Фигурный = "Фигурный";
			private const string ОбратныйРадиус = "Обратный радиус";

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// свой профиль
				if (model.ProfileSystem.Name != Thermo76 && model.ProfileSystem.Name != Estet)
					return list;

				bool restriction = getRestriction(model);

				// фурнитура 
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new Thermo76Restriction(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				// толщина заполнения === 32 проверяется в FillResstriction

				// уплотнение === серый, штапик === фигурный
				clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
				clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");

				if (upRubberColor != null && upRubberColor.StringValue != Серый)
				{
					list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1} цвет уплотнений", model.ProfileSystem.Name, Серый), delegate { upRubberColor.StringValue = Серый; }));
				}

				string rightShtapic = model.ProfileSystem.Name == Estet ? ОбратныйРадиус : Фигурный;
				if (upShtap != null && upShtap.StringValue != rightShtapic)
				{
					list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1} штапик", model.ProfileSystem.Name, rightShtapic), delegate { upShtap.StringValue = rightShtapic; }));
				}

				return list;
			}
		}

		private class Neo80Restriction : Invalidance
		{
			private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, RotoNT, RotoNTDesigno, SiegeniaAxxent});

			private readonly string _message;

			private Neo80Restriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			// todo global
			private const string Серый = "Серый";
			private const string Фигурный = "Фигурный";

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// свой профиль
				if (model.ProfileSystem.Name != NEO_80)
					return list;

				bool restriction = getRestriction(model);

				// фурнитура 
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new Neo80Restriction(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				// толщина заполнения === 32 проверяется в FillResstriction

				// уплотнение === серый, штапик === фигурный
				clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
				clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");

				if (upRubberColor != null && upRubberColor.StringValue != Серый)
				{
					list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1} цвет уплотнений", model.ProfileSystem.Name, Серый), delegate { upRubberColor.StringValue = Серый; }));
				}

				if (upShtap != null && upShtap.StringValue != Фигурный)
				{
					list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1} штапик", model.ProfileSystem.Name, Фигурный), delegate { upShtap.StringValue = Фигурный; }));
				}

				return list;
			}
		}

		private class AlutechProfileRestriction : Invalidance
		{
			protected readonly string _message;

			public override string message()
			{
				if (subject is clsLeaf)
					return string.Format("{0} {1}", ((clsLeaf) subject).Name, _message);
				else
					return string.Format("{0}", _message);
			}

			public AlutechProfileRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string uniq()
			{
				return message();
			}

			private static readonly List<string> skewCornerProfileList = new List<string>(new string[]
				{
				"AYPC.W62.0101", "AYPC.W62.0102", "AYPC.W62.0201", "AYPC.W62.0202", "AYPC.W62.0301", "AYPC.W62.0302",
				"AYPC.C48.0101", "AYPC.C48.0102", "AYPC.C48.0201"
				});

			public static string getPureMarking(clsBeem beem)
			{
				return beem.Profile != null && beem.Profile.Marking != null ? beem.Profile.Marking.Split(' ')[0] : null;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				bool restriction = getRestriction(model);

				// огранич по маятниковым на конструкцию
				if (model.ConstructionType.Name == _swingdoor)
				{
					switch (model.Leafs.Count)
					{
						case 1:
							if (model.Frame.Width < 500 || model.Frame.Width > 970)
							{
								if (restriction)
									list.Add(new AlutechProfileRestriction(model, string.Format("ширина одностворчатой маятниковой двери {0}, что выходит за допустымые пределы от {1} до {2}",
										model.Frame.Width, 500, 970)));
							}

							break;

						case 2:
							if (model.Frame.Width < 1100 || model.Frame.Width > 1800)
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("ширина двустворчатой маятниковой двери {0}, что выходит за допустымые пределы от {1} до {2}",
									model.Frame.Width, 1100, 1800)));
							}

							break;

						default:
							list.Add(new AlutechProfileRestriction(model, "маятниковая дверь может иметь только 1 или 2 створки"));
							return list;
					}

					if (model.Frame.Height < 1600 || model.Frame.Height > 2350)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("высота маятниковой двери {0}, что выходит за допустымые пределы от {1} до {2}",
							model.Frame.Height, 1600, 2350)));
					}
				}


				/// ~ todo уже НЕ только прямоугольные конструкции, косого креплеия импостов я еще не видел, только угловые на створках
				/// ограничения по соединениям балок // 30-150 градусов

				// рама
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("Профиль {0} не гнётся", beem.Profile.Marking)));
						continue;
					}

					if (beem.PositionBeem == ItemSide.Other)
					{
						if (skewCornerProfileList.Contains(getPureMarking(beem)))
						{
							decimal a = beem.Cutting_Angle2;
							decimal b = beem.Cutting_Angle1;
							switch (beem.Connect1)
							{
								case ConnectType.Равное:
									a *= 2;
									if (a < 32 || a > 150)
										list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2:F2}°, что выходит за допустымые пределы 32° - 150°", beem.Name, beem.Beem1.Name, a)));
									break;

								case ConnectType.Длинное:
								case ConnectType.Короткое:
									if (a < 40 || a > 140)
										list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2:F2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem1.Name, a)));

									break;

								default:
									list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect1)));
									break;
							}

							switch (beem.Connect2)
							{
								case ConnectType.Равное:
									b *= 2;
									if (b < 32 || b > 150)
										list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2:F2}°, что выходит за допустымые пределы 32° - 150°", beem.Name, beem.Beem2.Name, b)));
									break;

								case ConnectType.Длинное:
								case ConnectType.Короткое:
									if (b < 40 || b > 140)
										list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2:F2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem2.Name, b)));

									break;

								default:
									list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect2)));
									break;
							}
						}
						else
						{
							list.Add(new AlutechProfileRestriction(model, string.Format("Балка рамы из профиля {0} может использоваться только под прямым углом, для наклонных балок используйте оконный профиль серий  xx.xx01, xx.xx02", beem.Profile.Marking)));
						}
					}
				}

				// створка
				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0 || beem.R2 > 0)
						{
							list.Add(new AlutechProfileRestriction(model, string.Format("Профиль {0} не гнётся", beem.Profile.Marking)));
							continue;
						}

						if (beem.PositionBeem == ItemSide.Other)
						{
							if (skewCornerProfileList.Contains(getPureMarking(beem)))
							{
								int a = (int) Math.Round(beem.Cutting_Angle2);
								int b = (int) Math.Round(beem.Cutting_Angle1);

								switch (beem.Connect1)
								{
									case ConnectType.Равное:
										a *= 2;
										if (a < 30 || a > 150)
											list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 30° - 150°", beem.Name, beem.Beem1.Name, a)));
										break;

									case ConnectType.Длинное:
									case ConnectType.Короткое:
										if (a < 40 || a > 140)
											list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem1.Name, a)));

										break;

									default:
										list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect1)));
										break;
								}

								switch (beem.Connect2)
								{
									case ConnectType.Равное:
										b *= 2;
										if (b < 30 || b > 150)
											list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 30° - 150°", beem.Name, beem.Beem2.Name, b)));
										break;

									case ConnectType.Длинное:
									case ConnectType.Короткое:
										if (b < 40 || b > 140)
											list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem2.Name, b)));

										break;

									default:
										list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect2)));
										break;
								}
							}
							else
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("Балка створки из профиля {0} может использоваться только под прямым углом, для наклонных балок используйте оконный профиль серий  xx.xx01, xx.xx02", beem.Profile.Marking)));
							}
						}
					}

					/// отдельно ограничений DoorRestriction = нет :( поэтому пока тут
					/// W62 и С48 когда выбран замок многозапорный KFV0370.12 == многозапорный нажимной гарнитур
					/// 1) Для  двустворчатых  - минимальная высота створки = 1978мм; 
					/// 2) Для одностворчатых  - минимальная высота створки = 1900мм 
					clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
					if (up != null && (up.StringValue == __keykey || up.StringValue == __keybar))
					{
						const int minHeight = 1970;
						switch (leaf.ShtulpOpenType)
						{
							case ShtulpOpenType.NoShtulp:
								if (leaf.Height < minHeight)
								{
									if (restriction)
										list.Add(new AlutechProfileRestriction(leaf, string.Format("Минимальная высота створки для механизма {0} = {1}",
											up.StringValue, minHeight)));
								}

								break;

							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.Height < minHeight) // 2030
								{
									if (restriction)
										list.Add(new AlutechProfileRestriction(leaf, string.Format("Минимальная высота штульповой створки для механизма {0} = {1}",
											up.StringValue, minHeight))); // 2030
								}

								break;
						}
					}
				}

				// импост
				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("Профиль {0} не гнётся", beem.Profile.Marking)));
						continue;
					}

					if (beem.PositionBeem == ItemSide.Other)
					{
						if (true)
						{
							int a = (int) Math.Round(beem.Cutting_Angle1);
							int b = (int) Math.Round(beem.Cutting_Angle2);
							if (beem.Connect1 == ConnectType.ImpostShort)
							{
								if (a < 40 || a > 140)
									list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem1.Name, a)));
							}
							else
							{
								list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect1)));
							}

							if (beem.Connect2 == ConnectType.ImpostShort)
							{
								if (b < 40 || b > 140)
									list.Add(new AlutechProfileRestriction(beem, string.Format("между балками {0} и {1} угол {2}°, что выходит за допустымые пределы 40° - 140°", beem.Name, beem.Beem2.Name, b)));
							}
							else
							{
								list.Add(new AlutechProfileRestriction(beem, string.Format("соединение {0} не предусмотренно", beem.Connect2)));
							}
						}
						else
						{
							list.Add(new AlutechProfileRestriction(model, "Испост можно установить только под прямым углом"));
						}
					}
				}


				// порог тока снизу
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.bType == ComponentType.Porog && (beem.PositionBeem != ItemSide.Bottom))
					{
						list.Add(new AlutechProfileRestriction(model, "Порог только внизу"));
						break;
					}
				}

				// TODO надо как-то ограничить размеры конструкций
				// ограничения на длинну балки
				// if(model.Frame.Height > 3000 || model.Frame.Width > 6000)

				const int max = 4000;

				int min = isImpostLess(model) ? 200 : 250;

				foreach (clsBeem beem in model.Frame)
				{
					int m = beem.PositionBeem == ItemSide.Other || beem.Beem1.PositionBeem == ItemSide.Other || beem.Beem2.PositionBeem == ItemSide.Other ? 250 : min;

					if (beem.Lenght < m || beem.Lenght > max)
						if (restriction)
							list.Add(new AlutechProfileRestriction(model, string.Format("Длина балки рамы {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, m, max)));
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					min = isImpostLess(leaf) ? 200 : 250;

					foreach (clsBeem beem in leaf)
					{
						int m = beem.PositionBeem == ItemSide.Other || beem.Beem1.PositionBeem == ItemSide.Other || beem.Beem2.PositionBeem == ItemSide.Other ? 250 : min;

						if (beem.Lenght < m || beem.Lenght > max)
							list.Add(new AlutechProfileRestriction(model, string.Format("Длина балки створки {3} {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, m, max, leaf)));
					}
				}

				// .Lenght - длина импоста по осям, .Inside_Lenght - видимая часть внутри рамы, beem.LineC1.Length - по резке
				foreach (clsBeem beem in model.Imposts)
					if (beem.LineC1.Length < 200 || beem.LineC2.Length < 200 || beem.Lenght > max)
						if (restriction)
							list.Add(new AlutechProfileRestriction(model, string.Format("Длина импоста {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, 200, max)));

				////


				// Фурнтиуры
				switch (model.ConstructionType.Name)
				{
					case _window:
					case _balcon:
						if (model.FurnitureSystem.Name != GEISSE && model.FurnitureSystem.Name != FurnSystBEZ)
						{
							clsSystemFurniture fs = model.WinDraw.FurnitureSystems.FromName(GEISSE);
							if (fs != null)
								list.Add(new FurnitureInvariant(model, fs));
							else
								list.Add(new AlutechProfileRestriction(model, string.Format("Окна и Балконные девери {0} используют только {1}", model.ProfileSystem.Name, GEISSE)));
						}

						break;
					case _indoor:
					case _outdoor:
					case _swingdoor:
						if (model.FurnitureSystem.Name != FurnSystDver && model.FurnitureSystem.Name != FurnSystBEZ)
						{
							clsSystemFurniture fs = model.WinDraw.FurnitureSystems.FromName(FurnSystDver);
							if (fs != null)
								list.Add(new FurnitureInvariant(model, fs));
							else
								list.Add(new AlutechProfileRestriction(model, string.Format("Девери {0} используют только {1}", model.ProfileSystem.Name, FurnSystDver)));
						}

						break;
				}


				/// максимальные размеры створки, минимальные ограничены имн. размерами балок - немного выше - 200 / 250 мм, и ограничениями фурнитуры
				/// макс высота створки окна 1950, двери 2400
				/// макс ширина створок окна 1000, двери 1100мм, 
				/// фрамуги 1000 x 1000
				foreach (clsLeaf leaf in model.Leafs)
				{
					int h = (int) Math.Round(leaf.Height);
					int w = (int) Math.Round(leaf.Width);
					switch (model.ConstructionType.Name)
					{
						case _window:
						case _balcon:
							switch (leaf.OpenType)
							{
								case OpenType.Поворотное:
								case OpenType.Поворотно_откидное:
									if (w > 1000 || h > 1950)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("размеры {0} x {1} мм, нарушены предельные размеры створки: ширина не более {2}, высота не более {3} мм",
												w, h, 1000, 1950)));
									break;

								case OpenType.Откидное:
								case OpenType.Подвесное:

									if (w > 1500 || h > 1000)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("размеры {0} x {1} мм, нарушены предельные размеры створки: ширина не более {2}, высота не более {3} мм",
												w, h, 1500, 1000)));
									break;

								default:
									list.Add(new AlutechProfileRestriction(leaf, string.Format("{0} тип открывания {1} не предусмотрен", model.ProfileSystem.Name, leaf.OpenType)));
									break;
							}

							break;

						case _indoor:
						case _outdoor:
						case _swingdoor:
							switch (leaf.OpenType)
							{
								case OpenType.Поворотное:
								case OpenType.Маятниковое:
									if (w > 1100 || h > 2400)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("размеры {0} x {1} мм, нарушены предельные размеры створки: ширина не более {2}, высота не более {3} мм",
												w, h, 1100, 2400)));
									break;
								default:
									list.Add(new AlutechProfileRestriction(leaf, string.Format("{0} тип открывания {1} не предусмотрен для конструкции типа {2}", model.ProfileSystem.Name, leaf.OpenType, model.ConstructionType.Name)));
									break;
							}

							break;
						default:
							list.Add(new AlutechProfileRestriction(model, string.Format("Тип конструкции {0} не предусмотрен в профильной системе {1}", model.ConstructionType.Name, model.ProfileSystem.Name)));
							break;
					}
				}

				// запрет внутренней штульповой двери с реальным штульпом
				if (model.Leafs.Count > 0 && model.Leafs[0].OpenView == OpenView.Внутрь && (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor))
				{
					foreach (clsBeem beem in model.Imposts)
					{
						clsImpost impost = beem as clsImpost;
						if (impost != null && impost.BalkaType == ModelPart.Shtulp && !impost.Profile.Marking.ToLower().Contains("без"))
							list.Add(new AlutechProfileRestriction(model, string.Format("{0} внутренние штульповые двери строятся только со штульпом `Без профиля`", ALUTECH62)));
					}
				}

				// в дверной раме 0104 / 0105 невозможно поставить импост, можно в 0103 + 0805 ,но это потом
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.BalkaType == ModelPart.Porog || unImpostableProfileList.Contains(beem.Profile.Marking))
					{
						foreach (clsBeem impost in beem.ConnectedImposts)
						{
							if (impost.BalkaType == ModelPart.Impost) // штульпы можно
								list.Add(new AlutechProfileRestriction(model, string.Format("{0} невозможно уставновить на профиль '{1} {2}'", impost.Name, beem.Profile.Marking, beem.Profile.Comment)));
						}
					}
				}

				// невозможно установить заполнение на порог и дверную створку 0104 / 0105 (кроме без стекла и без штапика)
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 0 && region._Frame != null)
					{
						foreach (clsLine line in region.FillConture)
						{
							clsBeem beem = line.Beem;

							if (beem != null && (beem.BalkaType == ModelPart.Porog || unFillableProfileList.Contains(beem.Profile.Marking)))
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("Невозможно уставновить заполнение {0} на профиль '{1} {2}' используйте другой тип конструкции, тип профиля рамы или заполение 'Без_стекла_и_штапика'", region.Part, beem.Profile.Marking, beem.Profile.Comment)));
								break;
							}
						}
					}
				}

				// ранние попытки совместимости профиля, не относится к TZ инвариантам
				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (leafPorogCompatiblityDictionary.ContainsKey(beem.Profile.Marking))
						{
							clsBeem adj = leaf.AdjacentBeem((clsLeafBeem) beem);

							if (adj != null)
							{
								if (adj.bType == ComponentType.Porog)
								{
									if (!leafPorogCompatiblityDictionary[beem.Profile.Marking].Contains(adj.Profile.Marking))
									{
										list.Add(new AlutechProfileRestriction(model, string.Format("{0} балка '{1} {2}' не совместима с порогом '{3} {4}'", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
								else if (adj.bType == ComponentType.Frame)
								{
									if (!leafFrameCompatiblityDictionary[beem.Profile.Marking].Contains(adj.Profile.Marking))
									{
										if (restriction)
											list.Add(new AlutechProfileRestriction(model, string.Format("{0} балка '{1} {2}' не совместима с рамой '{3} {4}', сориентируйте профиль рамы согласно открыванию створки", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
							}
						}
					}

					// отдельная проверка для порогов которые иногда пролетают мимо .AdjacentBeem(...) который выдает неверный результат для порога 0 толщины // привет КОлхозникам !!!
					if (leaf.IsFrameLeaf)
					{
						foreach (clsBeem adj in model.Frame)
						{
							if (adj.bType == ComponentType.Porog)
							{
								clsLeaf beems = leaf.ConnectedBeem(adj); // это какой-то изощреный мразм
								foreach (clsBeem beem in beems)
								{
									if (leafPorogCompatiblityDictionary.ContainsKey(beem.Profile.Marking) && !leafPorogCompatiblityDictionary[beem.Profile.Marking].Contains(adj.Profile.Marking))
									{
										if (restriction)
											list.Add(new AlutechProfileRestriction(model, string.Format("{0} балка '{1} {2}' не совместима с балкой рамы '{3} {4}'", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
							}
						}
					}

					// leaf.ConnectedBeem(model.Frame[0])
				}

				// водоотводящие только наружу и 48 и 62 // 09/07/2018  --водоотводящие только: 62 наружу , 48 вниз, ниче не спрашиваем - сразу фиксим
				clsUserParam upv = model.UserParameters.GetParam("Водоотводящие");
				if (upv != null && upv.StringValue != "Наружу")
				{
					upv.StringValue = "Наружу";
				}

				// нельзя ставить москитные сетки на двери
				if (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.Нет)
							list.Add(new AlutechMoskitoLessDoorInvariant(leaf));
					}
				}

				return list;
			}

			private static bool isImpostLess(clsLeaf leaf)
			{
				foreach (clsImpost impost in leaf.Imposts)
					return false;
				return true;
			}

			private static bool isImpostLess(clsModel model)
			{
				foreach (clsImpost impost in model.Imposts)
					if (impost.Leaf == null)
						return false;
				return true;
			}

			private static readonly List<string> unImpostableProfileList = new List<string>(new string[] {"AYPC.W62.0104", "AYPC.W62.0105", "AYPC.C48.0105", "AYPC.C48.0111"});
			private static readonly List<string> unFillableProfileList = new List<string>(new string[] {"AYPC.W62.0104", "AYPC.W62.0105", "AYPC.C48.0105", "AYPC.C48.0111"});
			private static readonly Dictionary<string, List<string>> leafPorogCompatiblityDictionary = new Dictionary<string, List<string>>();
			private static readonly Dictionary<string, List<string>> leafFrameCompatiblityDictionary = new Dictionary<string, List<string>>();

			static AlutechProfileRestriction()
			{
				// c# 2.0 нет инициализатора словарей
				// 62
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0303 (створка без порога)", new List<string>(new string[] {"Без порога"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0106 (створка без порога)", new List<string>(new string[] {"Без порога"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0106 (створка)", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0103 (створка)", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0204", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0205", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				// 62 дверная створка -> рама
				leafFrameCompatiblityDictionary.Add("AYPC.W62.0204", new List<string>(new string[] {"AYPC.W62.0104", "AYPC.W62.0205 (рама)"}));
				leafFrameCompatiblityDictionary.Add("AYPC.W62.0205", new List<string>(new string[] {"AYPC.W62.0105", "AYPC.W62.0204 (рама)"}));

				// 48
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0104 (створка цоколь без порога)", new List<string>(new string[] {"Без порога"}));
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0104 (створка цоколь)", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806"}));

				// 48 створка T нельзя для использовать совместно с рамой AYPC.C48.0202 (рама), но до этой проверки мы доходим только если есть порожные ограничения
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0202", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806", "Без порога"}));
				leafFrameCompatiblityDictionary.Add("AYPC.C48.0202", new List<string>(new string[] {"AYPC.C48.0105", "AYPC.C48.0203 (рама)", "Без рамы"}));

				leafPorogCompatiblityDictionary.Add("AYPC.C48.0203", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806", "Без порога"}));
				leafFrameCompatiblityDictionary.Add("AYPC.C48.0203", new List<string>(new string[] {"AYPC.C48.0105", "AYPC.C48.0202 (рама)", "Без рамы"}));
			}
		}


		private class AlutechPorogInvariant : SilentInvariant
		{
			private readonly ComponentType _componentType;

			public AlutechPorogInvariant(clsBeem subj, ComponentType componentType) : base(subj)
			{
				_componentType = componentType;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// обрабатывается только для след типов событий т.к. смена .ChangeLeafSize прилетает раньше .ChangeProfile 
				if (model.LastAction == wdAction.ChangeProfile || model.LastAction == wdAction.AddPerimetr || model.LastAction == wdAction.ChangeProfileSystem || model.LastAction == wdAction.ChangeConstructionType)
				{
					// правильный тип
					ComponentType type = model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor ? ComponentType.Porog : ComponentType.Frame;

					// параметр отбоя контроля
					clsUserParam up = model.UserParameters.GetParam("Ручное управление порогом");

					// иногда, используя свои сверхспособности мы можем попытаться угадать намерения пользователя
					if (model.LastAction == wdAction.ChangeProfile && model.SelectedBeems != null && model.SelectedBeems.Count > 0 && isOnlyBottomFrame(model.SelectedBeems))
					{
						if (model.SelectedBeems[0].bType != type)
						{
							if (up != null)
								up.StringValue = "да";
						}
					}

					// снести значение если не тот тип конструкции
					if (up != null && up.DefaultValue.StrValue != up.StringValue)
					{
						if (model.ConstructionType.Name != _indoor && model.ConstructionType.Name != _outdoor && model.ConstructionType.Name != _swingdoor)
							up.StringValue = up.DefaultValue.StrValue;
					}

					// если стоит ручное управление порогом то выходим отсюдова
					if (up != null && up.DefaultValue.StrValue != up.StringValue)
						return list;

					// todo построитель сам меняет порог, но не у всех нижних балок, блин колхоз, на то что доступно в окне поэтому сюда приходит уже Frame
					foreach (clsBeem beem in model.Frame)
						if (beem.PositionBeem == ItemSide.Bottom && beem.bType != type)
							if (getRestrictionOnPorog(model))
								list.Add(new AlutechPorogInvariant(beem, type));
				}

				return list;
			}

			private static bool isOnlyBottomFrame(colBeem selectedBeems)
			{
				foreach (clsBeem beem in selectedBeems)
				{
					if (beem.BalkaType != ModelPart.RamaItem)
						return false;

					//                    if (beem.bType != ComponentType.Porog)
					//                        return false;

					if (beem.PositionBeem != ItemSide.Bottom)
						return false;
				}

				return true;
			}

			// todo костыль
			private static bool getRestrictionOnPorog(clsModel model)
			{
				try
				{
					OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
					DataRow[] drdocsign = order.ds.docsign.Select("sign_name = 'Заказ' and signvalue_str = 'Отмена ограничений' and deleted is NULL");
					if (drdocsign.Length > 0)
						foreach (int i in Settings.idpeoplegroup)
							if (i == 10 || i == 36 || i == 44 || i == 45 || i == 46)
								return false;
				}
				catch (Exception)
				{
				}

				return true;
			}

			public override string uniq()
			{
				return base.uniq() + ((clsBeem) subject).GetHashCode();
			}

			public override void fix()
			{
                /*foreach (clsProfile profile in Model.WinDraw.ProfileSystems.FromName(Model.ProfileSystem.Name).colProfileFrame)
                {
                    if (profile.Marking == "AYPC.W62.0801")
                    {
                        beem.Profile = profile;
                        beem.ConnectType = ConnectType.Короткое;
                    }
                }*/
				clsBeem beem = (clsBeem) subject;
				switch (_componentType)
				{
					case ComponentType.Porog:
						// тут надо выставить в первый из порогов опеределеных в системах их может быть более 1 но то не наше дело
						foreach (clsProfile profile in beem.Model.ConstructionType.ProfForType(_componentType)) // теперь выдает согласно numpos, но раму и порог вместе
						{
							if (profile.ProfType == _componentType)
							{
								beem.Profile = profile;
								beem.ConnectType = ConnectType.Короткое;
								break;
							}
						}

						break;

					case ComponentType.Frame:
						// todo построитель сам меняет порог на то что идем первое в новом типе конструкции - пока не трогеем тут
						return;
				}
			}
		}

		private class AlutechSocleInvariant : SilentInvariant
		{
			private readonly ConnectType _connectType;

			public AlutechSocleInvariant(clsLeafBeem subj, ConnectType connectType) : base(subj)
			{
				_connectType = connectType;
			}

			public override string uniq()
			{
				return base.uniq() + subject.GetHashCode();
			}

			public static IEnumerable<Invalidance> test(clsLeaf leaf)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				foreach (clsLeafBeem beem in leaf)
				{
					if (isLeafOnlyProfile(beem))
					{
						if (beem.PositionBeem == ItemSide.Bottom && beem.ConnectType != ConnectType.Равное)
							list.Add(new AlutechSocleInvariant(beem, ConnectType.Равное));
					}
					else
					{
						if (beem.PositionBeem == ItemSide.Bottom && beem.ConnectType != ConnectType.Короткое)
							list.Add(new AlutechSocleInvariant(beem, ConnectType.Короткое));

						if (beem.PositionBeem != ItemSide.Bottom)
							list.Add(new AlutechProfileRestriction(leaf, "цокольный профиль только для нижней балки"));
					}
				}

				return list;
			}

			protected static bool isLeafOnlyProfile(clsBeem beem)
			{
				string mark = beem.Profile.Marking.Split(' ')[0];

				// встречается в рамах
				if (beem.Model.ProfileSystem.GetFrameProfileByMarking(mark, false) != null)
					return false;

				// встречается в импостах
				if (beem.Model.ProfileSystem.GetImpostProfileByMarking(mark, false) != null)
					return false;

				return true;
			}

			public override void fix()
			{
				((clsBeem) subject).ConnectType = _connectType;
			}
		}

		private class AlutechMoskitoLessDoorInvariant : Invariant
		{
			public AlutechMoskitoLessDoorInvariant(clsLeaf subj) : base(subj)
			{
			}

			public override string message()
			{
				return string.Format("На Двери {0} москитная сетка не устанавливаниется", ((clsLeaf) subject).Model.ProfileSystem.Name);
			}

			public override void fix()
			{
				((clsLeaf) subject).IsMoskit = IsMoskit.Нет;
			}
		}

		private class ColorRestriction : Invalidance
		{
			private readonly string _message;

			protected ColorRestriction(clsModel subj, string message) : base(subj)
			{
				this._message = message;
			}

			public override string message()
			{
				return _message;
			}

			// id цветовых групп
			public const int __witeColorGroup = 10;
			public const int __laminatedColorGroup = 1;
			public const int __aluminiumColorGroup = 12;
			public const int __RALColorGroup = 26;

			// белый пвх
			public static bool isWhite(clsConstructionColor color)
			{
				return color.IDColorGroup == __witeColorGroup;
			}

			// ламинация
			public static bool isLam(clsConstructionColor color)
			{
				return color.IDColorGroup == __laminatedColorGroup || color.IDParentColorGroup == __laminatedColorGroup;
			}


			// фабричный алюминий
			public static bool isFabcolor(clsConstructionColor color)
			{
				return color.IDColorGroup == __aluminiumColorGroup;
			}

			public static bool isPaint(clsConstructionColor color)
			{
				return color.IDColorGroup == __RALColorGroup || color.IDParentColorGroup == __RALColorGroup;
			}


			// можно покрасить ?
			public static bool isPaintable(clsConstructionColor color)
			{
				return isFabcolor(color) && (color.ColorName == "Белый RAL9016" || color.ColorName == "Не окрашенный"); /// TODO use Dictionary
			}

			// алютех может идти сам как есть его ламинируют или красят, что касаемо одного конкретного цвета
			public static bool isAlutechColor(clsConstructionColor color)
			{
				return isFabcolor(color) || isLam(color) || isPaint(color);
			}

			// говорит о том чо например если other зеленый или ламинация то color может быть фабричным, а если уже фабричный и не != color то нельзя
			public static bool isAcceptFabDependsOther(clsConstructionColor color, clsConstructionColor other)
			{
				return !(alutechColorMap.ContainsValue(other.IDColor) && alutechColorMap.ContainsKey(color.IDColor) && alutechColorMap[color.IDColor] != other.IDColor);
			}

			// таблица приведения краски к фабричным цветам
			private static readonly Dictionary<int, int> alutechColorMap;

			static ColorRestriction()
			{
				alutechColorMap = new Dictionary<int, int>();
				alutechColorMap.Add(8017, 109);
				alutechColorMap.Add(9006, 107);
				alutechColorMap.Add(9016, 108);
			}

			private static readonly List<string> restrictedColorNames = new List<string>(new string[] {"Wisconsin XD"});

			public static bool isRestrictedColor(clsConstructionColor color)
			{
				return restrictedColorNames.Contains(color.ColorName);
			}

			public static Invalidance test(clsModel model)
			{
				bool restriction = getRestriction(model);

				if (restriction && (isRestrictedColor(model.ColorInside) || isRestrictedColor(model.ColorOutside)))
					return new ColorRestriction(model, string.Format("Цвет {0} доступен только по согласованию с произаодством", string.Join(",", restrictedColorNames.ToArray())));

				// пробой слоя = как обычно правила зависят от продавца
				int idseller = getIdseller(model);

				// нельзя ламинировать или красить цветами windraw = 0
				// это также повлияет на конструкции из цветов которые снесены в неиспользуемые
				/* http://yt:8000/issue/wd-34
				if (!model.ColorInside.IsVisible)
				if (restriction)
				return new ColorRestriction(model, string.Format("Цвет {0} не используется для конструкций", model.ColorInside.ColorName));

				if (!model.ColorOutside.IsVisible)
				if (restriction)
				return new ColorRestriction(model, string.Format("Цвет {0} не используется для конструкций", model.ColorOutside.ColorName));
				*/

				/// при сносе цвета в несипользуемые idcolor и id-группы выставляется в 0 что мешает правильному функционированию этого методв
				if (model.ColorOutside.IDColor == 0 || model.ColorOutside.IDColorGroup == 0)
				{
					return new ColorRestriction(model, string.Format("Цвет {0} не идентифицируется, попробуйте пересчитать конструкцию еще раз, Если это не поможет то попробуйте выбрать цвет конструкции в списке доступных", model.ColorOutside.ColorName));
				}

				if (model.ColorInside.IDColor == 0 || model.ColorInside.IDColorGroup == 0)
				{
					return new ColorRestriction(model, string.Format("Цвет {0} не идентифицируется, попробуйте пересчитать конструкцию еще раз, Если это не поможет то попробуйте выбрать цвет конструкции в списке доступных", model.ColorInside.ColorName));
				}

				/// алютех бывает некрашеный или фабричного готового цвета, +белый и +не_окрашенный можно покрасить!, а коричневый или серый уже нельзя, 
				/// если красим зеленым то либо оба зеленые либо один из них белый
				/// нельзя скрещивать краску и ламинацию
				/// если краск то up основа Белый RAL 9016
				if (model.ProfileSystem.Name == ALUTECH62 || model.ProfileSystem.Name == ALUTECH_48)
				{
					// групап цвета допустима # 12 Алюминий #1 ламинация или родительская группа = ламин или родитель RAL
					//                    if ((model.ColorInside.IDColorGroup != __aluminiumColorGroup && model.ColorInside.IDColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __RALColorGroup)
					//                        || (model.ColorOutside.IDColorGroup != __aluminiumColorGroup && model.ColorOutside.IDColorGroup != __laminatedColorGroup && model.ColorOutside.IDParentColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __RALColorGroup))
					//					{
					//						return new ColorRestriction(model, string.Format("Конструкции из {0} не бывают без цвета, используйте цвета Алюминия или Ламинацию, или RAL", model.ProfileSystem.Name));
					//					}

					// это должны быть цвета алютехоприменимые
					if (!isAlutechColor(model.ColorInside) || !isAlutechColor(model.ColorOutside))
						return new ColorRestriction(model, string.Format("Конструкции из {0} не бывают без цвета, используйте цвета Алюминия или Ламинацию, или RAL", model.ProfileSystem.Name));

					// если алюминий фабричных цветов то с обоих сторон === одинаковый
					//					if (model.ColorInside.IDColorGroup == __aluminiumColorGroup && model.ColorOutside.IDColorGroup == __aluminiumColorGroup)
					//					{
					//						if (model.ColorInside.IDColor != model.ColorOutside.IDColor)
					//							return new ColorRestriction(model, string.Format("Исходный цвет профиля {0} с обоих сторон должен быть идентичным или используйте Ламинацию", model.ProfileSystem.Name));
					//					}

					// если алюминий фабричных цветов то с обоих сторон === одинаковый
					if (isFabcolor(model.ColorInside) && isFabcolor(model.ColorOutside) && model.ColorInside.IDColor != model.ColorOutside.IDColor)
						return new ColorRestriction(model, string.Format("Фабричный цвет профиля {0} с обоих сторон должен быть обинаковым или используйте Ламинацию или покраску в цвета из палитры RAL", model.ProfileSystem.Name));

					// если выбрали из RAL == фабричному то отконвертить в фабричный без шума и пыли
					if (isPaint(model.ColorInside) && alutechColorMap.ContainsKey(model.ColorInside.IDColor) && isAcceptFabDependsOther(model.ColorInside, model.ColorOutside))
					{
						clsConstructionColor color = model.WinDraw.ConstructionColors.GetColorByID(alutechColorMap[model.ColorInside.IDColor]);
						if (color != null)
						{
							model.ColorInside = color;
						}
					}

					if (isPaint(model.ColorOutside) && alutechColorMap.ContainsKey(model.ColorOutside.IDColor) && isAcceptFabDependsOther(model.ColorOutside, model.ColorInside))
					{
						clsConstructionColor color = model.WinDraw.ConstructionColors.GetColorByID(alutechColorMap[model.ColorOutside.IDColor]);
						if (color != null)
						{
							model.ColorOutside = color;
						}
					}

					// если покрасисли в разные цвета то банить отдельным мессаджем
					// Леонтьев Григорий, 20.03.2019 12:36:52:
					// ну мы же красим профиль в разные цвета с разных сторон, никогда проблем не было
					// if (isPaint(model.ColorInside) && isPaint(model.ColorOutside) && model.ColorInside.IDColor != model.ColorOutside.IDColor)
					//  return new ColorRestriction(model, "Используйте одинаковые цвета покраски обеих сторон профиля");

					// елси покраска то другой цвет или такой же или покрашиваемый
					if (isPaint(model.ColorInside) && !(model.ColorInside.IDColor == model.ColorOutside.IDColor || isPaintable(model.ColorOutside)) ||
						isPaint(model.ColorOutside) && !(model.ColorInside.IDColor == model.ColorOutside.IDColor || isPaintable(model.ColorInside)))
					{
						if (restriction)
							return new ColorRestriction(model, string.Format("Покрасить возможно только профиль фабричного цвета {1} или {2}", model.ProfileSystem.Name, "Белый RAL 9016", "Не окрашенный")); // TODO Dictionary colors
					}

					// если покраска то up.основа ALUTECH = Белый RAL 9016 или неокрашенный
					if (isPaint(model.ColorInside) || isPaint(model.ColorOutside))
					{
						clsUserParam up = model.UserParameters.GetParam("Основа ALUTECH");
						if (up != null && up.StringValue != "Белый RAL9016" && up.StringValue != "Не окрашенный")
						{
							up.StringValue = "Белый RAL9016";
						}
					}
					else if (isLam(model.ColorInside) || isLam(model.ColorOutside))
					{
					}
					else
					{
						clsUserParam up = model.UserParameters.GetParam("Основа ALUTECH");
						if (up != null && up.StringValue != up.DefaultValue.StrValue)
						{
							up.StringValue = up.DefaultValue.StrValue;
						}
					}
				}
				else if (model.ProfileSystem.Name == SystSlayding60)
				{
					// слайдинг может быть или белый или с обоих сорон RAL одного цвета
					if ((isWhite(model.ColorInside) && isWhite(model.ColorOutside)) || (isPaint(model.ColorInside) && isPaint(model.ColorOutside) && model.ColorInside.ID == model.ColorOutside.ID))
					{
						// ok
					}
					else
					{
						if ((isPaint(model.ColorInside) || isWhite(model.ColorInside)) /*&& isWhite(model.ColorOutside)*/ && model.LastAction == wdAction.ChangeInsColor)
						{
							return new SimpleInvariant(model, string.Format("{0} должен быть окрашен с обоих сторон одинаково", model.ProfileSystem.Name), delegate { model.ColorOutside = model.ColorInside; });
						}
						else if ((isPaint(model.ColorOutside) || isWhite(model.ColorOutside)) /*&& isWhite(model.ColorInside)*/ && model.LastAction == wdAction.ChangeOutColor)
						{
							return new SimpleInvariant(model, string.Format("{0} должен быть окрашен с обоих сторон одинаково", model.ProfileSystem.Name), delegate { model.ColorInside = model.ColorOutside; });
						}
						else
						{
							return new ColorRestriction(model, string.Format("{0} окрашивается с обоих сторон одним цветом RAL или остается белым", model.ProfileSystem.Name));
						}
					}
				}
				else if (model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74 || model.ProfileSystem.Name == ProfSystGlass)
				{
					if (model.ColorInside.IDColorGroup != __witeColorGroup || model.ColorOutside.IDColorGroup != __witeColorGroup)
						return new ColorRestriction(model, string.Format("{0} только белый", model.ProfileSystem.Name));
				}
				else if ((model.ProfileSystem.Name == SibDesign && idseller != 798) // топлингам можно
					|| model.ProfileSystem.Name == RehauEuro70
					|| model.ProfileSystem.Name == RehauOptima || model.ProfileSystem.Name == Eco60 || model.ProfileSystem.Name == EnwinMinima
					|| ((model.ProfileSystem.Name == RehauBlitzNew || model.ProfileSystem.Name == RehauGrazio) && !isRbnGrazioGrey(idseller))) // этим можно, но есть иные проверки куда они должны упасть
				{
					// эти бывают только белые
					if (model.ColorInside.IDColorGroup != __witeColorGroup || model.ColorOutside.IDColorGroup != __witeColorGroup)
						if (getRestriction(model))
							return new ColorRestriction(model, string.Format("Профильная система {0} не ламинируется", model.ProfileSystem.Name));
				}
				else if (model.ProfileSystem.Name == Vario || model.ProfileSystem.Name == DECOR || model.ProfileSystem.Name == "Rehau 60мм в массе" || model.ProfileSystem.Name == "Rehau 70мм в массе" || model.ProfileSystem.Name == BAUTEKmass)
				{
					if (!isLam(model.ColorInside) || !isLam(model.ColorOutside))
					{
						if (restriction) // например заказ 192210
							return new ColorRestriction(model, string.Format("В профильной системе {0} ламинания обязательна", model.ProfileSystem.Name));
					}
				}
				else if (model.ProfileSystem.Name == FORWARD && idseller == 798)
				{
					if (isLam(model.ColorInside) || isLam(model.ColorOutside))
					{
						if (getRestriction(model))
							return new ColorRestriction(model, string.Format("Профильная система {0} не ламинируется", model.ProfileSystem.Name));
					}
				}
				else
				{
					// ПВХ нельзя использовать цвета Алюминия
					// групап цвета допустима # 10 Белый #1 ламинация или родительская группа = ламин
					if ((model.ColorInside.IDColorGroup != __witeColorGroup && model.ColorInside.IDColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __laminatedColorGroup)
						|| (model.ColorOutside.IDColorGroup != __witeColorGroup && model.ColorOutside.IDColorGroup != __laminatedColorGroup && model.ColorOutside.IDParentColorGroup != __laminatedColorGroup))
					{
						return new ColorRestriction(model, string.Format("Конструкции {0} не используют цвета Алюминия", model.ProfileSystem.Name));
					}
				}

				// отдельный чек на створку в определённой системе
				if (model.ProfileSystem.Name == EVO)
				{
					if (model.ColorInside.IDColorGroup == __laminatedColorGroup || model.ColorInside.IDParentColorGroup == __laminatedColorGroup || model.ColorOutside.IDColorGroup == __laminatedColorGroup || model.ColorOutside.IDParentColorGroup == __laminatedColorGroup)
					{
						foreach (clsLeaf leaf in model.Leafs)
						{
							if (isZ64(leaf) && getRestriction(model))
							{
								return new ColorRestriction(model, string.Format("{0} створка L64 не ламинируется", model.ProfileSystem.Name));
							}
						}
					}
				}

				// todo
				// eurodesign не ламинируется в оптовых заказах
				// оставил на усмотрение менеджера

				return null;
			}
		}


		/// much useful
		private const string Разработка = "Разработка";
		/// Системная глубина профильноый системы
		private const string __systemDepth = "Системная глубина";
		// ПВХ
		private const int __system60 = 60; // рехау | пимапен
		private const int __system70 = 70; // рехау
		private const int __system80 = 80; // рехау INTELIO 80 aka NEO 80
		private const int __system86 = 86; // рехау Geneo
		// АЛЮТЕХ
		private const int __system48 = 48;
		private const int __system62 = 62;

		// константы дверной фурнитуры
		internal const string __mechanizm = "Механизм";
		internal const string __mechanizmFlag = "МеханизмФлаг";
		internal const string __hinge = "Петли";

		// todo structure it
		internal const string __office1 = "Офисная - однозапорный замок ключ/ключ";
		internal const string __office1b = "Офисная - однозапорный замок ключ/барашек";
		internal const string __officeM = "Офисная - многозапорный ключ/ключ";
		internal const string __officeMb = "Офисная - многозапорный ключ/барашек";

		internal const string __keykey = "Нажимная ключ/ключ";
		internal const string __keybar = "Нажимная ключ/барашек";
		internal const string __keykey1 = "Нажимная однозапорный ключ/ключ";
		internal const string __keybar1 = "Нажимная однозапорный ключ/барашек";
		internal const string __balcon = "Балконный гарнитур";
		internal const string __balconk = "Балконный гарнитур ключ/ключ";
		internal const string __balconb = "Балконный гарнитур ключ/ручка";
		internal const string __wc = "Туалетный гарнитур";
		internal const string __window = "Оконная";
		internal const string __no = "Нет";

		internal const string __portalOneSide = "Портал односторонняя";
		internal const string __portalOneSideKey = "Портал односторонняя с ключом";
		internal const string __portalKeyKey = "Портал двухсторонняя ключ/ключ";
		internal const string __portalKeyBar = "Портал двухсторонняя ключ/барашек";

		internal const string __white = "Белый";
		internal const string __brown = "Коричневый";
		internal const string __whitebrown = "Белый/Коричневый";
		internal const string __bronze = "Бронза";
		internal const string __silver = "Серебро";
		internal const string __darksilver = "Темное серебро";
		internal const string __gold = "Золото";

		private static bool isDev(clsModel model)
		{
			clsUserParam up = model.UserParameters.GetParam(Разработка);
			return up != null && up.StringValue == "да";
		}

		private static bool isNewMech(clsModel model)
		{
			clsUserParam up = model.UserParameters.GetParam(__mechanizmFlag);
			return up != null && up.StringValue == true.ToString();
		}

		private static bool isLegacy(clsModel model)
		{
			return model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74 || model.ProfileSystem.Name == SystSlayding60 || model.ProfileSystem.Name == "Стеклянные конструкции";
		}

		private static void setNewMech(clsModel model, bool flag)
		{
            /*if (flag)
            {
                if (!model.CalcVariables.ContainsKey(__mechanizm))
                    model.CalcVariables.Add(__mechanizm, true.ToString());
            }
            else
            {
                if (model.CalcVariables.ContainsKey(__mechanizm))
                    model.CalcVariables.Remove(__mechanizm);
            }*/

			clsUserParam up = model.UserParameters.GetParam(__mechanizmFlag);
			if (up != null)
			{
				model.ChangeUserParameters(model.Parts[ComponentType.Frame].Name, __mechanizmFlag, flag.ToString(), string.Empty, 0);
				++model.GetMasterConstruction().Undo.isNew;
			}
			else
			{
				throw new ArgumentException(string.Format("параметр {0} не найден", __mechanizmFlag));
			}
		}

		/// открывать доступ юзерам на это поле нельзя, поэтому ставим Visible = false
		/// что приводит к невозможности его у становки руками, но и нам оно надо в рассчётах
		/// поэтому мв ставим видимый но значение только из списка а сами туда пишем только нам известную пургу
		/// 
		private void hideFlag(clsModel model)
		{
			//            clsUserParam up = model.UserParameters.GetParam(__mechanizmFlag);
			//            if (up != null)
			//                up.Visible = false;
		}

		//		private static readonly string[] toDisableStrings = new string[] { "Расстояние", "Дверные петели", "Замковый цилиндр", "Дверной замок", "Запирание штульповой створки", "Тип ручки", "Нажимная гарнитура", "Офисная ручка", "Многозапорный замок", "Оконная ручка", "Тип защёлки" };
		private static readonly string[] oldParameterNames = new string[] {"Дверной замок", "Дверные петели", "Замковый цилиндр", "Запирание штульповой створки", "Нажимная гарнитура", "Оконная ручка", "Офисная ручка", "Тип защёлки", "Тип ручки"};
		private static readonly string[] newParameterNames = new string[] {__mechanizm};


		private void disableEnable(clsModel model, wdAction action)
		{
			if (isLegacy(model))
			{
				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (string s in newParameterNames)
					{
						clsUserParam up = leaf.UserParameters.GetParam(s);
						if (up != null)
							up.Visible = false;
					}
				}

				return;
			}

			bool b = isNewMech(model);

			if (model.WinDraw != null && !model.WinDraw.ReadOnly && model.Leafs.Count > 0)
			{
				model.WinDraw.ReadOnly = !b;
			}

			foreach (clsLeaf leaf in model.Leafs)
			{
				foreach (string s in oldParameterNames)
				{
					clsUserParam up = leaf.UserParameters.GetParam(s);
					if (up != null)
						up.Visible = !b;
				}

				foreach (string s in newParameterNames)
				{
					clsUserParam up = leaf.UserParameters.GetParam(s);
					if (up != null)
						up.Visible = b;
				}
			}
		}

		/// ++this._model.master.Undo.isNew;
		private void newDoorsFlagAndRecogniserSubroutine(clsModel model, wdAction action)
		{
			if (!isLegacy(model))
			{
				switch (action)
				{
					case wdAction.BeforeParametersForm:

						break;

					// для одномодельных конструкций
					case wdAction.ShowModel:

						/// открытие  - Тынц в модель
						/// 1) елси модель = конструкция то вызвается ShowModel
						/// 2) если несколько моделей в конструкции то ничё кроме Draw не вызввается :( странно :((
						/// 3) Ведро теряет информацию в model.CalcVariables после нажатия "Отмена" в пользовательских характеристиках в построителе // просто АБЗАЦ
						/// потому что реализация разных методов Save(...) по РАЗНОМУ сериализует модель
						/// при этом особенностью нажатия "Отмена" является вызов SelectModel - этих блин Архитекторов-Аграриев ..... !
					case wdAction.SelectModel:

						// фоновый пересчет старой модели или выход из построителя с новой моделью
					case wdAction.BeforeModelCalc:
						recognizeDoorParameters(model);
						break;
				}
			}
		}

		private class TransformData
		{
			// до кучи
			public clsConstructionType constructionType;
			public clsLeaf leaf;

			// старые параметры
			public HandleType oldHandleType;

			// для окон
			public string Оконная_ручка; // цвет оконной ручки

			// для балконных дверей
			public string Тип_ручки; // тип ручки для балконных дверей, поубивав бы.....
			public string Тип_ручки_цвет; // цвет балконной гарнитуры, но не хрена ни оконной !, убить еще раз ....
			public string Балконная_ручка; // притажка

			// для дверей
			public string Дверные_петели_цвет;
			public string Замковый_цилиндр;
			public string Дверной_замок;
			public string Дверной_замок_расстояние;
			public string Нажимная_гарнитура; // цвет нажимного гарнитура
			public string Офисная_ручка; // цвет офисной ручки
			public string Тип_защёлки; // Фалевая | Роликовая
			public string Многозапорный_замок; // От ключа | От ручки = todo не понятно на что влияет

			// новые параметры
			public HandleType newHandleType;
			public string Механизм;
			public string Механизм_Цвет;
		}

		/// распознавание старых характеристик :
		/// 1) делается 1 - раз, как понять в текущес ситуации model.up уже есть но там пусто или бред ибо никто не отслеживал, ориентировацца на продукт собственной деятельности :\ - плохая идея
		/// 2) переписывает что бы там ни стояло в новых характеристиках 
		/// 4) флажок храниться в <calcVariables>Doors</calcVariables> 
		/// 5) там ошибка поэтому сделали up механизмФлаг
		private void recognizeDoorParameters(clsModel model)
		{
			//            return;

			/// флажок нового рачсёта дверей, система крайне инертна (нельзя утром проснуться в другом мире)
			/// имя флажка = имя параметра, но в CalcVariables
			/// если новый расчет то выставляем флажок и требуем его в расчётах
			/// если не новый - то по фиг - лесом
			/// версия расчета влияет на видимость clsUserParam, они навечно остаются в classNative, но в class переходят только видимые
			/// clsUserParam.Visible - персиститься в classNative поэтому достаточно выставить один раз, и убрать из построителя код который его показывает обратно это есть балконных дверях
			/// после отключения параметра в modelparam.isactive новые созданные classNative уже не будут иметь этого парамета что повалит построитель кое-где
			/// дубли classNative наследуют этот параметр, перед отключением почитить! построитель и возможно скрипты/дерево
			/// например если [тип ручки] сделать невидимым или неактивны, то благодаря useunvisible в class мы получим оконную ручку и деерво просчитает оконную, тоже надо править перед вырубанием =>
			/// какое-то количество времени на заводе будут видны оба параметра потом как переедут двери, допилим окна и тогда сделаем выгрузку диллерам - это несколько дней точно
			if (isNewMech(model))
				return;

			/// тут надо изобрести типа формочку АХТУНГ мы обнаружили такие-то параметры дверей YES | NO | Cancel
			/// ну и если YES тогда пишем новые параметры иначе лесом ниче не трогаем, 
			/// интересна как должен вести себя построитель с моделью у которой старые параметры, новых нет уже разработк ( а потом и всегда ?)
			/// ну наверно он не должен давать возможность использовать старые параметры, по идее должон быть еще и .Enable ??
			/// Хрена !!! есть только enabled в каком-то идиотском DataTable, в котором "тип" торкает неколько полей зараз //
			/// Tools.ChangeSetting("Visibility", "Тип петель", true);


			/// куда клаcть 
			List<TransformData> transformDataList = new List<TransformData>();

			foreach (clsLeaf leaf in model.Leafs)
			{
				clsUserParam up = leaf.UserParameters[__mechanizm];
				if (up == null)
					throw new Exception(string.Format("проверьте настрйки, отсутствует параметр {0}", __mechanizm));

				// todo List<struct>.Add() работает по значению, а List<class>.Add() по ссылке ктобы мог подумать !!!!?????
				TransformData transformData = new TransformData();
				transformDataList.Add(transformData);

				// безусловно
				transformData.constructionType = leaf.Model.ConstructionType;
				transformData.leaf = leaf;
				transformData.oldHandleType = leaf.HandleType;

				// ~todo нет - значит нет, пофиг на конструкцию
				if (leaf.HandleType == HandleType.Нет_ручки)
				{
					transformData.newHandleType = HandleType.Нет_ручки;
					transformData.Механизм = __no;
					if (leaf.ShtulpLeaf != null)
					{
						/// наследовать цвет механизма от основной створки
						/// чтобы еще раз не проходить квест с поиском цвета 
						/// делем это позже во втором проходе
					}
					else
					{
						transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
					}
				}
					// ~todo есть ручка, окно - пофиг какая была, будет только оконная
				else if (leaf.Model.ConstructionType.Name == _window)
				{
					transformData.newHandleType = HandleType.Оконная;
					transformData.Механизм = __window;

					// цвет оконной ручки
					clsUserParam upWindowHandleColor = leaf.UserParameters.GetParam("Оконная ручка");
					if (upWindowHandleColor != null && upWindowHandleColor.Visible)
					{
						transformData.Оконная_ручка = upWindowHandleColor.StringValue;
						switch (transformData.Оконная_ручка)
						{
							case "Белая":
								transformData.Механизм_Цвет = __white;
								break;
							case "Коричневая":
								transformData.Механизм_Цвет = __brown;
								break;
							default:
								transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
								break;
						}
					}
					else
					{
						transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
					}
				}
					/// ~todo есть ручка, балконная дверь, по фиг какая leaf.HandleType смотрим по юзер параметрам,
					/// параметр Тип_ручки будет штатно всегда, но у него если узкая створка будет визибл == false
				else if (model.ConstructionType.Name == _balcon)
				{
					clsUserParam upBalconHandle = leaf.UserParameters.GetParam("Тип ручки");
					if (upBalconHandle != null && upBalconHandle.Visible && upBalconHandle.StringValue == "Балконная гарнитура")
					{
						transformData.Тип_ручки = "Балконная гарнитура";
						transformData.newHandleType = HandleType.Нажимной_гарнитур;
						transformData.Механизм = __balcon;

						transformData.Тип_ручки_цвет = upBalconHandle.StringValue2;
						switch (transformData.Тип_ручки_цвет)
						{
							case "Белая":
							transformData.Механизм_Цвет = __white;
							break;
							case "Коричневая":
							transformData.Механизм_Цвет = __brown;
							break;
							case "Белая/Коричневая":
							transformData.Механизм_Цвет = __whitebrown;
							break;
							default:
							transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
							break;
						}

						// дефолтово все БГ встают как БГ, но они еще бывают ключ/ключ == простой, ключ/ручка == с ручкой
						clsUserParam upCilinder = leaf.UserParameters.GetParam("Замковый цилиндр");
						if (upCilinder != null && upCilinder.Visible && upCilinder.StringValue == "С ручкой")
						{
							transformData.Механизм = __balconb;
						}
						else if (upCilinder != null && upCilinder.Visible && upCilinder.StringValue == "Простой")
						{
							transformData.Механизм = __balconk;
						}
					}
					else
					{
						transformData.Тип_ручки = "Оконная ручка";
						transformData.newHandleType = HandleType.Оконная;
						transformData.Механизм = __window;
						// цвет оконной ручки понимается, не как можно было подумать, в "тип ручки", а в другом параметре совсем
						clsUserParam upWindowHandleColor = leaf.UserParameters.GetParam("Оконная ручка");
						if (upWindowHandleColor != null && upWindowHandleColor.Visible)
						{
							transformData.Оконная_ручка = upWindowHandleColor.StringValue;
							switch (transformData.Оконная_ручка)
							{
								case "Белая":
									transformData.Механизм_Цвет = __white;
									break;
								case "Коричневая":
									transformData.Механизм_Цвет = __brown;
									break;
								default:
									transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
									break;
							}
						}
						else
						{
							transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
						}
					}
				}
					// двери межкомнатные и входные
				else if (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor)
				{
					//                // для дверей
					//                public string Дверные_петели_цвет;
					//                public string Замковый_цилиндр;
					//                public string Дверной_замок;
					//                public string Дверной_замок_расстояние;
					//                public string Нажимная_гарнитура; // цвет нажимного гарнитура
					//                public string Офисная_ручка; // цвет офисной ручки
					//                public string Тип_защёлки; // Фалевая | Роликовая
					//                public string Многозапорный_замок; // От ключа | От ручки = todo не понятно на что влияет

					// тут появляется некое подобие логики есть три варианта дверных ручек - нет (уже пройдено), н.г. / офисная
					transformData.newHandleType = leaf.HandleType;
					if (leaf.HandleType == HandleType.Нажимной_гарнитур || leaf.HandleType == HandleType.Офисная)
					{
						clsUserParam upDoorLock = leaf.UserParameters.GetParam("Дверной замок");
						if (upDoorLock != null && upDoorLock.Visible)
						{
							transformData.Дверной_замок = upDoorLock.StringValue;
							transformData.Дверной_замок_расстояние = upDoorLock.StringValue2;
						}

						clsUserParam upCilinder = leaf.UserParameters.GetParam("Замковый цилиндр");
						if (upCilinder != null && upCilinder.Visible)
						{
							transformData.Замковый_цилиндр = upCilinder.StringValue;
						}

						/// ни на чё не влияет просто выводим
						clsUserParam upLatch = leaf.UserParameters.GetParam("Тип защёлки");
						if (upLatch != null && upLatch.Visible)
						{
							transformData.Тип_защёлки = upLatch.StringValue;
						}

						clsUserParam upMLL = leaf.UserParameters.GetParam("Многозапорный_замок");
						if (upMLL != null && upMLL.Visible)
						{
							transformData.Многозапорный_замок = upMLL.StringValue;
						}
						///

						/// ~todo~ //////////////////////////////////////////////////
						if (leaf.HandleType == HandleType.Нажимной_гарнитур)
						{
							// механизм
							if (transformData.Дверной_замок == "Многозапорный от ключа" || transformData.Дверной_замок == "Многозапорный от ручки" || transformData.Дверной_замок == "Однозапорный")
							{
								if (transformData.Замковый_цилиндр == "Простой")
									transformData.Механизм = __keykey;
								else if (transformData.Замковый_цилиндр == "С ручкой")
									transformData.Механизм = __keybar;
								else
									transformData.Механизм = __keykey;
							}
							else if (transformData.Дверной_замок == "Защёлка")
							{
								transformData.Механизм = __wc;
							}
							else
							{
								transformData.Механизм = __keykey;
							}

							// цвет
							clsUserParam upDoorHandle = leaf.UserParameters.GetParam("Нажимная гарнитура");
							if (upDoorHandle != null && upDoorHandle.Visible)
							{
								transformData.Нажимная_гарнитура = upDoorHandle.StringValue;
								switch (upDoorHandle.StringValue)
								{
									case "Белая":
										transformData.Механизм_Цвет = __white;
										break;
									case "Коричневая":
										transformData.Механизм_Цвет = __brown;
										break;
									case "Белая/Коричневая":
										transformData.Механизм_Цвет = __whitebrown;
										break;
									default:
										/// todo там еще бывают серебристые п рочие = все лесом
										MessageBox.Show(string.Format("цвет нажимной гарнитуры '{0}' не реализован в новых параметрах дверей", upDoorHandle.StringValue), "Внимание!");
										transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
										break;
								}
							}
							else
							{
								transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
							}
						}

						/// ~todo~ //////////////////////////////////////////////////
						if (leaf.HandleType == HandleType.Офисная)
						{
							// механизм 
							if (transformData.Дверной_замок == "Однозапорный")
							{
								if (transformData.Замковый_цилиндр == "Простой")
									transformData.Механизм = __office1;
								else if (transformData.Замковый_цилиндр == "С ручкой")
									transformData.Механизм = __office1b;
								else
									transformData.Механизм = __office1;
							}
							else if (transformData.Дверной_замок == "Многозапорный от ключа" || transformData.Дверной_замок == "Многозапорный от ручки")
							{
								if (transformData.Замковый_цилиндр == "Простой")
									transformData.Механизм = __office1; // TODO АХТУНГ !!! АХТУНГ !!
								else if (transformData.Замковый_цилиндр == "С ручкой")
									transformData.Механизм = __office1b; // TODO АХТУНГ !!! АХТУНГ !!
								else
									transformData.Механизм = __office1; // TODO АХТУНГ !!! АХТУНГ !!
							}
							else
							{
								transformData.Механизм = __office1;
							}

							// цвет
							clsUserParam upOfficeHandle = leaf.UserParameters.GetParam("Офисная ручка");
							if (upOfficeHandle != null && upOfficeHandle.Visible)
							{
								transformData.Офисная_ручка = upOfficeHandle.StringValue;
								switch (upOfficeHandle.StringValue)
								{
									case "Белая":
										transformData.Механизм_Цвет = __white;
										break;
									case "Коричневая":
										transformData.Механизм_Цвет = __brown;
										break;
									case "Белая/Коричневая":
										transformData.Механизм_Цвет = __whitebrown;
										break;
									default:
										/// todo ???
										MessageBox.Show(string.Format("цвет офисной ручки '{0}' не реализован в новых параметрах дверей", upOfficeHandle.StringValue), "Внимание!");
										transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
										break;
								}
							}
							else
							{
								transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
							}
						}

						/// цвет петель = лесом
						clsUserParam upHingeColor = leaf.UserParameters.GetParam("Дверные петели");
						if (upHingeColor != null && upHingeColor.Visible)
						{
							transformData.Дверные_петели_цвет = upHingeColor.StringValue2;

							// todo возможно надо проверить соответствие с цветом механизма
						}
					}
					else
					{
						transformData.newHandleType = HandleType.Нет_ручки;
						transformData.Механизм = __no;
						transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
					}
				}
				else if (model.ConstructionType.Name == _moskit)
				{
					transformData.newHandleType = HandleType.Нет_ручки;
					transformData.Механизм = __no;
					transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
				}
				else if (model.ConstructionType.Name == _pskportal)
				{
					transformData.newHandleType = leaf.HandleType;
					transformData.Механизм_Цвет = up.DefaultValue.StrValue2;
					switch (leaf.HandleType)
					{
						case HandleType.Нажимной_гарнитур:
							transformData.Механизм = __keykey;
							break;
						case HandleType.Офисная:
							transformData.Механизм = __office1;
							break;
						default:
							transformData.Механизм = __no;
							break;
					}
				}
					// прочее
				else
				{
					throw new NotImplementedException(string.Format("Приведение параметров створок к новому формату. Тип конструкции {0} не реализован", leaf.Model.ConstructionType.Name));
				}
			}

			// унаследовать цвет на пассивных створках от цвета активной створки
			foreach (TransformData data in transformDataList)
			{
				if (data.leaf.HandleType == HandleType.Нет_ручки && data.leaf.ShtulpLeaf != null)
					foreach (TransformData data2 in transformDataList)
						if (data2.leaf == data.leaf.ShtulpLeaf)
							data.Механизм_Цвет = data2.Механизм_Цвет;
			}

			///
			/// собрать все это в простыню и спросить пользователя
			///
			StringBuilder sb = new StringBuilder();
			foreach (TransformData t in transformDataList)
			{
				sb.Append(string.Format("\n{0} {1}\n======================================\n", t.constructionType.Name, t.leaf.Name));
				sb.Append("Старые параметры:\n");
				sb.Append(string.Format("Тип ручки:{0}\n", t.oldHandleType));

				if (t.constructionType.Name == _window || t.constructionType.Name == _balcon)
				{
					sb.Append(string.Format("Оконная_ручка: {0}\n", t.Оконная_ручка));
					sb.Append(string.Format("Тип_ручки: {0}, Цвет: {1}\n", t.Тип_ручки, t.Тип_ручки_цвет));
					sb.Append(string.Format("Балконная_ручка: {0}\n", t.Балконная_ручка));
				}
				else
				{
					sb.Append(string.Format("Дверные_петели_цвет: {0}\n", t.Дверные_петели_цвет));
					sb.Append(string.Format("Замковый_цилиндр: {0}\n", t.Замковый_цилиндр));
					sb.Append(string.Format("Дверной_замок: {0}, Дверной_замок_расстояние: {1}\n", t.Дверной_замок, t.Дверной_замок_расстояние));
					sb.Append(string.Format("Нажимная_гарнитура: {0}\n", t.Нажимная_гарнитура));
					sb.Append(string.Format("Офисная_ручка: {0}\n", t.Офисная_ручка));
					sb.Append(string.Format("Тип_защёлки: {0}\n", t.Тип_защёлки));
					sb.Append(string.Format("Многозапорный_замок: {0}\n", t.Многозапорный_замок));
				}

				sb.Append("\nНовые параметры:\n");
				sb.Append(string.Format("Тип ручки:{0}\n", t.newHandleType));
				sb.Append(string.Format("Механизм:{0}\n", t.Механизм));
				sb.Append(string.Format("Цвет:{0}\n", t.Механизм_Цвет));

				if (t.constructionType.Name == _indoor || t.constructionType.Name == _outdoor)
				{
					sb.Append(string.Format("Дверные петли:{0}\n", t.Механизм_Цвет));
				}
			}

			// если в transformDataList только стекла и москиты то нефиг и спрашиват
			bool ask = false;
			foreach (TransformData transformData in transformDataList)
			{
				if (transformData.constructionType.Name != _moskit)
					ask = true;
			}

			DialogResult result = ask ? MessageBox.Show(sb.ToString(), "Приведение параметров створок к новому формату", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) : DialogResult.OK;

			if (result == DialogResult.OK)
			{
				// fix parameters
				foreach (TransformData transformData in transformDataList)
				{
					clsLeaf leaf = transformData.leaf;

					leaf.HandleType = transformData.newHandleType;

					clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
					if (up != null)
					{
						up.StringValue = transformData.Механизм;
						up.StringValue2 = transformData.Механизм_Цвет;
					}
				}

				// fix flag
				setNewMech(model, true);
			}
		}

		/// прямой ход когерентности up.strValue <= створка.тип_ручки
		private void upht(clsModel model)
		{
			// TODO
			if (!isNewMech(model))
				return;

			// xxx foreach (clsLeaf leaf in model.Leafs.SelectedLeafs) // не канает при делании штульпа
			foreach (clsLeaf leaf in model.Leafs) // а так мы торкаем все створки модели - тоде не айс
			{
				clsUserParam up = leaf.UserParameters[__mechanizm];
				if (up != null)
				{
					switch (leaf.HandleType)
					{
						case HandleType.Нажимной_гарнитур:
							if (up.StringValue != __keykey1 && up.StringValue != __keybar1 && up.StringValue != __keykey && up.StringValue != __keybar && up.StringValue != __wc && up.StringValue != __balcon && up.StringValue != __balconk && up.StringValue != __balconb && up.StringValue != __portalOneSide && up.StringValue != __portalOneSideKey && up.StringValue != __portalKeyKey && up.StringValue != __portalKeyBar)
								up.StringValue = model.ConstructionType.Name == _pskportal ? __portalKeyKey : __keykey;
							break;
						case HandleType.Офисная:
							if (up.StringValue != __office1 && up.StringValue != __office1b && up.StringValue != __officeM && up.StringValue != __officeMb)
								up.StringValue = __office1;
							break;
						case HandleType.Оконная:
							up.StringValue = __window;
							break;
						case HandleType.Нет_ручки:
							up.StringValue = __no;
							break;
					}
				}
			}
		}

		/// обратный ход когерентности створка.тип_ручки <= up.strValue
		private void htup(clsModel model)
		{
			if (!isNewMech(model))
				return;

			foreach (clsLeaf leaf in model.Leafs)
			{
				clsUserParam up = leaf.UserParameters[__mechanizm];
				switch (up.StringValue)
				{
					case __office1:
					case __office1b:
					case __officeM:
					case __officeMb:
						leaf.HandleType = HandleType.Офисная;
						break;
					case __keykey:
					case __keybar:
					case __keykey1:
					case __keybar1:
					case __wc:
						leaf.HandleType = HandleType.Нажимной_гарнитур;
						break;

					case __balcon:
					case __balconk:
					case __balconb:
						leaf.HandleType = HandleType.Нажимной_гарнитур;
						break;

					case __window:
						leaf.HandleType = HandleType.Оконная;
						break;
					case __no:
						leaf.HandleType = HandleType.Нет_ручки;
						break;

					case __portalOneSide:
					case __portalOneSideKey:
					case __portalKeyKey:
					case __portalKeyBar:
						leaf.HandleType = HandleType.Нажимной_гарнитур;
						break;

					default:
						MessageBox.Show("неизвестный механизм"); // не меняем модель если не нашли
						break;
				}

				model.WinDraw.Refresh();
			}
		}


		/// дОлжно вызызывать сей метод из BMC и перед валидатором, ибо
		/// 1) если происходит фоновый (без вызова построителя) расчет то нам надо рихтануть clssNative до того как его поглотит .Calc(), в AMC уже поздно
		/// 2) таже пользователь, а с ним и построитель может чёнить напоправлять так что мало не покажется, а валидатор который ошибки выдаёт, опирается на то что есть в classNative
		private void setInvariants(clsModel model)
		{
			// из старого  // противовзломность
			if (model.ProfileSystem.Name == Vario && (model.FurnitureSystem.Name == RotoNT || model.FurnitureSystem.Name == RotoNTDesigno))
				model.UserParameters["Комплектация фурнитуры"].StringValue = "Противовзломная";

			// параметры створок
			foreach (clsLeaf leaf in model.Leafs)
			{
				setInvariants(leaf);
			}

			// системная глубина число(int) или пустая строка
			clsUserParam up = model.UserParameters[__systemDepth];
			if (up != null)
			{
				int? depth = getSystemDepth(model.ProfileSystem.ID);				
				up.StringValue = depth != null ? depth.ToString() : string.Empty;
			}

			// Z60 Antik // не убрать там уже полно помойки навешано
			if (restriction)
				RehauZ60AntikInvariant.fix(model);
		}

		/// классификация по типу материала конструкции: неизветно; ПВХ, Алюминий, Дерево, etc. :: /// deWrapper
		private static int? getSystemDepth(int idprofsys)
		{
			// lazy proxy inside
			return CalcProcessor.Modules["getSystemDepth"].Invoke(new object[] {idprofsys})[0] as int?;
		}

		// инварианты створок створок
		private static void setInvariants(clsLeaf leaf)
		{
			/// количество петель жестко в userparam т.к. какие-то там скрипты смотрят в параметр // todo выжечь использование юзер_параметра
			leaf.UserParameters["Дверные петели"].NumericValue = leaf.HingeCount;

			/// todo новая фича
			/// Дорнсмас жестко пишем кодом // дорнмас может поменяться в самых разных местах: смена профиля, системы, вида открывания, типа конструкции
			/// построитель пишет туда эмпирически
			/// Dornmas  = (c-e)/2
			/// потом поправляем по размерному раду 0,8, 15, 25, 35
			/// это будет ключ для дерева построителя чтоб не вязаться на артикул створки
			clsBeem beem = leaf.HandleBeem ?? leaf[0];
			double dornmas = (beem.Profile.GetC() - beem.Profile.GetE()) / 2;

			if (leaf.Model.ProfileSystem.Name == ALUTECH_48 && leaf.Model.ConstructionType.Name == _swingdoor)
				dornmas = 35;

			if (dornmas < 8)
				leaf.Dornmas = 0;
			else if (dornmas < 15)
				leaf.Dornmas = 8;
			else if (dornmas < 25)
				leaf.Dornmas = 15;
			else if (dornmas < 35)
				leaf.Dornmas = 25;
			else
				leaf.Dornmas = 35;
		}

		// todo учитывая невозможность обрабатывать () в деревер расчетов обе вурнитуры пока Дверная , будет друго политическое решение  - будем думать / переделывать
		// пока минус в том что приходится анализировать еще и профиль прежде чем что-то делать - неудобно
		private class DoorInvalidance : Invalidance
		{
			private readonly string _message;

			//    SELECT DISTINCT --constructiontype.name, 
			//    system.name 
			//    FROM constructiontypedetail 
			//    INNER JOIN systemdetail on constructiontypedetail.idsystemdetail = systemdetail.idsystemdetail AND constructiontypedetail.deleted IS NULL AND systemdetail.deleted IS NULL AND constructiontypedetail.isactive = 1 AND systemdetail.isactive = 1
			//    INNER JOIN system ON systemdetail.idsystem = system.idsystem AND system.deleted IS NULL AND (system.isactive =1 OR system.idsystem = 50)
			//    INNER JOIN constructiontype ON constructiontypedetail.idconstructiontype = constructiontype.idconstructiontype AND constructiontype.deleted IS NULL AND constructiontype.isactive = 1
			//    WHERE constructiontype.name = 'Дверь межкомнатная' OR constructiontype.name = 'Дверь входная'
			//    --ORDER BY constructiontype.name, system.name 
			//            ALUTECH ALT W62
			//            Pimapen
			//            Rehau 60мм
			//            Rehau 70
			//            Rehau 70мм
			//            Vario
			//            КРАСАЛ КП45 ?
			//            КРАСАЛ КПТ74 ?

			private DoorInvalidance(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				return leaf != null ? string.Format("{0} {1}", leaf.Name, _message) : string.Format("{0}", _message);
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// порталы лесом
				if (model.ConstructionType.Name == _pskportal)
					return list;

				// КП(Т) лесом
				if (model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74)
					return list;

				if (!isNewMech(model))
					return list;

				// тип конструкции ~ фурнитура
				if ((model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor || model.ConstructionType.Name == _wcdoor) && model.FurnitureSystem.Name != FurnSystDver && model.FurnitureSystem.Name != FurnSystBEZ)
				{
					list.Add(new DoorInvalidance(model, string.Format("{0} использует только {1}", model.ConstructionType.Name, FurnSystDver)));
				}

				//Вычисление глубины системы перенес из  нежилежащего условия  потому как пригодится дальше

				int systemDepth;
				try
				{
					clsUserParam upSystemDepth = model.UserParameters.GetParam(__systemDepth);
					systemDepth = int.Parse(upSystemDepth.StringValue);
				}
				catch
				{
					list.Add(new DoorInvalidance(model, string.Format("параметр {0} не указан, либо указан некоректно, проверьте настройки", __systemDepth)));
					return list;
				}
				// своя фурнитура  // todo не своя ниже
				if (model.FurnitureSystem.Name == FurnSystDver)
				{
					// глубина системы - перенесено выше - Шрамко 252/03/20
//					int systemDepth;
//					try
//					{
//						clsUserParam upSystemDepth = model.UserParameters.GetParam(__systemDepth);
//						systemDepth = int.Parse(upSystemDepth.StringValue);
//					}
//					catch
//					{
//						list.Add(new DoorInvalidance(model, string.Format("параметр {0} не указан, либо указан некоректно, проверьте настройки", __systemDepth)));
//						return list;
//					}

					foreach (clsLeaf leaf in model.Leafs)
					{
						// 20-12-2017
						if (model.ConstructionType.Name == _wcdoor || model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor)
						{
							if (leaf.OpenType != OpenType.Поворотное)
								leaf.OpenType = OpenType.Поворотное;
						}

						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up == null)
						{
							list.Add(new DoorInvalidance(leaf, string.Format("параметр {0} не указан, проверьте настройки", __mechanizm)));
							continue;
						}

						string mechanizm = up.StringValue;

						/// todo разные профсистемы разные правила --слитие
						/// щас есть Дверная и Дверная (Алюминий), вторую нельзя использовать в дереве расчетов ибо он будет искать 'Алюминий' и естессно не найдёт
						/// переименовывать на ходу не вариант, ибо из-за латентности распределённой системы нам надо _ПЛАВНЫЙ_ переход
						/// поэтому если потом захотим таки иметь Дверная Алюминий то можно будет организовать, пофиксив дереве и тут

						// бьем по типу системы todo у нас все еще нет признака ПВХ / Алюмний, Типы продукции ??? Клева, НО их тут еще нет!!! Поубивав бы .... 
						// todo это опасное место !!! , надо обязательно здесь прописывать систему
						switch (model.ProfileSystem.Name)
						{
							// пвх
							case Rehau60:
							case Rehau70:
							case Rehau70mm:
							case Vario:
							case Pimapen:
							case DECOR:

							case EuroDesign:
							case SibDesign:
							case RehauOptima:
							case RehauMaxima:
							case RehauDeluxe:

							case FORWARD:
							case BAUTEK:
							case BAUTEKmass:

								// бьем по типу конструкции 
								// потом по механизму, как в дереве расчёта
								switch (model.ConstructionType.Name)
								{
									case _outdoor:
										switch (mechanizm)
										{
											case __office1:
											case __office1b:
												// дорнмас >= 35 
												if (leaf.Dornmas < 35)
													if (RunCalc.getRestriction(model)) // Леонтьев Григорий, 11.02.2016 10:47:39: Д361384 - сними ограничения
														list.Add(new DoorInvalidance(leaf, string.Format("{0} - требуется створка из более широкого профиля Z98 / T118", mechanizm)));

												// todo прочие проверки

												break;
											case __keykey:
											case __keybar:
											case __no:

												// todo прочие проверки

												break;

											default:
												list.Add(new DoorInvalidance(leaf, string.Format("механизм {0} не реализован в {1}, используйте: {2}, {3}, {4}, {5}",
													mechanizm, model.ConstructionType.Name, __office1, __office1b, __keykey, __keybar)));
												break;
										}

										break;

									case _indoor:

										// доступно только в 60 системах и только с Dornmas = 25 , пока.... 
										// дорнмасс выставляется setInvariant немного раньше
										if (systemDepth == __system60 && leaf.Dornmas == 25)
										{
											switch (mechanizm)
											{
												case __keykey:
												case __keybar:
												case __keykey1:
												case __keybar1:
												case __wc:
												case __no:
													//case __office1:	

													// todo прочие проверки

													break;

												default:
													list.Add(new DoorInvalidance(leaf, string.Format("механизм {0} не реализован в {4}, используйте: {1}, {2}, {3}",
														mechanizm, __keykey, __keybar, __wc, _indoor)));
													break;
											}
										}
										else
										{
											OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
											bool reclamacia = order != null && order.DocRow != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 59;
											//if (!reclamacia && getRestriction(model)) 
											list.Add(new DoorInvalidance(leaf, string.Format("{0} возможно построить только в системе {1}мм из створки Z74 / T94", _indoor, __system60)));
										}

										break;

									case _wcdoor:
										switch (mechanizm)
										{
											case __wc:
											case __keykey:
											case __keybar:
											case __keykey1:
											case __keybar1:
											case __no:
												// todo прочие проверки

												break;

											default:
												if (getRestriction(model))
													list.Add(new DoorInvalidance(leaf, string.Format("механизм {0} не реализован в {4}, используйте: {1}, {2}, {3}",
														mechanizm, __wc, __keybar, __keykey, model.ConstructionType.Name)));
												break;
										}

										break;

									default:
										list.Add(new DoorInvalidance(model, string.Format("{0} используется только в конструкциях: {1} и {2}", model.FurnitureSystem.Name, _outdoor, _indoor)));
										break;
								}

								break;

							// алюмний
							case ALUTECH62:
							case ALUTECH_48:

								// todo не смотрим тип конструкции ?? наверно его пока его рулит построитель в другом месте
								switch (model.ConstructionType.Name)
								{
									case _indoor:
									case _outdoor:

										// требуем механизм
										switch (mechanizm)
										{
											case __keykey:
											case __keybar:
											case __office1:
											case __office1b:
											case __keykey1:
											case __keybar1:
											case __no:
												// todo прочие проверки
												break;
											default:
												list.Add(new DoorInvalidance(leaf, string.Format("механизм {0} не реализован в {6} / {7}, используйте: {1}, {2}, {3}, {4}, {5}",
													mechanizm, __office1, __keykey1, __keybar1, __keykey, __keybar, model.ProfileSystem.Name, model.FurnitureSystem.Name)));
												break;
										}

										break;

									case _swingdoor:
										// требуем механизм
										switch (mechanizm)
										{
											case __office1:
											case __office1b:
											case __keykey1:
											case __keybar1:
											case __no:
												// todo прочие проверки
												break;
											default:
												list.Add(new DoorInvalidance(leaf, string.Format("механизм {0} не реализован в {4} / {5}, используйте: {1}, {2}, {3}",
													mechanizm, __office1, __keykey1, __keybar1, model.ProfileSystem.Name, model.FurnitureSystem.Name)));
												break;
										}

										break;

									default:
										list.Add(new DoorInvalidance(model, string.Format("{0} используется только в конструкциях: {1}, {2} и {3}", model.FurnitureSystem.Name, _outdoor, _indoor, _swingdoor)));
										break;
								}

								break;

							default:
								list.Add(new DoorInvalidance(model, string.Format("{0} не используется в профильной системе {1}", model.FurnitureSystem.Name, model.ProfileSystem.Name)));
								return list;
						}
					}
				}

					// todo надо еще ограничить в окнах - только оконные механизмы независимо от системы
				else if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balconGlazing)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							switch (leaf.HandleType)
							{
								case HandleType.Оконная:
									// цвет тока белый или коричневый, нкльзя бело коричневый
									//if(up.StringValue2 == __whitebrown)
									//	list.Add(new DoorInvalidance(leaf, string.Format("{0} ручка бывает только {1} или {2}", HandleType.Оконная, __white, __brown)));
									break;
								case HandleType.Нет_ручки:
									break;
								default:
									list.Add(new DoorInvalidance(leaf, string.Format("только {0} ручка или {1}", HandleType.Оконная, HandleType.Нет_ручки)));
									break;
							}
						}
						else
						{
							list.Add(new DoorInvalidance(leaf, string.Format("параметр {0} не указан, проверьте настройки", __mechanizm)));
							continue;
						}
					}
				}
					// ~todo в балконных дверях - оконная или балконная гарнитура при DM=25
				else if (model.ConstructionType.Name == _balcon)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							switch (leaf.HandleType)
							{
								case HandleType.Оконная:
									// цвет тока белый или коричневый, нкльзя бело коричневый
									//if (up.StringValue2 == __whitebrown)
									//	list.Add(new DoorInvalidance(leaf, string.Format("{0} ручка бывает только {1} или {2}", HandleType.Оконная, __white, __brown)));

									break;
								case HandleType.Нет_ручки:
									// ok
									break;
								case HandleType.Нажимной_гарнитур:
									if (up.StringValue == __balcon || up.StringValue == __balconk || up.StringValue == __balconb)
									{
										if (leaf.Dornmas >= 25 && (model.FurnitureSystem.Name == RotoNT || model.FurnitureSystem.Name == RotoNTDesigno || model.FurnitureSystem.Name == SiegeniaTitan || model.FurnitureSystem.Name == SiegeniaAxxent))
										{
											//ok
										}
										else
										{
											if (getRestriction(model))
												list.Add(new DoorInvalidance(leaf, string.Format("{0} можно установить только на створку Z74,T94 при использовании фурнитурных систем {1},{2},{3}", __balcon, RotoNT, RotoNTDesigno, SiegeniaTitan)));
										}
									}
									else
									{
										list.Add(new DoorInvalidance(leaf, string.Format("{0} можно устанавливать только {1}, {2} или {3}", _balcon, __balcon, HandleType.Оконная, HandleType.Нет_ручки)));
									}

									break;
								default:
									list.Add(new DoorInvalidance(leaf, string.Format("{0} ручка не ставитья на {1}", leaf.HandleType, _balcon)));
									break;
							}
						}
						else
						{
							list.Add(new DoorInvalidance(leaf, string.Format("параметр {0} не указан, проверьте настройки", __mechanizm)));
							continue;
						}
					}
				}


				/// цвет ручки
				/// ht == Оконная && linea все кроме б/к
				/// ht == Оконная && !linea только б,к
				/// прочие б, к, б/к
				foreach (clsLeaf leaf in model.Leafs)
				{
					if (leaf.HandleType == HandleType.Нет_ручки)
					{
					
					}
					else if (leaf.HandleType == HandleType.Оконная)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							// это где то уже проверяется
							if (up.StringValue2 == __whitebrown)
							{
								list.Add(new DoorInvalidance(leaf, string.Format("Цвет оконной ручки не может быть {0}", up.StringValue2)));
								continue;
							}

							if (isPvh(model))
							{
								/// TODO refactor: анализировать один раз синглтоном дерево и составлять карту допустимых вариаций которую использовать для валидации многократно
								/// белая или коричневая
								// стандартное
								// с ключом
								// Rehau LINEA Design с замком

								/// серебро, золото, бронза, коричневый, белый ++ некоторые имеют свои особенные пока не реализованые
								// Rehau LINEA Design
								// Hoppe Austin
								// Hoppe SecuForte Hamburg
								// Hoppe Stuttgart

								/// серебро
								// Hoppe Athinai Secustik

								/// бронза
								// Hoppe Singapore
								clsUserParam up2 = leaf.UserParameters.GetParam("Исполнение оконной ручки");
								if (up2 != null)
								{
									switch (up2.StringValue)
									{
										case "стандартное":
										case "с ключом":
										case "Rehau LINEA Design с замком":
											if (up.StringValue2 != __white && up.StringValue2 != __brown)
												list.Add(new DoorInvalidance(leaf, string.Format("Цвет оконной ручки исполнения {0} не может быть {1} используйте {2} или {3} или иное исполнение", up2.StringValue, up.StringValue2, __white, __brown)));
											break;

										case "Hoppe Athinai Secustik":
											if (up.StringValue2 != __silver)
												list.Add(new SimpleInvariant(model, string.Format("Цвет оконной ручки исполнения {0} только {1}", up2.StringValue, __silver), delegate { up.StringValue2 = __silver; }));
											break;

										case "Hoppe Singapore":
											if (up.StringValue2 != __bronze)
												list.Add(new SimpleInvariant(model, string.Format("Цвет оконной ручки исполнения {0} только {1}", up2.StringValue, __bronze), delegate { up.StringValue2 = __bronze; }));
											break;
										// кроме декенника и четко попадающих в свой цвет руки не могут быть темным серебром...
										case "Rehau LINEA Design":
										case "Hoppe Austin":
										case "Hoppe SecuForte Hamburg":
										case "Hoppe Stuttgart":
											if (up.StringValue2 == __darksilver)
												list.Add(new DoorInvalidance(leaf, string.Format("Цвет оконной ручки исполнения {0} не может быть {1} используйте иное исполнение", up2.StringValue, up.StringValue2)));
											break;
										//DECEUNINCK Шрамко 23/3/2020 эти ручки применимы только для заказов Леруа...
										case "DECEUNICK":
											if (getiddocoper(model) != iddocoperLerua)
											 list.Add(new SimpleInvariant(model, string.Format("Ручка {0} недоступна для этого типа заказов", up2.StringValue), delegate { up2.StringValue = "стандартное"; }));
											else if (up.StringValue2 == __bronze || up.StringValue2 == __gold)
												list.Add(new DoorInvalidance(leaf, string.Format("Цвет оконной ручки исполнения {0} не может быть {1} используйте другой цвет", up2.StringValue, up.StringValue2)));
											break;										
										default:
											// не проверяем = все подходят
											break;
									}
								}
							}
							else
							{
								if (up.StringValue2 != __white && up.StringValue2 != __brown && up.StringValue2 != __silver)
								{
									list.Add(new DoorInvalidance(leaf, string.Format("Цвет оконной ручки не может быть {0}", up.StringValue2)));
								}
							}
						}
					}
					else
					{
						const string ИсполнениеДвернойРучки = "Исполнение дверной ручки";
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							if (up.StringValue2 != __white && up.StringValue2 != __brown && up.StringValue2 != __whitebrown)
							{
								// list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки может быть только: {0}, {1} или {2}", __white, __brown, __whitebrown)));
								if (model.ProfileSystem.Name.StartsWith("ALUTECH") && (up.StringValue == "Нажимная однозапорный ключ/ключ" || up.StringValue == "Нажимная однозапорный ключ/барашек") && up.StringValue2 == "Серебро")
								{
									// реализовано
								}
									//Все игры с цветами касаются только входных дверей с "широкой створкой"
								else if ( model.ConstructionType.Name == _outdoor & leaf.Dornmas >= 35)
								{
									//Когда исполнение без ручки - контролим цвета по своему 
									if (leaf.UserParameters.GetParam(ИсполнениеДвернойРучки) != null & leaf.UserParameters.GetParam(ИсполнениеДвернойРучки).StringValue != leaf.UserParameters.GetParam(ИсполнениеДвернойРучки).DefaultValue.StrValue)
									{
										if (up.StringValue2 == __silver || up.StringValue2 == __gold || up.StringValue2 == __bronze)
										{
											// реализовано для цвета серебро золото бронза.. 
											
										}
										else //это временно дублируется условие для всех - чтобы точно оттестировать под program
										{
											list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
										}
									}
									else //если не без ручки - тогда делим еще и системной глубине, для 70 мм доступен не весь набор
									{
										if (systemDepth == __system70)
										{
											if (up.StringValue2 == __silver || up.StringValue2 == __bronze)
											{
												// done
											}
											else
											{
												list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
											}
										}
										else if (systemDepth == __system60 & up.StringValue2 == __silver || up.StringValue2 == __bronze || up.StringValue2 == __gold)
										{
											//done
										}
										else //все другие варианты не работают...
										{

											list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
										}
                                                                               
										
										//list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
									}
                                                                         

								}
								//реализация для межкомнатных дверей с дорнмассом менее 35 (узкие створки
								else if (model.ConstructionType.Name == _indoor & leaf.Dornmas < 35)
								{
//									if (Settings.idpeople == 255)
//										MessageBox.Show (leaf.Dornmas.ToString());	
									//ситуацию без ручки вообе не вижу смысла контролить, не никакой связи с петлями, поэтому ее не трогаем и не рассматриваем. от глубины зависимость не установлена, все что нас интересует попадание только в определенные цвета : серебро, бронза золото
									if (up.StringValue2 == __silver || up.StringValue2 == __bronze || up.StringValue2 == __gold)
									{
										//done
									}
									else
									{
										list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
									}
								}
								else
								{
									list.Add(new DoorInvalidance(leaf, string.Format("Цвет ручки {0}, не доступен", up.StringValue2)));
								}
							}
						}
					}

				}

				/// накладки на петли для не _outdoor
				if (model.ConstructionType.Name == _indoor)
				{
					clsUserParam up = model.UserParameters.GetParam("Накладки на петли");
					if (up != null)
					{
						switch (up.StringValue)
						{
							case "Коричневый":
							case "Серебро":
							case "Бронза средняя":
							case "Без накладок":
							case "Белый":
								break;
							case "Золото матовое":
							case "Золото блестящее":
							default:
								list.Add(new DoorInvalidance(model, "Цвет дек. накладок может быть один из: Белый, Коричневый, Серебро, Бронза, Без накладок"));
								break;
						}
					}
				}


				return list;
			}
		}

		private class OppositeOpenView : Invalidance
		{
			private readonly string _message;

			protected OppositeOpenView(clsModel subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			public static OppositeOpenView test(clsModel model)
			{
				OpenView? openView = null;
				foreach (clsLeaf leaf in model.Leafs)
				{
					if (openView != null)
					{
						if (openView != leaf.OpenView) // не соединять ИФы !!!
							if (getRestriction(model))
								return new OppositeOpenView(model, "Створки открываются в разном направлении внутрь / наружу");
					}
					else
					{
						openView = leaf.OpenView;
					}
				}

				return null;
			}
		}

		// todo тут осталась одна проверка можно впилить в основной класс
		private class AlutechHeterogenLeafInvariant : SimpleInvariant
		{
			public AlutechHeterogenLeafInvariant(clsModel model, string message, FixDelegate fixDelegate) : base(model, message, fixDelegate)
			{
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>();

				// если створки открываются в разные стороны то не хер тут искать - модель уже косая, ориентировацца на неё нельзя
				OppositeOpenView oppositeOpenView = OppositeOpenView.test(model);
				if (oppositeOpenView != null)
				{
					list.Add(oppositeOpenView);
				}
				else
				{
					// нехер тестировать маятниковую створку
					if (model.ConstructionType.Name != _swingdoor)
					{
						foreach (clsImpost impost in model.Imposts)
						{
							// при преобразовании двери в окно АТ городит бредовый не сущ. профиль, давно не видел этих граблей
							// if (string.IsNullOrEmpty(impost.Profile.Marking)) 
							//    continue;

							foreach (clsLeafBeem b in impost.ConnectedLeafBeem)
							{
								clsLeaf leaf = b.Leaf;
								// малореально но не невозможно
								if (leaf == null)
									continue;

								// инверсный профиль ставится только на пассивную створку при использовании штульпа без профиля у которого .A == 0 (NULL)
								bool invert = leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && impost.Profile != null && impost.Profile.A == 0;

								// отталкиваемся от петлевой балки , если ее нет то выкинет исключнеие
								if (leaf.HingBeem == null || leaf.HingBeem.Profile == null || leaf.HingBeem.Profile.A == 0)
									throw new ArgumentException("не найдена петлевая балка или не указан её профиль или он 'без профиля'");

								clsProfile rightProfile = invert ? getConjugateProfile(model, leaf.HingBeem.Profile) : leaf.HingBeem.Profile;

								if (b.Profile != rightProfile)
								{
									clsLeafBeem beemToModify = b;
									list.Add(new SimpleInvariant(model, string.Format("Cтворка {0} меняем профиль балки ручки с {1} на {2}", leaf.Name, beemToModify.Profile, rightProfile), delegate { beemToModify.Profile = rightProfile; }));
								}
							}
						}
					}
				}

				return list;
			}
		}

		// створкам 0205 рама 0105 / 0204 -> 0104
		private class AlutechLeafFrameConjunctionInvariant : SilentInvariant
		{
			private readonly clsProfile _must;

			protected AlutechLeafFrameConjunctionInvariant(clsBeem subj, clsProfile must) : base(subj)
			{
				_must = must;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>();

				// если створки открываются в разные стороны то не хер тут искать - модель уже косая, ориентировацца на неё нельзя
				OppositeOpenView oppositeOpenView = OppositeOpenView.test(model);
				if (oppositeOpenView != null)
				{
					list.Add(oppositeOpenView);
				}
				else
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						foreach (clsLeafBeem leafBeem in leaf)
						{
							// есть такая тема ?
							if (leafBeem.Profile.Comment != null && (leafBeem.Profile.Comment.Contains(T) || leafBeem.Profile.Comment.Contains(Z)))
							{
								clsBeem adjacentBeem = leaf.AdjacentBeem(leafBeem);

								// пороги не T | Z пока :) и профиль рамы тоже в теме
								if (adjacentBeem != null && adjacentBeem.BalkaType == ModelPart.RamaItem && (adjacentBeem.Profile.Comment.Contains(T) || adjacentBeem.Profile.Comment.Contains(Z)))
								{
									if (!isConjuncatablePlrofile(leafBeem, adjacentBeem))
									{
										clsProfile must = getConjuncatablePlrofile(leafBeem, adjacentBeem);

										if (must != null)
											list.Add(new AlutechLeafFrameConjunctionInvariant(adjacentBeem, must));
									}
								}
							}
						}
					}
				}

				return list;
			}

			private static clsProfile getConjuncatablePlrofile(clsLeafBeem leafBeem, clsBeem frameBeem)
			{
				// тип от створки
				string TZ;
				if (leafBeem.Profile.Comment.Contains(T))
					TZ = T;
				else if (leafBeem.Profile.Comment.Contains(Z))
					TZ = Z;
				else
					return null;

				foreach (clsProfile p in frameBeem.Profile.ProfileSystem.colProfileFrame)
				{
					// сравнть .A
					if (frameBeem.Profile.RefA != p.RefA)
						continue;

					// неужели у него сопряженная буква и он входит в состав доступных для этого типа конструкций => БИНГО !
					if (p.Comment.Contains(TZ) && frameBeem.Model.ConstructionType._Frame.Contains(p))
						return p;
				}

				// увы
				return null;
			}

			private static bool isConjuncatablePlrofile(clsLeafBeem leafBeem, clsBeem frameBeem)
			{
				return !(leafBeem.Profile.Comment.Contains(Z) && frameBeem.Profile.Comment.Contains(T) || leafBeem.Profile.Comment.Contains(T) && frameBeem.Profile.Comment.Contains(Z));
			}


			public override void fix()
			{
				((clsBeem) subject).Profile = _must;
			}
		}

		private class DoorPassiveDependanceInvariant : SilentInvariant
		{
			private readonly clsUserParam upPassive;
			private readonly clsUserParam upActive;

			private DoorPassiveDependanceInvariant(clsLeaf leaf, clsUserParam upPassive, clsUserParam upActive) : base(leaf)
			{
				this.upPassive = upPassive;
				this.upActive = upActive;
			}

			public static DoorPassiveDependanceInvariant test(clsLeaf leaf)
			{
				if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.ShtulpLeaf != null)
				{
					clsUserParam upPassive = leaf.UserParameters.GetParam(__mechanizm);
					clsUserParam upActive = leaf.ShtulpLeaf.UserParameters.GetParam(__mechanizm);

					// фиксим ручку на пасивной створке зараз без колебаний вообще
					if (leaf.HandleType != HandleType.Нет_ручки && leaf.OpenType != OpenType.Маятниковое && leaf.OpenType != OpenType.Раздвижное)
					{
						leaf.HandleType = HandleType.Нет_ручки;
						if (upPassive != null)
							upPassive.StringValue = __no;
					}
						// для маятниковой сносим если не офисная
					else if (leaf.HandleType != HandleType.Нет_ручки && leaf.OpenType == OpenType.Маятниковое && leaf.HandleType != HandleType.Офисная)
					{
						leaf.HandleType = HandleType.Нет_ручки;
						if (upPassive != null)
							upPassive.StringValue = __no;
					}


					if (upPassive != null && upPassive.StringValue2 != upActive.StringValue2)
						return new DoorPassiveDependanceInvariant(leaf, upPassive, upActive);
				}

				return null;
			}

			public override void fix()
			{
				upPassive.StringValue2 = upActive.StringValue2;
			}
		}

		private class GiesseRestriction : Invalidance
		{
			private const string name = GEISSE;
			private static readonly List<string> profSystemsToUse = new List<string>(new string[] {ALUTECH62, ALUTECH_48, ProfSystBEZ});

			private readonly string _message;

			private GiesseRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				return leaf != null ? string.Format("{0} {1}", leaf.Name, _message) : string.Format("{0}", _message);
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				if (model.FurnitureSystem.Name == name)
				{
					// ограничения
					bool restriction = RunCalc.getRestriction(model);

					// родная профильная стситема
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new GiesseRestriction(model, string.Format("{0} не доступна в профильной системе {1}", name, model.ProfileSystem.Name)));

					// пробег по створкам
					foreach (clsLeaf leaf in model.Leafs)
					{
						// токо прямоуг = roto OK, если не канает тогда делаем ззамену на roto NT, но в модели написано что это Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								if (restriction)
								{
									list.Add(new GiesseRestriction(leaf, string.Format("{0} только прямогуольные створки", name)));
									break;
								}
							}
						}

						// размеры и проч
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						switch (leaf.OpenType)
						{
							case OpenType.Поворотное:
								switch (leaf.ShtulpOpenType)
								{
									// не штульповая
									case ShtulpOpenType.NoShtulp:
										if (w_F < 390 || w_F > 1700 || h_F < 600 || h_F > 2500)
											if (restriction)
												list.Add(new GiesseRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
													w_F, h_F, 390, 1700, 600, 2500)));
										break;

									// штульповая Активная
									case ShtulpOpenType.NoShtulpOnLeaf:
										list.Add(new GiesseRestriction(leaf, "Активная штульповая створка должна быть поворотно-откидной"));
										break;

									// штульповая пассивная
									case ShtulpOpenType.ShtulpOnLeaf:
										if (w_F < 300 || w_F > 1200 || h_F < 600 || h_F > 2500)
											if (restriction)
												list.Add(new GiesseRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
													w_F, h_F, 300, 1200, 600, 2500)));

										break;
								}

								break;

							case OpenType.Поворотно_откидное:
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
									list.Add(new GiesseRestriction(leaf, "Пассивная штульповая створка должна быть поворотной"));

								if (w_F < 390 || w_F > 1700 || h_F < 600 || h_F > 2500)
									if (restriction)
										list.Add(new GiesseRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры поворотно-откидной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
											w_F, h_F, 390, 1700, 600, 2500)));

								break;

							case OpenType.Откидное:
							case OpenType.Подвесное:
								if (w_F < 600 || w_F > 2500 || h_F < 390 || h_F > 1700)
									if (restriction)
										list.Add(new GiesseRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры откидной створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
											w_F, h_F, 600, 2500, 390, 1700)));


								break;
							default:
								list.Add(new GiesseRestriction(leaf, string.Format("{0} тип открывания {1} не предусмотрен", name, leaf.OpenType)));
								break;
						}

						/// Параметры уровня створки

						// тольо центральное положение ручки, инвариантом не делать - может захотят поменять систему
						if (leaf.HandlePositionType != HandlePositionType.Центральное)
							list.Add(new GiesseRestriction(leaf, "доступно только Центральное положение ручки"));

						// только оконная ручка в модели и в userParam // todo check after DOORS here
						if (leaf.HandleType != HandleType.Оконная && leaf.HandleType != HandleType.Нет_ручки)
							list.Add(new GiesseRestriction(leaf, "доступны только Оконные ручки"));

						/// Автомат по петлям, правит при проверке не спрашивая,
						/// спорный функционал, проблема возникает при смене системы на ту которая за этим не следит
						/// либо делать интерфейс тестера фурнитуры и чтоб все его реализовали - так будет по чеснаку
						if (leaf.OpenType == OpenType.Поворотное)
						{
							leaf.HingeCount = h_F > 1200 ? 3 : 2;
						}
						else if ((leaf.OpenType == OpenType.Откидное || leaf.OpenType == OpenType.Подвесное))
						{
							leaf.HingeCount = w_F > 1200 ? 3 : 2;
						}
					}

					// остатки уровня модели

					// --WK1
					clsUserParam wk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
					if (wk1 != null && wk1.StringValue != "Основная")
						list.Add(new GiesseRestriction(model, string.Format("{0}, доступна только Основная Комплектация фурнитуры", RotoOK)));
				}

				return list;
			}
		}

		private class FurnitureInvariant : SilentInvariant
		{
			private readonly clsSystemFurniture furnitureSystem;

			public FurnitureInvariant(clsModel subj, clsSystemFurniture furnitureSystem) : base(subj)
			{
				this.furnitureSystem = furnitureSystem;
			}

			public override void fix()
			{
				clsModel model = subject as clsModel;
				if (model != null)
				{
					model.FurnitureSystem = furnitureSystem;
				}
			}
		}

		private class HingeInvariant : SilentInvariant
		{
			private readonly HingeType hingeType;
			private readonly HingePosition hingePosition;
			private readonly int hingeCount;

			private HingeInvariant(clsLeaf subj, HingeType hingeType, HingePosition hingePosition, int hingeCount) : base(subj)
			{
				this.hingeType = hingeType;
				this.hingePosition = hingePosition;
				this.hingeCount = hingeCount;
			}

			public override void fix()
			{
				clsLeaf leaf = subject as clsLeaf;
				if (leaf != null)
				{
					leaf.HingeType = hingeType;
					leaf.HingePosition = hingePosition;
					leaf.HingeCount = hingeCount;
				}
			}

			public static Invalidance test(clsLeaf leaf)
			{
				// todo refactor // some legacy code
				if (leaf.Model.ConstructionType.Name == _window || leaf.Model.ConstructionType.Name == _balcon)
				{
					// на фрамугах может быть 4 петли, обычным не более 3
					if (leaf.OpenType != OpenType.Подвесное && leaf.OpenType != OpenType.Откидное && leaf.HingeCount > 3)
						leaf.HingeCount = 3;

					leaf.HingePosition = HingePosition.HingeProportional;
				}
				else if (leaf.Model.ProfileSystem.Name == ALUTECH62 || leaf.Model.ProfileSystem.Name == ALUTECH_48)
				{
					if (leaf.Model.ConstructionType.Name == _indoor || leaf.Model.ConstructionType.Name == _outdoor)
					{
						leaf.HingeCount = 3;
						leaf.HingePosition = isPvh(leaf.Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;
						leaf.HingeType = HingeType.DoorHinge;
					}
				}

				return null;
			}
		}


		private class MoskitInvalidance : Invalidance
		{
			protected MoskitInvalidance(object subj) : base(subj)
			{
			}

			public override string message()
			{
				return "Москитные сетки только прямоугольные";
			}

			public static Invalidance test(clsModel model)
			{
				if (getRestriction(model))
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.Нет)
						{
							if (leaf.Count != 4)
								return new MoskitInvalidance(leaf);
							foreach (clsBeem beem in leaf)
							{
								if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
									return new MoskitInvalidance(leaf);
							}
						}
					}
				}

				return null;
			}
		}

		private class RehauProfileRestriction : Invalidance
		{
			private readonly string _message;

			private static readonly List<string> rehauList = new List<string>(new string[]
				{
				Rehau60, Rehau70, Rehau70mm, RehauEuro70, Thermo70,
				ThermoLock, Classic, Solar, Vario, DECOR, EVO,
				EuroDesign, SibDesign, RehauOptima, RehauMaxima, RehauDeluxe, RehauBlitzNew, RehauGrazio, NEO_80, Thermo76, Estet
				});

			public RehauProfileRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			//		    public override string uniq()
			//		    {
			//		        return base.uniq()+message();
			//		    }

			public static bool isConsistent(colBeem beems)
			{
				List<clsProfile> profiles = new List<clsProfile>(1);

				foreach (clsBeem beem in beems)
				{
					clsProfile profile = beem.Profile;

					if (profile.ProfType != ComponentType.Porog && profile.A > 0 && !profiles.Contains(profile))
						profiles.Add(profile);
				}

				return profiles.Count <= 1;
			}

			private static List<string> getDistinctMarkings(clsModel model)
			{
				List<string> list = new List<string>();
				foreach (clsBeem beem in model.Frame)
				{
					clsProfile profile = beem.Profile;
					if (profile.ProfType != ComponentType.Porog && profile.A > 0 && !list.Contains(profile.Marking))
						list.Add(profile.Marking);
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						clsProfile profile = beem.Profile;
						if (profile.A > 0 && !list.Contains(profile.Marking))
							list.Add(profile.Marking);
					}
				}

				foreach (clsImpost beem in model.Imposts)
				{
					clsProfile profile = beem.Profile;
					if (beem.ShtulpType == ShtulpType.Impost && profile.A > 0 && !list.Contains(profile.Marking))
						list.Add(profile.Marking);
				}

				return list;
			}

			private static bool isMultiMassColor(IEnumerable<string> list)
			{
				bool? massColor = null;
				foreach (string marking in list)
				{
					bool m = marking.Contains("-608") || marking.Contains("-808") || marking.Contains("-908");
					if (massColor == null)
					{
						massColor = m;
					}
					else if (massColor != m)
					{
						return true;
					}
				}

				return false;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				bool restriction = getRestriction(model);

				/// todo надо или флажок или типы продукции = это уже бардель какой то выходит что придётся прописывать в 130 разных местах
				/// только по своим
				if (rehauList.Contains(model.ProfileSystem.Name))
				{
					// периметр должен состоять из одного типа профиля
					if (!isConsistent(model.Frame))
						if (restriction)
							list.Add(new RehauProfileRestriction(model, "Рама состоит из профилей разного сечения"));

					foreach (clsLeaf leaf in model.Leafs)
					{
						if (!isConsistent(leaf))
							if (restriction)
								list.Add(new RehauProfileRestriction(model, string.Format("Створка {0} состоит из профилей разного сечения", leaf.Name)));
					}

					// инвариант порога
					foreach (clsFrame frame in model.Frame)
					{
						if (frame.bType == ComponentType.Porog && frame.ConnectType != ConnectType.Короткое)
						{
							// C# 2.0 behaviour
							clsFrame frame1 = frame;
							list.Add(new SimpleInvariant(model, null, delegate { frame1.ConnectType = ConnectType.Короткое; }));
						}
					}

					// расчёт 55000 только через Григория
					if (restriction)
					{
						foreach (clsFrame frame in model.Frame)
						{
							const string marking = "550000";
							if (frame.Profile.Marking == marking)
							{
								list.Add(new RehauProfileRestriction(model, string.Format("Профиль {0} доступен только по согласованию с производством", marking)));
							}
						}
					}

					// todo в изделии нельзя совмещать белый и кашированный профиль
					List<string> markings = getDistinctMarkings(model);
					if (markings != null && isMultiMassColor(markings))
						list.Add(new RehauProfileRestriction(model, string.Format("Изделие систоит из профилей разной основы {0}", string.Join(",", markings.ToArray()))));

					// 14-01-2019 окончательный и беспоровотный откат бана // пони, круг ....
					// 26-09-2018 окончательный и беспоровотный бан без исключений 
					// 24-07-2018 бан по 550711 кроме SIB-DeSIGN
					//					foreach (clsBeem beem in model.Frame)
					//					{
					//						if (beem.Profile.Marking == "550711")
					//						{
					//							list.Add(new RehauProfileRestriction(model, string.Format("Профиль {0} недоступен", beem.Profile.Marking)));
					//							break;
					//						}
					//					}

					// на пороги не положено ставить импост
					foreach (clsBeem beem in model.Frame)
					{
						if (beem.BalkaType == ModelPart.Porog)
						{
							foreach (clsBeem impost in beem.ConnectedImposts)
							{
								if (impost.BalkaType == ModelPart.Impost)
								{
									list.Add(new RehauProfileRestriction(model, "Не возможно установить импост на порог"));
									break;
								}
							}
						}
					}

					int? iddocoper = getiddocoper(model);

					if (restriction)
					{
						/// створка: вклейка с/п _начиная_ с 24мм, 
						/// тут проверка условий и если необходимо поправить то пинает в список объект ArmingInvarinat, который тока правит и гворит что поправлено  (или не говорит)
						foreach (clsLeaf leaf in model.Leafs)
						{
							clsUserParam upGlue = leaf.UserParameters["Вклейка Стеклопакета"];
							clsUserParam upArm = leaf.UserParameters["_arm"];


							/// смортим только на 0 балку, выбираем только оконные створки по ширине профиля 
							/// с указанным армированием, есть створки без армирования - их лесом
							if (leaf[0].Profile.C <= 60 && leaf.CommonColSteel().Count > 0 && !string.IsNullOrEmpty(leaf.CommonColSteel()[0]))
							{
								/// место для расчета
								string marking;
								bool glue;

								if (getRightArming(leaf, out marking, out glue))
								{
									// поправка на Vario и _Двери которые клеим всегда
									if (leaf.Model.ProfileSystem.Name == Vario || leaf.Model.ProfileSystem.Name == EVO || !leaf.Region.IsSquare())
									{
										glue = true;
									}

									// фиксим прям тут, сразу
									// порядок пользовательского армирования
									// сравниваем с порядком найденного армирования, пользовательские неизвестные и пустые армирования получают ord = 0
									if (getOrder(marking, glue) > getOrder(upArm.StringValue, upGlue.StringValue != upGlue.DefaultValue.StrValue) || !isSteelValidHere(upArm.StringValue, iddocoper))
									{
										// todo delegate установить по таблице если действительно разные армирования
										if (isDiffer(marking, upArm.StringValue))
										{
											foreach (clsBeem beem in leaf)
											{
												beem.MarkingSteel = marking;
											}
										}

										// клей
										upGlue.StringValue2 = glue ? _yes : _no;
									}
									else
									{
										// установить по таму что выбрал юзер, его пожелания канают
										foreach (clsBeem beem in leaf)
										{
											beem.MarkingSteel = upArm.StringValue;
										}

										// клей
										upGlue.StringValue2 = upGlue.StringValue; // glue ? "да" : "нет"; //;
									}
								}
								else
								{
									if (restriction)
										list.Add(new RehauProfileRestriction(leaf, string.Format("не найдено подходящего армирования для створки {0}x{1} мм", leaf.Width, leaf.Height)));
								}
							}
								// двери .C > 60 // ибо бывают оконные створки без армирования  ( L )
							else if (leaf[0].Profile.C > 60 && leaf.Model.ConstructionType.Name != _window && leaf.Model.ConstructionType.Name != _balcon)
							{
								upGlue.StringValue2 = _yes;
							}
								// EVO , Vario всегда
							else if (leaf.Model.ProfileSystem.Name == EVO || leaf.Model.ProfileSystem.Name == Vario)
							{
								upGlue.StringValue2 = _yes;
							}
								// прочее
							else
							{
								upGlue.StringValue2 = upGlue.DefaultValue.StrValue;
								upArm.StringValue = string.Empty;
							}
						}
					}

					/// крупнооптовые заказы из EURO-DESIGN по умолчанию тонкое армирование стоит парвым в списке армирования, у других систем последним - факультатив
					/// если юзер тыкает в это армирование а в ED оно вылетает на автомате то если правильный тип заказа то пропускаем если нет то пытаемся найти армирование без (1.1)
					/// если находим то меняем автоматично, иначе мат. в студию!
					{
						foreach (clsBeem beem in beemEnumerable(model))
						{
							if (!isSteelValidHere(beem, iddocoper)) //(isThinArm(beem))
							{
								clsBeem beem1 = beem;

								string steel = getNextSteel(beem);

								if (string.IsNullOrEmpty(steel))
								{
									list.Add(new RehauProfileRestriction(model, string.Format("Армирование {0} не доступно для данного типа заказа", beem.MarkingSteel)));
								}
								else
								{
									list.Add(new SimpleInvariant(model, string.Format("армирование {0} -> {1} ", beem.MarkingSteel, steel), delegate { beem1.MarkingSteel = steel; }));
								}
							}
						}
					}

					// Леонтьев Григорий, 27.06.2017 15:03:20: Нужно срочно закрыть возможность строить Штульповые окна и балконные двери с алюминиевым порогом
					if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon)
					{
						foreach (clsBeem beem in model.Frame)
						{
							if (beem.bType == ComponentType.Porog)
							{
								foreach (clsBeem impost in beem.ConnectedImposts)
								{
									if (impost.bType == ComponentType.Shtulp)
									{
										if (restriction)
											list.Add(new RehauProfileRestriction(model, string.Format("Невозможно использование штульпа совместно с порогом {0}", beem.Profile.Marking)));
									}
								}
							}
						}
					}


					// Errata по армированию в построителях
					foreach (clsBeem connector in model.Connectors)
					{
						if (!connector.Profile.Steels.Contains(connector.MarkingSteel) && connector.Profile.Steels.Count > 0)
						{
							string steel = connector.Profile.Steels[0];
							clsBeem connector1 = connector;
							// WdLog.Add(connector.Profile.Marking, connector.MarkingSteel, steel);
							list.Add(new SimpleInvariant(model, string.Format("{0} армирование {1} -> {2} ", connector.Name, connector.MarkingSteel, steel), delegate { connector1.MarkingSteel = steel; }));
						}
					}


					// ищщем под фанарём, как обычно
					// Леонтьев Григорий, 17.10.2018 17:26:09:
					//    » Д496688 - -опять чуть не залетел заказ с несимметричными створками, 
					//    » можно как то продумать, что арочные створки не могут быть не 
					//    » симметричными в дверях хотя бы
					// симметричноть створок как вы того хотите __ни поможет__ справиться с неприемлемыми углами реза в этой конструкции
					if (restriction && model.Leafs.Count > 1)
					{
						int width = 0;
						foreach (clsLeaf leaf in model.Leafs)
						{
							if (isArc(leaf))
							{
								if (width == 0)
									width = (int) leaf.Width;
								else if (width != (int) leaf.Width)
									list.Add(new RehauProfileRestriction(leaf, "Согласуйте с производством возможность изготовления асимметричных арочных створок"));
							}
						}
					}

					// Ващило Анастасия, 17.10.2018 17:20:57: д485308 - в одном изделии стоят разные створки. Можно 
					// поставить ограничения в программе типа: в одном изделии должны ставиться одинаковые створки вне зависимости от хотелок
					if (restriction && model.Leafs.Count > 1)
					{
						clsProfile profile = null;
						foreach (clsLeaf leaf in model.Leafs)
						{
							if (profile == null)
								profile = leaf.HingBeem.Profile;
							else if (profile != leaf.HingBeem.Profile)
								list.Add(new RehauProfileRestriction(leaf, "Створки из различных профилей в одной раме"));
						}
					}

					// todo между осевой и пеллевой балкой min < 30° пока используем костыль в ограничениях титана
					//                    foreach (clsLeaf leaf in model.Leafs)
					//                    {
					//                        clsLeafBeem hingBeem = leaf.HingBeem;
					//                        if (hingBeem != null)
					//                        {
					//                        }
					//                    }

					// 20-08-2019 // 541150 жестко ставим на место правильное армирование, потом убрать как пройдет лаг со старым армировнием
					foreach (clsBeem impost in model.Imposts)
					{
						const string marking = "541150 (импост)";
						const string steel = "251886";
						if (impost.Profile.Marking == marking && impost.MarkingSteel != steel)
						{
							clsBeem impost1 = impost;
							list.Add(new SimpleInvariant(model, string.Format("импост {0} правильное армирование {1}", marking, steel), delegate { impost1.MarkingSteel = steel; }));
						}
					}
				}

				list.AddRange(InactiveProfileRestriction.test(model));

				return list;
			}

			// доступное тут армирование по причине а) присутствия в списе профиля, б) по иным причинам например 1,1 - не для всех
			private static bool isSteelValidHere(clsBeem beem, int? iddocoper)
			{
				// todo is beem.markingSteel in Steels

				if (!isThinArm(beem))
					return true;

				return iddocoper == idddocoperKrupn || iddocoper == idddocoperPred;
			}

			private static bool isSteelValidHere(string steel, int? iddocoper)
			{
				if (!steel.EndsWith("(1.1)"))
					return true;

				return iddocoper == idddocoperKrupn || iddocoper == idddocoperPred;
			}


			private static string getNextSteel(clsBeem beem)
			{
				// немного банально
				int indexOf = beem.Profile.Steels.IndexOf(beem.MarkingSteel);
				indexOf = ++indexOf % beem.Profile.Steels.Count;
				return beem.Profile.Steels[indexOf];
			}

			private static bool isThinArm(clsBeem beem)
			{
				return beem.MarkingSteel.EndsWith("(1.1)");
			}

			private static IEnumerable<clsBeem> beemEnumerable(clsModel model)
			{
				foreach (clsBeem beem in model.Frame)
				{
					yield return beem;
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						yield return beem;
					}
				}

				foreach (clsBeem beem in model.Imposts)
				{
					yield return beem;
				}
			}

			private static bool isDiffer(string arm, string other)
			{
				const int L = 6;
				if (arm.Length > L)
					arm = arm.Remove(L);
				if (other.Length > L)
					other = other.Remove(L);

				return arm != other;
			}

			private static bool isThinArm(clsModel model, out string arm)
			{
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.MarkingSteel.EndsWith("(1.1)"))
					{
						arm = beem.MarkingSteel;
						return true;
					}
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (beem.MarkingSteel.EndsWith("(1.1)"))
						{
							arm = beem.MarkingSteel;
							return true;
						}
					}
				}

				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.MarkingSteel.EndsWith("(1.1)"))
					{
						arm = beem.MarkingSteel;
						return true;
					}
				}


				arm = null;
				return false;
			}

			private const string _yes = "да";
			private const string _no = "нет";


			private static readonly string[] armOrder;

			private static int getOrder(string marking, bool glue)
			{
				/// чтоб проканывало 244506(1.1)
				if (marking.Length > 6)
					marking = marking.Remove(6);

				for (int i = 0; i < armOrder.Length; i++)
				{
					if (armOrder[i] == marking)
						return i * 2 + (glue ? 1 : 0);
				}

				return -1;
			}


			private class Vector
			{
				public readonly double x0;
				public readonly double x1;
				public readonly double y0;
				public readonly double y1;

				public Vector(double x0, double y0, double x1, double y1)
				{
					this.x0 = x0;
					this.x1 = x1;
					this.y0 = y0;
					this.y1 = y1;
				}

				public double p(double x, double y)
				{
					return (x1 - x0) * (y - y0) - (y1 - y0) * (x - x0);
				}
			}

			private static readonly DataTable group;

			// статический конструктор вызывается один раз при первом использованиии объекта, обычно при первом открытии построителя после загрузки настроекк
			// имхо это рулиться чем-то - тут как удобно компилятору
			static RehauProfileRestriction()
			{
				// todo 
				DataTable table = Settings.GetSetVar("Створка армирование").TableCustomValue;
				//DataTable table = Settings.GetSetVar(313).TableCustomValue;
				group = (new DataView(table)).ToTable(true, new string[] {_openType, _color, _marking, _glue, "ord"}); // ord = порядок перебора вариантов
				group.Columns.Add(_vectors, typeof(List<Vector>));

				// make slabs
				foreach (DataRow row in group.Select())
				{
					List<Vector> vectors = new List<Vector>();
					DataRow[] rows = table.Select(string.Format("openType = '{0}' and color = {1} and glue = {2} and marking = {3}", row[_openType], row[_color], row[_glue], row[_marking]), "id");

					for (int i = 0, j = 1; j < rows.Length; i++, j++)
					{
						const string x = "x";
						const string y = "y";
						Vector v = new Vector(Convert.ToDouble(rows[i][x]), Convert.ToDouble(rows[i][y]), Convert.ToDouble(rows[j][x]), Convert.ToDouble(rows[j][y]));
						if (v.y0 != v.y1)
							vectors.Add(v);
					}

					row[_vectors] = vectors;
				}

				// arming order
				// expects: 244506,244526,244536
				string s = Settings.GetSetVar("Створка армирование").strvalue;
				if (string.IsNullOrEmpty(s))
					throw new ArgumentException("прядок применения армирования не указан в переменной `Створка армирование`");

				armOrder = Regex.Split(Regex.Replace(s, @"\s", string.Empty), @"[,;]");
			}

			private const string _openType = "openType";
			private const string _color = "color";
			private const string _vectors = "vectors";
			private const string _marking = "marking";
			private const string _glue = "glue";

			private static bool getRightArming(clsLeaf leaf, out string marking, out bool glue)
			{
				bool color = !Atechnology.winDraw.Model.Settings.Settings.WhiteColors.Contains(leaf.Model.ColorInside.ColorName) || !Atechnology.winDraw.Model.Settings.Settings.WhiteColors.Contains(leaf.Model.ColorOutside.ColorName);
				OpenType openType;

				switch (leaf.OpenType)
				{
					case OpenType.Поворотное:
					case OpenType.Поворотно_откидное:
					case OpenType.Маятниковое:
					case OpenType.Раздвижное:
						openType = OpenType.Поворотно_откидное;
						break;
					case OpenType.Глухое:
						openType = leaf.Height > leaf.Width ? OpenType.Поворотно_откидное : OpenType.Откидное;
						break;
					case OpenType.Откидное:
					case OpenType.Подвесное:
					default:
						openType = leaf.OpenType;
						break;
				}


				int? iddocoper = getiddocoper(leaf.Model);


				DataRow[] rows = group.Select(string.Format("openType = '{0}' and color = {1}", openType, color), "ord");
				foreach (DataRow row in rows)
				{
					foreach (Vector v in (List<Vector>) row[_vectors])
					{
						// ориентация створки
						double x = leaf.Width;
						double y = leaf.Height;

						// вхождение в слаб
						if (v.y0 < y && y <= v.y1)
						{
							// если v^b >= 0 то ok, иначе снаружи куска (slab)
							double p = v.p(x, y);
							if (p >= 0)
							{
								clsBeem beem = leaf[0];
								// проверка наличия найденого армироания в списке этого профиля && допустимости в этом типе закза
								if (beem.Profile != null && beem.Profile.Steels != null && beem.Profile.Steels.Contains(row[_marking].ToString()) && isSteelValidHere(row[_marking].ToString(), iddocoper))
								{
									marking = row[_marking].ToString();
									glue = Boolean.Parse(row[_glue].ToString());

									/// 03-08-2016 Vario && Decor армирование фсегда 244536 палюбому.
									// if (false && (leaf.Model.ProfileSystem.Name == ProfSystVario || leaf.Model.ProfileSystem.Name == ProfSystDECOR))
									//	marking = "244536";

									return true;
								}
							}
						}
					}
				}

				marking = string.Empty;
				glue = false;
				return false;
			}
		}

		/// запоминает выбор армирования, для отсчета в дальнейшем, тут еще есть тонкостя, 
		/// при смене профиля армирование, если у нового профиля нет армирование которое стояло то оно, слетает в дефолтное для данного профиля, поэтому мы тоже должны сбрасывать если так
		private void ChangeSteel(clsModel model, wdAction action)
		{
			if (model.SelectedBeems.Count > 0)
			{
				List<clsLeaf> leafs = new List<clsLeaf>();

				// выбрать уникальные створки по выбранным балкам, оно всегда выбирает балки
				foreach (clsBeem beem in model.SelectedBeems)
				{
					clsLeafBeem leafBeem = beem as clsLeafBeem;
					if (leafBeem != null)
					{
						if (!leafs.Contains(leafBeem.Leaf))
							leafs.Add(leafBeem.Leaf);
					}
				}

				// понять поменяное армирование, грубо но быстро, тут еще могут быть косяки если legacy будет менять армирование, а оно меняет у рамы и импостов
				string arm = model.SelectedBeems[0].MarkingSteel;

				foreach (clsLeaf leaf in leafs)
				{
					clsUserParam up = leaf.UserParameters["_arm"];

					if (action == wdAction.ChangeSteel)
					{
						up.StringValue = arm;
					}
					else if (action == wdAction.ChangeProfile || action == wdAction.ChangeProfileSystem)
					{
						if (!string.IsNullOrEmpty(up.StringValue) && !leaf.CommonColSteel().Contains(up.StringValue))
						{
							up.StringValue = null;
						}
					}
				}
			}
		}

		private class GabaritRestriction : Invalidance
		{
			private readonly string _message;

			public GabaritRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			public static GabaritRestriction test(clsModel model)
			{
				// эти не ограничиваем
				switch (model.ConstructionType.Name)
				{
					case _facade:
					case _manual:
					case _gate:
					case _wicket:
						return null;
				}

				if (getRestriction(model))
				{
					const double max = 2800;

					if (model.MaxY > max && model.MaxX > max)
						return new GabaritRestriction(model, @"Габариты конструкции превышают максимально допустимые, одно из измерений не должно превышать 2800, разделите модель на части");

					clsUserParam up = model.UserParameters.GetParam("Наличник");
					if (up != null && up.StringValue != up.DefaultValue.StrValue)
					{
						const double max1 = 2500; // 13-02-2018 // 2800
						const double max2 = 2800; // 13-02-2018 // infinity+

						//				        int face = up.StringValue.StartsWith("60") ? 60 : up.StringValue.StartsWith("90") ? 90 : 0;
						//				        bool piForm = up.StringValue2 != "По периметру";
						//				        int dx = (int)up.NumericValue;

						//				        int width = (int)(model.MaxX + 2 * face);
						//				        int height = (int)(model.MaxY + face + dx);

						int width = (int) (model.MaxX);
						int height = (int) (model.MaxY);

						if (width > max1 && height > max1)
							return new GabaritRestriction(model, @"Размеры наличника превышают максимально допустимые, одно из измерений не должно превышать " + max1);

						if (width > max2 || height > max2)
							return new GabaritRestriction(model, @"Размеры наличника превышают максимально допустимые, ни одно из измерений не должно превышать " + max2);
					}
				}

				return null;
			}
		}

		#region Наличник

		// утиное наследование от Balka, его бестоляк наследовать ибо он не Serializable, поубивав бы
		// требуется публичность в целях сериализации, ибо нет object поля в Construction классе и наледовать его тоже бестоляк, поубивав бы вторично 
		// а такой офигинительный механизм как calcVariables = new Dictionary<string, string>() поубивав бы в третий раз
		// еще там есть GlobalParamList : List<GlobalParam> =  { string _Name; object _Value; , правда он как-то чуднО персистится element.SetAttribute("Value", this._Value.ToString()); 
		// и чем оно лучше серализации неопнятно
		[Serializable]
		public class B
		{
			// длинна балки в чистоте без сварки
			public int Lenght;

			// углы и радиуса по аналогии с Balka as decimal
			public decimal Ang1;
			public decimal Ang2;
			public decimal Radius1;
			public decimal Radius2;

			/// тип соединения
			public SoedType Connect1;
			public SoedType Connect2;

			/// сторона
			public ItemSide Side;
		}

		private class NalichnikRestriction : Invalidance
		{
			// имя параметра
			private const string Наличник = "Наличник";

			private readonly string _message;

			public NalichnikRestriction(clsModel subj, string message)
				: base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			public static Invalidance test(clsModel model)
			{
				clsUserParam up = model.UserParameters.GetParam(Наличник);

				// только если есть
				if (up != null && up.StringValue != "нет")
				{
					// todo если по периметру ?? забили зачемто
					// == вылет > 0
					if (up.NumericValue < 0)
						up.NumericValue = 0;

					// todo если П то нужна только 1 нижняя балка
					bool piForm = up.StringValue2 != "По периметру";
					if (piForm)
					{
						clsBeem bottom = null;
						foreach (clsBeem beem in model.Frame)
						{
							if (beem.PositionBeem == ItemSide.Bottom)
							{
								if (bottom != null)
								{
									// если уже есть то сбрасываем ибо нам нужна тольео одна 
									bottom = null;
									break;
								}

								bottom = beem;
							}
						}

						if (bottom == null)
							return new NalichnikRestriction(model, "П-образный наличник: необходимо наличие только одной нижней горизонтальной балки");

						if (model.Frame.Count > 2)
						{
							if (bottom.Beem2.PositionBeem != ItemSide.Left || bottom.Beem2.R1 > 0.05)
								return new NalichnikRestriction(model, "П-образный наличник: левая балка не ортогональна");

							if (bottom.Beem1.PositionBeem != ItemSide.Right || bottom.Beem1.R1 > 0.05)
								return new NalichnikRestriction(model, "П-образный наличник: правая балка не ортогональна");
						}
					}

					bool restriction = getRestriction(model);

					foreach (clsBeem beem in model.Frame)
					{
						// todo пока без эллиптических арок
						if (beem.R1 > 0 && beem.R2 > 0)
							if (restriction)
								return new NalichnikRestriction(model, "эллиптический арочный наличник не реализован, используйте круглые арки R2=0");

						if (beem.R1 < 0)
							return new NalichnikRestriction(model, "обратно вогнутый арочный наличник не реализован");

						if (beem.PositionBeem == ItemSide.Bottom && up.NumericValue > 0 && model.Frame.Count > 2 && (beem.Beem2.PositionBeem != ItemSide.Left || beem.Beem1.PositionBeem != ItemSide.Right))
							return new NalichnikRestriction(model, "на не ортогональном наличнике вылет не реализован");
					}

					/// если указан наличник как параметр и тип конструкции еще не наличник то стоит задуматься о гендербендер и поставить тип конструкции = Наличник
					/// критерии
					/// стоворок = 0
					/// заполнения 1 шт и без заполнения 
					/// вся рама .A = 0
					if (model.ConstructionType.Name != _nalichnik && model.Leafs.Count == 0 && model.VisibleRegions.Count == 1 && (model.VisibleRegions[0].Fill.FillType == FillType.Unknown || model.VisibleRegions[0].Fill.FillType == FillType.NotFill) && model.Frame.TrueForAll(delegate(clsBeem beem) { return beem.Profile.A < 1; }))
					{
						return new SimpleInvariant(model, "По всей видимости это наличник, меняем тип конструкции привентивно", delegate
							{
								model.ProfileSystem = model.WinDraw.ProfileSystems.FromName(ProfSystBEZ);
								model.ConstructionType = model.ProfileSystem.ConstructionTypes.Find(_nalichnik, model.WinDraw.ProfileSystems.FromName(ProfSystBEZ));
							});
					}

					if (model.ConstructionType.Name == _nalichnik)
					{
						// отстуствие створки и импостов достигается отсутсвием профилей
						// заполнение придётся рулить руками
						foreach (clsRegion region in model.VisibleRegions)
						{
							if (region.Fill.FillType != FillType.Unknown && region.Fill.FillType != FillType.NotFill)
							{
								clsRegion region1 = region;
								return new SimpleInvariant(model, "Наличник только без заполнения", delegate { region1.Fill = region1.Model.ProfileSystem.GetFillByMarking("Без_стекла_и_штапика"); });
							}
						}
					}
				}

				if (model.ConstructionType.Name == _nalichnik && up != null && up.StringValue == up.DefaultValue.StrValue)
				{
					return new NalichnikRestriction(model, "Выставьте в пользовательских параметрах ширину наличника по фасаду");
				}


				return null;
			}

			private static int getFace(clsUserParam up)
			{
				switch (up.StringValue)
				{
					case "60мм":
						return 60;
					case "90мм":
						return 90;
					default:
						return 0;
				}
			}

			// отрисовать наличник если есть
			internal static void draw(clsModel model)
			{
				clsUserParam up = model.UserParameters.GetParam(Наличник);
				if (up != null && up.StringValue != "нет")
				{
					bool piForm = up.StringValue2 != "По периметру";
					int face = getFace(up);

					double @baseX = model.Frame.MinX;
					double @baseY = model.Frame.MinY;

					List<clsLine> offsets = new List<clsLine>();

					// построить оффсеты
					foreach (clsBeem beem in model.Frame)
					{
						double overhang = beem.PositionBeem == ItemSide.Bottom ? up.NumericValue : 0;
						clsLine offset = beem.LGabarit.Clone.CreateOffset(piForm & beem.PositionBeem == ItemSide.Bottom ? overhang : face + overhang);
						offsets.Add(offset);
					}

					// пересечения
					for (int i = 0; i < offsets.Count; i++)
					{
						clsLine offset = offsets[i];
						int j = i > 0 ? i : offsets.Count;
						clsLine prev = offsets[j - 1];

						Atechnology.winDraw.Collections.colPoint crossPoints = offset.Point_CrossAll_Lines(prev);
						if (crossPoints.Count > 0)
						{
							if (crossPoints.Count > 1)
							{
								crossPoints.Sort(delegate(clsPoint point0, clsPoint point1) { return offset.Point1.DistanceToPoint(point0).CompareTo(offset.Point1.DistanceToPoint(point1)); });
							}

							clsPoint crossPoint = crossPoints[0];
							// todo
							offset.Point1.x = crossPoint.x;
							offset.Point1.y = crossPoint.y;
							// todo
							prev.Point2.x = crossPoint.x;
							prev.Point2.y = crossPoint.y;
						}
					}


					// отрисовка
					Pen pen = new Pen(Color.Gray);
					pen.DashPattern = new float[] {4, 4};

					for (int i = 0; i < model.Frame.Count; i++)
					{
						clsLine offset = offsets[i];

						clsBeem beem = model.Frame[i];
						if (piForm && beem.PositionBeem == ItemSide.Bottom)
						{
							clsLine bottom1 = offset.Clone;
							clsLine bottom2 = offset.Clone;

							bottom1.Point2.x = bottom1.Point1.x - face;
							bottom2.Point1.x = bottom2.Point2.x + face;

							bottom1.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, bottom1.ToIPrimitive());
							bottom2.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, bottom2.ToIPrimitive());
						}
						else
						{
							clsLine connect1 = new clsLine();
							connect1.Point1 = offset.Point1.Clone;
							// bug если не П-форма + вылет, отрисовывает стык до рамы всеравно, а надо до мысленной линии .A наличника, щас делать не буду = лесом
							connect1.Point2 = beem.Point1.Clone;

							offset.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, offset.ToIPrimitive());

							// не отрисовыываем стык на левой П-балке 
							if (piForm && beem.Beem1.PositionBeem == ItemSide.Bottom)
								continue;
							connect1.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, connect1.ToIPrimitive());
						}
					}
				}
			}

			// рассчитать clsModel.Nalichnik -> Construction.Nalichnik
			internal static void calc(clsModel model)
			{
				/// пробуем рассчитать наличник как раму и запихать в Construction.CalcVaraibles
				const string __nalichnik = "Наличник";
				clsUserParam up = model.UserParameters.GetParam(__nalichnik);

				if (up == null)
					return;

				// анфас
				int face = getFace(up);

				// форма
				bool piForm = up.StringValue2 != "По периметру";

				/// todo calc offset && angles
				List<B> list = new List<B>();
				foreach (clsBeem beem in model.Frame)
				{
					// не ставим на Нижнюю
					// todo можливо это стоит писать как "Без профиля" но у нас пока еще нет профиля
					if (piForm && beem.PositionBeem == ItemSide.Bottom)
						continue;

					B b = new B();

					b.Side = beem.PositionBeem;

					// todo углы сносятся на 90 у балое которые касаются Нижней если П-образный наличник
					// нумерация углов как на раме
					double ang1 = beem.LGabarit.AngelBeetwenLines360(beem.Beem2.LGabarit) / 2;
					double ang2 = beem.Beem1.LGabarit.AngelBeetwenLines360(beem.LGabarit) / 2;

					// bug around here fixing
					// вылет для этой конкретной балки, если П-форма
					int dx;

					// тип соединения штатно = равное
					b.Connect1 = b.Connect2 = SoedType.Ravnoe;

					// левая, вид изнутри
					if (beem.Beem1.PositionBeem == ItemSide.Bottom)
					{
						// вылет 
						dx = (int) up.NumericValue;

						// если П-форма то у соседних с нижней будет длинное
						if (piForm)
						{
							ang2 = 90;
							b.Connect2 = SoedType.Dlinnoe;
						}
					}
						// правая с внутри
					else if (beem.Beem2.PositionBeem == ItemSide.Bottom)
					{
						// вылет
						dx = (int) up.NumericValue;

						// если П-форма то у соседних с нижней будет длинное
						if (piForm)
						{
							ang1 = 90;
							b.Connect1 = SoedType.Dlinnoe;
						}
					}
						// основание
					else if (beem.PositionBeem == ItemSide.Bottom)
					{
						dx = 0; // todo если не ограничить что правая левая соседняя с нижней перпендикулярно основанию, то база основание != длинны онования балки
					}
						// прочая иная
					else
					{
						dx = 0;
					}

					// todo тепреь это можно будет порешать
					// bug если периметр обратно выгнутый то ri.LengthDouble = ВНУТРЕННЯЯ ДЛИННА, при этом длинна ГАБАРИТА нигде не указана кроме построителя ri.LGabarit.Length !! @КОЛХОЗ
					double x1 = (ang1 <= 90 ? face : face + beem.Profile.A) / Math.Tan(ang1 * Math.PI / 180);
					double x2 = (ang2 <= 90 ? face : face + beem.Profile.A) / Math.Tan(ang2 * Math.PI / 180);

					// todo округляем до целых миллиметров 
					// todo длина растет (или уменьшается) от длинны балки на основной на 2 разных x = faсe * tg a = face / tg b
					int length = (int) Math.Round(beem.LGabarit.Length + x1 + x2 + dx); //  + w

					// если за счет кривого вылета длинна обналички нивелируется то не пишем её
					if (length <= 0)
						continue;

					if (beem.R1 > 0)
					{
						// длинна опорной хорды
						double lengthBasisArc = beem.LGabarit.BaseLineArc.Length + 2 * face;
						// внешний радиус наличника
						double r1 = beem.R1 + face;
						length = (int) Math.Round(Math.Asin(lengthBasisArc / (2 * r1)) * 2 * r1);
					}

					b.Lenght = length;
					b.Ang1 = (decimal) ang1;
					b.Ang2 = (decimal) ang2;
					// 01-06-2016 радиус __указываем__ по радиусу рамы, допустимы только круглые арки, элиптические - лесом                 // todo основание не гнуть
					b.Radius1 = (decimal) beem.R1;


					list.Add(b);
				}

				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(B[]));
					MemoryStream mem = new MemoryStream();
					serializer.Serialize(mem, list.ToArray());
					// bug нельзя делать xml внутри xml, поэтому сжимаем
					byte[] zip = ZipArchiver.Zip2(mem.ToArray());
					string xml = Convert.ToBase64String(zip);

					if (model.Construction.CalcVariables.ContainsKey(__nalichnik))
						model.Construction.CalcVariables[__nalichnik] = xml;
					else
						model.Construction.CalcVariables.Add(__nalichnik, xml);
				}
				catch
				{
					throw;
				}
			}
		}

		#endregion Наличник

		///  PORTAL ///
		private class PortalRestriction : Invalidance
		{
			private readonly string _message;

			public PortalRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string uniq()
			{
				return _message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				return leaf != null ? string.Format("{0} {1}", leaf.Name, _message) : _message;
			}

			// ограничения по типам фурнитуры
			private static readonly Dictionary<string, SizeRestriction> furnSizeRestrictionDictionary = new Dictionary<string, SizeRestriction>();

			// список портальных механизмов
			private static readonly List<string> protalMechanizmList = new List<string>(new string[] {__portalOneSide, __portalOneSideKey, __portalKeyKey, __portalKeyBar});

			static PortalRestriction()
			{
				// Ширина створки по фальцу (ШСФ) 670 - 1200,  Высота створки по фальцу (ВСФ) (840) 1000 - 2360 // ибо не для всхе вариаций есть 840 размеры все сложнее
				furnSizeRestrictionDictionary.Add(PSKPORTAL100, new SizeRestriction(670, 840, 1200, 2360));

				// Ширина створки по фальцу (ШСФ) 670 - 1600,  Высота створки по фальцу (ВСФ) (840) 1000 - 2360
				furnSizeRestrictionDictionary.Add(PSKPORTAL160, new SizeRestriction(670, 840, 1600, 2360));

				// Ширина створки по фальцу (ШСФ) 770 - 2000,  Высота створки по фальцу (ВСФ) 1690* - 2360 // 1690 ибо не реализованы в деревее меньшие размеры запоров
				furnSizeRestrictionDictionary.Add(PSKPORTAL200, new SizeRestriction(770, 1690, 2000, 2360));

				// Ширина створки по фальцу (ШСФ) 670 - 1600,  Высота створки по фальцу (ВСФ) (840) 1000 - 2360 (2800) // для 2800 потребуется допиливать дерево
				furnSizeRestrictionDictionary.Add(PSK_160_COMFORT, new SizeRestriction(670, 840, 2000, 2360));

				// Ширина створки по фальцу 
				furnSizeRestrictionDictionary.Add(PSK_Vorne, new SizeRestriction(600, 700, 1600, 2200));
			}

			// todo логично как-то поднимать из какого-то статика ссылки на clsFurnitureSystem
			private static readonly List<string> furnSystemList = new List<string>(new string[] {PSKPORTAL100, PSKPORTAL160, PSKPORTAL200, FurnSystBEZ, PSK_160_COMFORT, PSK_Vorne});

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// обратное правило использования портальной фурнитуры: не использовать её где не надо
				if (furnSystemList.Contains(model.FurnitureSystem.Name) && model.FurnitureSystem.Name != FurnSystBEZ && model.ConstructionType.Name != _pskportal)
				{
					list.Add(new PortalRestriction(model, string.Format("{0} ипользуется только на типе конструкции {1}", model.FurnitureSystem.Name, _pskportal)));
				}

				// если это не портал то идём лесом
				if (model.ConstructionType.Name != _pskportal)
					return list;

				// прямое правило использования портальной фурнитуры: в порталах использовать только портальные фурнитуры
				if (!furnSystemList.Contains(model.FurnitureSystem.Name))
				{
					// дальше не идем т.к. имя системы ключ словаря возможностей/ограничений фурнитурной системы
					list.Add(new PortalRestriction(model, string.Format("{0} допустимы только Фурнитуры PSK", model.ConstructionType.Name)));
					return list;
				}

				// флажок ограничений для избранных
				bool restriction = getRestriction(model);

				// ограничения створки 
				foreach (clsLeaf leaf in model.Leafs)
				{
					// если не раздвижное фиксим инлайн без валидатора
					if (leaf.OpenType != OpenType.Раздвижное)
					{
						clsLeaf leaf1 = leaf;
						list.Add(new SimpleInvariant(model, "Только раздвижное открывание", delegate { leaf1.OpenType = OpenType.Раздвижное; }));
					}

					// только прямоугольные створки
					if (!isRectangleLeaf(leaf))
						list.Add(new PortalRestriction(leaf, "только прямоугольные створки"));

					// проверка профильных ограничений // требуем Z
					clsProfile profile = leaf[0].Profile;
					if (!profile.Comment.Contains("Z"))
					{
						list.Add(new PortalRestriction(leaf, "доступны только Z створки"));
						break;
					}

					// .C створки - от него играют возможности фурнитуры
					int profileC = (int) leaf[0].Profile.C;

					// механизм
					clsUserParam upMechanizm = leaf.UserParameters.GetParam(__mechanizm);

					// ограничения на .C сечения профиля
					if (model.FurnitureSystem.Name == PSKPORTAL200)
					{
						if (98 <= profileC)
						{
							// nop
						}
						else if (74 <= profileC && profileC < 98)
						{
							list.Add(new SimpleInvariant(model, string.Format("для створок Z{0} недоступна фурнитура {1}, иисправлено на {2}",
								profileC, model.FurnitureSystem.Name, PSKPORTAL160), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(PSKPORTAL160); }));
							/// дальнейшие проверки безсмыслены вы вернёмся сюдя после отработки SimpleInvariant
							break;
						}
						else if (profileC < 74)
						{
							list.Add(new SimpleInvariant(model, string.Format("для створок Z{0} недоступна фурнитура {1}, иисправлено на {2}",
								profileC, model.FurnitureSystem.Name, PSKPORTAL100), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(PSKPORTAL100); }));
							/// дальнейшие проверки безсмыслены вы вернёмся сюдя после отработки SimpleInvariant
							break;
						}
						else
						{
							list.Add(new PortalRestriction(leaf, string.Format("не заявленная ширина профиля .С={0}", profileC)));
						}
					}
					else if (model.FurnitureSystem.Name == PSKPORTAL160 && upMechanizm != null && upMechanizm.StringValue != __portalOneSide)
					{
						if (profileC < 74)
						{
							list.Add(new PortalRestriction(model, string.Format("для {0} и створки Z{1} недоступна опция {2} используйте более широкий профиль Z74, Z98 или одностороннюю ручку",
								model.FurnitureSystem.Name, profileC, upMechanizm.StringValue)));
						}
					}

					// ВСФ, ШСФ
					int h_F = leaf.BaseRegion.Lines[0].HeightInt;
					int w_F = leaf.BaseRegion.Lines[0].WidthInt;


					// 
					// фурнитурные ограничения
					if (model.FurnitureSystem.Name != FurnSystBEZ)
					{
						// аналогично по списку фурнитур кроме фурнитуры без
						List<string> availableFurniture = new List<string>();
						foreach (KeyValuePair<string, SizeRestriction> pair in furnSizeRestrictionDictionary)
						{
							SizeRestriction sizeRestriction = pair.Value;

							if (sizeRestriction.minWidth <= w_F && w_F <= sizeRestriction.maxWidth && sizeRestriction.minHeight <= h_F && h_F <= sizeRestriction.maxHeight)
							{
								availableFurniture.Add(pair.Key);
							}
						}

						// если текущей фурнитуры нет среди допустимых то возможны варианты
						if (!availableFurniture.Contains(model.FurnitureSystem.Name))
						{
							if (availableFurniture.Count == 1)
							{
								list.Add(new SimpleInvariant(model, string.Format("для данных размеров створки {0}x{1} ШСФxВСФ доступны следующие фурнитуры:{2}", w_F, h_F, string.Join(",", availableFurniture.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(availableFurniture[0]); }));
								/// дальнейшие проверки этой створки безсмыслены вы вернёмся сюда после отработки SimpleInvariant
								break;
							}
							else if (availableFurniture.Count > 1)
							{
								list.Add(new PortalRestriction(leaf, string.Format("для данных размеров створки {0}x{1} ШСФxВСФ доступны следующие фурнитуры:{2}", w_F, h_F, string.Join(",", availableFurniture.ToArray()))));
							}
							else
							{
								if (restriction)
									list.Add(new PortalRestriction(leaf, "не найдено подходящей фурнитуры для данных размеров створки и типа профиля"));
							}
						}

						// TODO пока убрал этот бред но возможно придётся вернуть что-то аналогичное 
						// механизм имееты смысл проверять только если мы туда еще не наклали furnitureInvarian, ибо в обратном случае мы проверяем тень конструкции
						// if (!isFurnitureInvariantReached(list))
						{
							if (upMechanizm != null)
							{
								// допнем ручку на PSK100 = не положено двухсторонних
								if (leaf.Model.FurnitureSystem.Name == PSKPORTAL100)
								{
									if (upMechanizm.StringValue != __portalOneSide)
									{
										upMechanizm.StringValue = __portalOneSide;
									}
								}

								// на 100/160 жостко пасивный мех = активному, а для 200 только если есть ручка
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.ShtulpLeaf != null)
								{
									if (leaf.Model.FurnitureSystem.Name == PSKPORTAL100
										|| leaf.Model.FurnitureSystem.Name == PSKPORTAL160
										|| (leaf.Model.FurnitureSystem.Name == PSKPORTAL200 && leaf.HandleType != HandleType.Нет_ручки))
									{
										clsLeaf active = leaf.ShtulpLeaf;
										clsUserParam upActiveMechanizm = active.UserParameters.GetParam(__mechanizm);
										upMechanizm.StringValue = upActiveMechanizm.StringValue;
										upMechanizm.StringValue2 = upActiveMechanizm.StringValue2;
									}
								}

								if (leaf.HandleType != HandleType.Нет_ручки)
								{
									switch (upMechanizm.StringValue)
									{
										case __portalOneSide:
											break;
										case __portalOneSideKey:
											if (model.FurnitureSystem.Name != PSK_160_COMFORT)
											{
												list.Add(new PortalRestriction(leaf, string.Format("{0} можно установить только в фурнитуре {1}", upMechanizm.StringValue, PSK_160_COMFORT)));
											}

											break;

										case __portalKeyKey:
										case __portalKeyBar:
											// changed
											// if (model.FurnitureSystem.Name == PSK_160_COMFORT && Settings.idpeople != 255)
											//    list.Add(new PortalRestriction(leaf, string.Format("{0} нельзя установить в фурнитуре {1}", upMechanizm.StringValue, model.FurnitureSystem.Name)));
											// можно поставить только на DM=>25 т.е. от Z74
											if (profileC < 74)
												list.Add(new PortalRestriction(leaf, string.Format("{0} можно установить только на створки Z74 и Z98", upMechanizm.StringValue)));
											// можно только от ВСФ >= 1000
											if (h_F < 1000)
												list.Add(new PortalRestriction(leaf, string.Format("{0} можно установить только на створки ВСФ >= 1000мм", upMechanizm.StringValue)));
											break;

										default:
											list.Add(new PortalRestriction(leaf, string.Format("{0} допустимы только механизмы: {1}", model.ConstructionType.Name, string.Join(", ", protalMechanizmList.ToArray()))));
											break;
									}
								}
							}

							// пассивная створка без ручки возможна только с для PSK 200 c односторонней ручкой 200/GH рыжая
							if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.HandleType == HandleType.Нет_ручки)
							{
								bool ok = model.FurnitureSystem.Name == PSKPORTAL200;

								if (ok)
								{
									clsLeaf activeLeaf = leaf.ShtulpLeaf;
									// todo XXX иногда АТ теряет связь, вроде как лечили уже
									if (activeLeaf != null)
									{
										clsUserParam upMechanizmActive = activeLeaf.UserParameters.GetParam(__mechanizm);
										ok = upMechanizmActive != null && upMechanizmActive.StringValue == __portalOneSide;
									}
								}

								if (!ok)
								{
									list.Add(new PortalRestriction(leaf, string.Format("пассивная створка без ручки возможна только с для {0} c односторонней ручкой PSK200/GH на активной створке", PSKPORTAL200)));
								}
							}

							// цвет ручек
							if (model.FurnitureSystem.Name == PSK_160_COMFORT)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown && upMechanizm.StringValue2 != __gold)
								{
									list.Add(new PortalRestriction(leaf, string.Format("Цвет ручки может быть только: {0}, {1}, {2}, {3} или {4}", __white, __brown, __whitebrown, __silver, __gold)));
								}
							}
							else if (model.FurnitureSystem.Name == PSKPORTAL100 || model.FurnitureSystem.Name == PSKPORTAL160)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("Цвет ручки может быть только: {0}, {1}, {2} или {3}", __white, __brown, __whitebrown, __silver)));
								}
							}
							else if (model.FurnitureSystem.Name == PSKPORTAL200)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __bronze && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("Цвет ручки может быть только: {0}, {1}, {2}, {3} или {4}", __white, __brown, __whitebrown, __bronze, __silver)));
								}
							}
							else if (model.FurnitureSystem.Name == PSK_Vorne)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("Цвет ручки может быть только: {0}, {1}, {2} или {3}", __white, __brown, __whitebrown, __silver)));
								}
							}
						}
					}
				}

				return list;
			}

			private static bool isRectangleLeaf(clsLeaf leaf)
			{
				foreach (clsBeem beem in leaf)
				{
					if (beem.PositionBeem != ItemSide.Left && beem.PositionBeem != ItemSide.Right && beem.PositionBeem != ItemSide.Top && beem.PositionBeem != ItemSide.Bottom)
						return false;

					if (beem.R1 > 0 || beem.R2 > 0)
						return false;
				}

				return true;
			}
		}

		// в сборке но вне RunCalc, в целом не принципиально
		internal class SizeRestriction
		{
			public SizeRestriction(int minWidth, int minHeight, int maxWidth, int maxHeight)
			{
				this.maxHeight = maxHeight;
				this.maxWidth = maxWidth;
				this.minHeight = minHeight;
				this.minWidth = minWidth;
			}

			public readonly int minHeight;
			public readonly int maxHeight;
			public readonly int minWidth;
			public readonly int maxWidth;
		}

		private class SiegeniaClassicRestriction : Invalidance
		{
			private const string System = SiegeniaClassic;
			private readonly string _message;

			public SiegeniaClassicRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				if (leaf != null)
					return string.Format("{0} {1} {2}", leaf.Name, leaf.Model.FurnitureSystem.Name, _message);

				clsModel model = subject as clsModel;
				if (model != null)
					return string.Format("{0} {1}", model.FurnitureSystem.Name, _message);

				return _message;
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			// todo
			private static readonly List<string> profSystemsToUse = new List<string>(new string[] {ProfSystBEZ, Rehau60, ThermoLock, Classic, DECOR, RehauEuro70, Rehau70, Thermo70, Eco60, EuroDesign, EnwinMinima, SibDesign, RehauOptima, RehauMaxima, RehauDeluxe, RehauBlitzNew, RehauGrazio, Thermo76, Estet, NEO_80, FORWARD, BAUTEK, BAUTEKmass, FavoritSpace, SCHTANDART_START, SCHTANDART_COMFORT, SCHTANDART_PREMIUM });

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// родная фурнитурная система
				if (model.FurnitureSystem.Name == System && model.Leafs.Count > 0)
				{
					// ограничения
					bool restriction = RunCalc.getRestriction(model);

					// родная профильная стситема
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new SiegeniaClassicRestriction(model, string.Format("не доступна в профильной системе {1}", System, model.ProfileSystem.Name)));

					// пробег по створкам
					foreach (clsLeaf leaf in model.Leafs)
					{
						// токо прямоуг = roto OK, если не канает тогда делаем ззамену на roto NT, но в модели написано что это Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (leaf.Count != 4 || beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								list.Add(new SiegeniaClassicRestriction(leaf, "только прямогуольные створки"));
								break;
							}
						}

						// TODO смысл понятен, но логика спорна
						// нельзя поставить на T створку вследствие толлько DM15 
						if (leaf.OpenView == OpenView.Наружу)
							list.Add(new SiegeniaClassicRestriction(leaf, "не ставиться на створки T"));

						// штульповая створка только пов, пассивная только п/о
						switch (leaf.ShtulpOpenType)
						{
							case ShtulpOpenType.ShtulpOnLeaf:
								if (leaf.OpenType != OpenType.Поворотное)
									list.Add(new SiegeniaClassicRestriction(leaf, "пассивная штульповая створка только поворотне открывание"));
								break;
							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.OpenType != OpenType.Поворотно_откидное)
									list.Add(new SiegeniaClassicRestriction(leaf, "активная штульповая створка только поворотно-откидное открывание"));
								break;
						}

						// тольо центральное положение ручки, инвариантом не делать - может захотят поменять систему
						// дерево в курсе константного пложения тоже, но оно не проверялось
						if (leaf.HandlePositionType != HandlePositionType.Центральное)
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("доступно только {0} положение ручки", HandlePositionType.Центральное)));

						// лечение бага wd-4
						if (!leaf.HandlePosition.IsAutomat)
						{
							leaf.HandlePosition.IsAutomat = true;
							leaf.HandlePosition.HandlePosition = 0;
						}


						// тольо оконная ручка в модели и в userParam
						if (leaf.HandleType != HandleType.Оконная && leaf.HandleType != HandleType.Нет_ручки)
							list.Add(new SiegeniaClassicRestriction(leaf, "доступны только Оконные ручки"));

						clsUserParam ht = leaf.UserParameters.GetParam("Тип ручки");
						if (ht != null && ht.StringValue != "Оконная ручка")
							list.Add(new SiegeniaClassicRestriction(leaf, "доступны только Оконные ручки"));

						// более 2 петель только на откидных и поворотных
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("{0} петели можно установить толька на Поворотные и Откидные створки", leaf.HingeCount)));

						// --WK1
						clsUserParam wk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
						if (wk1 != null && wk1.StringValue != "Основная")
							list.Add(new SiegeniaClassicRestriction(model, "доступна только Основная Комплектация фурнитуры"));

						// http://yt:8000/issue/WINDRAW-45
						// только белый или коричневый цвет накладок
						// clsUserParam dekas = model.UserParameters.GetParam("Накладки на петли");
						// if (dekas != null && dekas.StringValue != "Белый" && dekas.StringValue != "Коричневый" && dekas.StringValue != "Без накладок")
						// 	if (restriction)
						//		list.Add(new SiegeniaClassicRestriction(model, "доступны только Белые и Коричневые накладки на петли"));

						// размеры
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						if (sizesDictionary.ContainsKey(leaf.OpenType))
						{
							int[] sizes = sizesDictionary[leaf.OpenType];

							if (w_F < sizes[0] || w_F > sizes[1] || h_F < sizes[2] || h_F > sizes[3])
							{
								if (restriction)
									list.Add(new SiegeniaClassicRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры {6} створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
										w_F, h_F, sizes[0], sizes[1], sizes[2], sizes[3], keepRussian(leaf.OpenType))));
							}
							else
							{
								// частности
								if (leaf.OpenType == OpenType.Поворотное && w_F > 700 && h_F < 490)
								{
									if (restriction)
										list.Add(new SiegeniaClassicRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, при ширине > 700 по фальцу высота не может быть менне 490 мм по фальцу", w_F, h_F)));
								}

								if (leaf.OpenType == OpenType.Поворотное && w_F <= 700 && h_F > 2360)
								{
									if (restriction)
										list.Add(new SiegeniaClassicRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, при ширине < 700 по фальцу высота не может быть более 2360 мм по фальцу", w_F, h_F)));
								}
							}
						}
						else
						{
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("тип открывания {0} не предусмотрен", leaf.OpenType)));
						}
					}
				}

				return list;
			}

			private static readonly Dictionary<OpenType, int[]> sizesDictionary = new Dictionary<OpenType, int[]>();

			static SiegeniaClassicRestriction()
			{
				sizesDictionary.Add(OpenType.Поворотно_откидное, new int[] {290, 1260, 490, 2400});
				sizesDictionary.Add(OpenType.Поворотное, new int[] {290, 1560, 200, 2400}); // без частностей
				sizesDictionary.Add(OpenType.Откидное, new int[] {200, 2360, 250, 700});
				sizesDictionary.Add(OpenType.Подвесное, new int[] {200, 2360, 250, 700});
			}

			private static string keepRussian(OpenType openType)
			{
				switch (openType)
				{
					case OpenType.Поворотное:
						return "поворотной";
					case OpenType.Поворотно_откидное:
						return "поворотно-откидной";
					case OpenType.Откидное:
						return "откидной";
					case OpenType.Подвесное:
						return "подвесной";
					default:
						return openType.ToString();
				}
			}
		}

		private class SiegeniaAxxentRestriction : Invalidance
		{
			private const string System = SiegeniaAxxent;
			private readonly string _message;

			public SiegeniaAxxentRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				if (leaf != null)
					return string.Format("{0} {1} {2}", leaf.Name, leaf.Model.FurnitureSystem.Name, _message);

				clsModel model = subject as clsModel;
				if (model != null)
					return string.Format("{0} {1}", model.FurnitureSystem.Name, _message);

				return _message;
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			private static readonly List<string> profSystemsToUse = new List<string>(new string[]
				{
				ProfSystBEZ,
				ThermoLock,
				Classic,
				Solar,
				Rehau70,
				Vario,
				Thermo70,
				Thermo76,
				Estet,
				EVO,
				DECOR,
				NEO_80,
				FavoritSpace
				});

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// родная фурнитурная система
				if (model.FurnitureSystem.Name == System && model.Leafs.Count > 0)
				{
					// ограничения
					bool restriction = RunCalc.getRestriction(model);

					// родная профильная стситема
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new SiegeniaAxxentRestriction(model, string.Format("не доступна в профильной системе {1}", System, model.ProfileSystem.Name)));

					// пробег по створкам
					foreach (clsLeaf leaf in model.Leafs)
					{
						// токо прямоуг = roto OK, если не канает тогда делаем ззамену на roto NT, но в модели написано что это Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (leaf.Count != 4 || beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								list.Add(new SiegeniaAxxentRestriction(leaf, "только прямогуольные створки"));
								break;
							}
						}

						// нельзя поставить на T створку 
						if (leaf.OpenView == OpenView.Наружу)
							list.Add(new SiegeniaAxxentRestriction(leaf, "не ставиться на створки T"));

						// штульповая створка только пов, активная только п/о
						switch (leaf.ShtulpOpenType)
						{
							case ShtulpOpenType.ShtulpOnLeaf:
								if (leaf.OpenType != OpenType.Поворотное)
									list.Add(new SiegeniaAxxentRestriction(leaf, "пассивная штульповая створка только поворотне открывание"));
								break;
							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.OpenType != OpenType.Поворотно_откидное)
									list.Add(new SiegeniaAxxentRestriction(leaf, "активная штульповая створка только поворотно-откидное открывание"));
								break;
						}

						// тольо центральное положение ручки, инвариантом не делать - может захотят поменять систему
						// дерево в курсе константного пложения тоже, но оно не проверялось
						if (leaf.HandlePositionType != HandlePositionType.Центральное)
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("доступно только {0} положение ручки", HandlePositionType.Центральное)));

						// лечение бага wd-4
						if (!leaf.HandlePosition.IsAutomat)
						{
							leaf.HandlePosition.IsAutomat = true;
							leaf.HandlePosition.HandlePosition = 0;
						}

						// тольо оконная ручка в модели и в userParam
						if (leaf.HandleType != HandleType.Оконная && leaf.HandleType != HandleType.Нет_ручки)
							list.Add(new SiegeniaAxxentRestriction(leaf, "доступны только Оконные ручки"));

						clsUserParam ht = leaf.UserParameters.GetParam("Тип ручки");
						if (ht != null && ht.StringValue != "Оконная ручка")
							list.Add(new SiegeniaAxxentRestriction(leaf, "доступны только Оконные ручки"));

						// более 2 петель только на откидных и поворотных
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("{0} петели можно установить толька на Поворотные и Откидные створки", leaf.HingeCount)));

						// --WK1
						// clsUserParam wk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
						// if (wk1 != null && wk1.StringValue != wk1.DefaultValue.StrValue)
						//	list.Add(new SimpleInvariant(model, "доступна только Основная Комплектация фурнитуры", delegate
						// 		{
						//			wk1.StringValue = wk1.DefaultValue.StrValue;
						//		}));

						// только белый цвет накладок ибо их нет
						clsUserParam dekas = model.UserParameters.GetParam("Накладки на петли");
						if (dekas != null && dekas.StringValue != "Белый" && dekas.StringValue != "Коричневый" && dekas.StringValue != "Без накладок")
							if (restriction)
								list.Add(new SimpleInvariant(model, string.Empty, delegate { dekas.StringValue = dekas.DefaultValue.StrValue; }));

						// размеры
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						if (sizesDictionary.ContainsKey(leaf.OpenType))
						{
							int[] sizes = sizesDictionary[leaf.OpenType];

							// крупнозернисто
							if (w_F < sizes[0] || w_F > sizes[1] || h_F < sizes[2] || h_F > sizes[3])
							{
								if (restriction)
									list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены предельные размеры {6} створки: ширина {2}-{3}, высота {4}-{5} по фальцу",
										w_F, h_F, sizes[0], sizes[1], sizes[2], sizes[3], keepRussian(leaf.OpenType))));
							}
							else
							{
								// частности, штульповое до 1450 по ширине
								const int ws_F = 1450;
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && w_F > ws_F)
								{
									list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("ширина штульповой створки не более {0}", ws_F)));
								}
							}
						}
						else
						{
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("тип открывания {0} не предусмотрен", leaf.OpenType)));
						}

						// автомат по количеству петлей
						if (leaf.OpenType == OpenType.Откидное || leaf.OpenType == OpenType.Подвесное)
						{
							leaf.HingeCount = w_F <= 1000 ? 2 : w_F <= 1600 ? 3 : 4;
						}
					}
				}

				return list;
			}

			private static readonly Dictionary<OpenType, int[]> sizesDictionary = new Dictionary<OpenType, int[]>();

			static SiegeniaAxxentRestriction()
			{
				/// [0] min_w, [1] max_w, [2] min_h, [3] max_h                 /// todo refactor: explicit semantic is goodness
				sizesDictionary.Add(OpenType.Поворотно_откидное, new int[] {380, 1650, 460, 3000});
				sizesDictionary.Add(OpenType.Поворотное, new int[] {380, 1650, 460, 3000});
				sizesDictionary.Add(OpenType.Откидное, new int[] {400, 2400, 500, 1600});
				sizesDictionary.Add(OpenType.Подвесное, new int[] {400, 2400, 500, 1600});
			}

			private static string keepRussian(OpenType openType)
			{
				switch (openType)
				{
					case OpenType.Поворотное:
						return "поворотной";
					case OpenType.Поворотно_откидное:
						return "поворотно-откидной";
					case OpenType.Откидное:
						return "откидной";
					case OpenType.Подвесное:
						return "подвесной";
					default:
						return openType.ToString();
				}
			}
		}

		private class SimpleInvariant : Invariant
		{
			public delegate void FixDelegate();

			private readonly FixDelegate fixDelegate;
			private readonly string _message;

			public override string message()
			{
				return _message;
			}

			public override void fix()
			{
				fixDelegate();
			}

			// delagate driven fixable invariant in legacy manner
			public SimpleInvariant(clsModel model, string message, FixDelegate fixDelegate) : base(model)
			{
				_message = message;
				this.fixDelegate = fixDelegate;
			}

			private clsModel model
			{
				get { return (clsModel) subject; }
			}
		}


		// Аспект типа заказа
		private class OrderTypeRestriction : Invalidance
		{
			// private static readonly List<string> profSystemList = new List<string>(new string[] { ProfSystEco60, ProfSystEuroDesign });

			private readonly string _message;

			public OrderTypeRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				return _message;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// для всех П/С
				// свой профиль
				// if (!profSystemList.Contains(model.ProfileSystem.Name))
				//    return list;

				const int idddocoperKrupn = 3;
				const int idddocoperPred = 33;
				const int idddocoperDA = 67;
				int? iddocoper = getiddocoper(model);

				// TODO ТУТ БУДЕТ КАКАЯ ИНАЯ ЛОГИКА
				//                if (iddocoper != idddocoperKrupn && iddocoper != idddocoperPred && iddocoper != idddocoperDA && iddocoper != 4)
				//                    list.Add(new OrderTypeRestriction(model, string.Format("Профильная система {0} недоступна для данного типа заказа", model.ProfileSystem.Name)));

				bool restriction = RunCalc.getRestriction(model);

				if (restriction && iddocoper != idddocoperKrupn && iddocoper != 33 && model.FurnitureSystem.Name == Axor)
					list.Add(new OrderTypeRestriction(model, string.Format("{0} недоступна", model.FurnitureSystem.Name)));

				// без ручек =  дефалт != инвариант
				if (iddocoper == idddocoperKrupn)
				{
					if (model.LastAction == wdAction.AddLeaf && model.Leafs.SelectedLeafs != null && isSomeSelectedLeafHaveHandle(model))
					{
						list.Add(new SimpleInvariant(model, string.Format("По умолчанию в {0} используем створки без ручек, но можно вручную", model.ProfileSystem.Name),
							delegate
							{
								foreach (clsLeaf leaf in model.Leafs.SelectedLeafs)
								{
									leaf.HandleType = HandleType.Нет_ручки;
									leaf.UserParameters[__mechanizm].StringValue = __no;
									leaf.UserParameters["Балконная ручка"].StringValue = __no;
								}
							}));
					}
				}
				else
				{
					// на толстых створках не поставится Austin :( остльные универсальные
					if (model.ProfileSystem.Name == EVO || model.ProfileSystem.Name == NEO_80 || model.ProfileSystem.Name == Thermo76 || model.ProfileSystem.Name == Estet)
					{
						foreach (clsLeaf leaf in model.Leafs)
						{
							// не действует на белые оконные ручки
							clsUserParam upMech = leaf.UserParameters.GetParam("Механизм");
							if (upMech != null && upMech.StringValue2 == upMech.DefaultValue.StrValue2)
							{
								clsUserParam up = leaf.UserParameters.GetParam("Исполнение оконной ручки");
								if (up != null && up.StringValue == "Hoppe Austin")
								{
									list.Add(new OrderTypeRestriction(model, string.Format("{0} = {1} не доступно в системе {2} из-за недостаточной длины штифта ручки", up.Name, up.StringValue, model.ProfileSystem.Name)));
								}
							}
						}
					}
				}

				return list;
			}

			private static bool isSomeSelectedLeafHaveHandle(clsModel model)
			{
				foreach (clsLeaf leaf in model.Leafs.SelectedLeafs)
				{
					if (leaf.HandleType != HandleType.Нет_ручки)
						return true;
				}

				return false;
			}
		}

		private class EuroSibOptimaMaximaRestriction : Invalidance
		{
			private static readonly List<string> profSystemList = new List<string>(new string[] {EuroDesign, SibDesign, RehauOptima, RehauMaxima});
			private static readonly List<string> furnSystemList60 = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, Vorne /*, Axor*/});
			private static readonly List<string> furnSystemList60bis = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, Vorne});
			private static readonly List<string> furnSystemList70 = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan});
			private static readonly List<string> furnSystemListDoor = new List<string>(new string[] {FurnSystDver});
			// todo испоьзовние Vorne ограничиено в типе заказа

			private readonly string _message;

			// todo DRY violation
			private EuroSibOptimaMaximaRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				// подмена З-1 на Г-1 ибо на картинке показана Г-1
				clsRegion region = subject as clsRegion;
				return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// свой профиль
				if (!profSystemList.Contains(model.ProfileSystem.Name))
					return list;

				bool restriction = RunCalc.getRestriction(model);

				//				if(restriction && model.ProfileSystem.Name == SibDesign)
				//				{
				//					list.Add(new EuroSibOptimaMaximaRestriction(model, string.Format("Профильная система {0} недоступна", model.ProfileSystem.Name)));
				//					return list;
				//				}

				// фурнитура
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					bool optimaTitan = isOptimaTitan(model);

					List<string> furnSystemList = model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon ? model.ProfileSystem.Name == SibDesign || model.ProfileSystem.Name == RehauMaxima ? furnSystemList70 : optimaTitan ? furnSystemList60bis : furnSystemList60 : furnSystemListDoor;

					if (restriction)
					{
						if (!furnSystemList.Contains(model.FurnitureSystem.Name))
							list.Add(new SimpleInvariant
								(model,
								string.Format("В профсистеме {0} для типа конструкции {1} используется {2}", model.ProfileSystem.Name, model.ConstructionType.Name, string.Join(", ", furnSystemList.ToArray())),
								delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
								));
					}
				}

				const string upShtapicName = "Штапик";
				const string upColorName = "Цвет уплотнений";
				clsUserParam upShtapic = model.UserParameters.GetParam(upShtapicName);
				clsUserParam upColor = model.UserParameters.GetParam(upColorName);

				/// 
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness > 0)
					{
						colProfile filtered = filterByThicknessOrderByNumpos(model, region.Fill.Thikness);
						if (filtered.Count > 0)
						{
							if (!listed(filtered, upShtapic.StringValue, upColor.StringValue) && restriction)
							{
								clsProfile profile = filtered[0];

								list.Add(new SimpleInvariant(
									model,
									string.Format("В профсистеме {0} используется для заполнения толщиной {1}мм иcпользуется шатпик {2} {3}", model.ProfileSystem.Name, region.Fill.Thikness, getShtapic(profile), getColor(profile)), delegate
									{
										model.UserParameters[upShtapicName].StringValue = getShtapic(profile);
										model.UserParameters[upColorName].StringValue = getColor(profile);
									}));
							}
						}
						else
						{
							List<int> thicknesses = getAvaliableThicknesses(model);
							if (thicknesses.Count > 0)
							{
								int thickness = thicknesses[0];
								// todo refactor
								// на скорую руку, но в целом яно что требуется, засады будут если дефолтово не минимальная и т.п.
								clsFill fill = thickness <= 24 ? model.ProfileSystem.Fills["4х16хИ4"] : model.ProfileSystem.Fills["4х24хИ4"];

								// make clojurable
								clsRegion region1 = region;
								string s = string.Join(", ", thicknesses.ConvertAll<string>(delegate(int input) { return input.ToString(); }).ToArray());
								list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} можно использовать только заполнения толщиной {1}мм", model.ProfileSystem.Name, s), delegate { region1.Fill = fill; }));
							}
							else
							{
								list.Add(new EuroSibOptimaMaximaRestriction(region, string.Format("не найдено доступных заполнений", model.ProfileSystem.Name, region.Fill.Thikness)));
							}
						}
					}
				}

				return list;
			}

			// ИБАНОЕ БЛЪДСТВО // пробой слоя = ограничения модели зависят от продавца а среди продавцов ИБАНЫЙ БАРДАК = 
			// одна сущьность имеет несколько разных id ради переменчивой вложеннои невыделенной сущности
			private static bool isOptimaTitan(clsModel model)
			{
				int idseller = getIdseller(model);
				object[] objects = CalcProcessor.Modules["isOptimaTitan"](new object[] {idseller});
				return objects != null && objects.Length > 0 && objects[0] is bool && (bool) objects[0];
			}

			private static List<int> getAvaliableThicknesses(clsModel model)
			{
				List<int> list = new List<int>();
				foreach (clsProfile profile in model.ConstructionType.Shtapik)
				{
					// todo refactor [F1,F2] это интревал, а поле Толщина (Thickness) тут не видно :((
					if (!list.Contains((int) profile.F1))
						list.Add((int) profile.F1);

					if (!list.Contains((int) profile.F2))
						list.Add((int) profile.F2);
				}

				list.Sort();

				return list;
			}

			private static string getShtapic(clsProfile profile)
			{
				return profile.Steels.Count > 0 ? profile.Steels[0] : string.Empty;
			}

			private static string getColor(clsProfile profile)
			{
				return profile.Comment;
			}


			private static bool listed(colProfile filtered, string shtapic, string color)
			{
				foreach (clsProfile profile in filtered)
				{
					if (getShtapic(profile) == shtapic && getColor(profile) == color)
						return true;
				}

				return false;
			}

			private static colProfile filterByThicknessOrderByNumpos(clsModel model, int thikness)
			{
				colProfile profiles = new colProfile();
				foreach (clsProfile profile in model.ConstructionType.Shtapik)
				{
					if (profile.F1 <= thikness && thikness <= profile.F2)
					{
						profiles.Add(profile);
					}
				}

				return profiles; // todo order ??? .Sort((p1, p2) => p1.);
			}
		}


		// так можно по ID или по имени
		// 45	Конструкция по ручному расчёту
		// 46	Ворота (ручной расчет)
		// 47	Калитка (ручной расчет)
		//        private static readonly ConstructionType manualConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("Конструкция по ручному расчёту");
		//        private static readonly ConstructionType gateConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("Ворота (ручной расчет)");
		//        private static readonly ConstructionType wicketConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("Калитка (ручной расчет)");

		///
		/// *** новый Титан
		/// 
		private class TitanRestriction : Invalidance
		{
			private static readonly List<string> profSystemsToUse = new List<string>(new string[]
				{
				ProfSystBEZ,
				Rehau60,
				ThermoLock,
				Classic,
				DECOR,

				RehauEuro70,
				Rehau70,
				Thermo70,
				Solar,
				Vario,
				EVO,

				EuroDesign,
				SibDesign,
				RehauOptima,
				RehauMaxima,
				RehauDeluxe,
				RehauBlitzNew,
				RehauGrazio,

				Gealan,

				Thermo76, Estet, NEO_80,

				FavoritSpace, FORWARD, BAUTEK, BAUTEKmass, 

				SCHTANDART_PREMIUM
				});

			private readonly string _message;

			public TitanRestriction(object subj, string message)
				: base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				clsLeaf leaf = subject as clsLeaf;
				if (leaf != null)
					return string.Format("{0} {1} {2}", leaf.Name, leaf.Model.FurnitureSystem.Name, _message);

				clsModel model = subject as clsModel;
				return model != null ? string.Format("{0} {1}", model.FurnitureSystem.Name, _message) : _message;
			}

			public override string uniq()
			{
				return base.uniq() + _message;
			}

			public static IEnumerable<Invalidance> test(clsModel model)
				{
				List<Invalidance> list = new List<Invalidance>(0);

				// родная фурнитурная система
				if (model.FurnitureSystem.Name != TitanDebug && model.FurnitureSystem.Name != SiegeniaTitan && model.FurnitureSystem.Name != SiegeniaTitanWK1)
					return list;

				if (model.Leafs.Count == 0)
					return list;

				// ограничения
				bool restriction = getRestriction(model);

				// родная профильная стситема
				if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
					list.Add(new TitanRestriction(model, string.Format("не доступна в профильной системе {0}", model.ProfileSystem.Name)));

				// пробег по створкам
				foreach (clsLeaf leaf in model.Leafs)
					{
					bool wk1 = getWk1(leaf);

					// варио тольо WK1
					if (model.ProfileSystem.Name == Vario && !wk1)
						list.Add(new SimpleInvariant(model, string.Format("в профильной системе {0} используется только противовзломное исполнение", model.ProfileSystem.Name), delegate
							{
								clsUserParam upWk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
								if (upWk1 != null)
									upWk1.StringValue = "Противовзломная";
							}));

					// только центральное или константное положение ручки
					if (leaf.HandlePositionType != HandlePositionType.Центральное && leaf.HandlePositionType != HandlePositionType.Фиксированное)
						list.Add(new TitanRestriction(leaf, string.Format("доступно только {0} или {1} положение ручки", HandlePositionType.Центральное, HandlePositionType.Фиксированное)));

					// DM8 только центральное положение ручки
					if (leaf.HandleBeem != null && leaf.HandleBeem.Profile.C <= 50 && leaf.HandlePositionType != HandlePositionType.Центральное)
						list.Add(new TitanRestriction(leaf, string.Format("для DM8 доступно только {0} положение ручки", HandlePositionType.Центральное)));

					// DM25 личинка ставится только при длинне запорной балки по фальцу >1200 // приводя.pdf стр. 6.2.30 - 6.2.34
					int TGMK0520 = 1200;
					if (leaf.HandleBeem != null && leaf.HandleBeem.Profile.C > 60 && leaf.HandleBeem.LineE.Length <= TGMK0520 && leaf.UserParameters[__mechanizm].StringValue.ToLower().Contains("ключ"))
						list.Add(new TitanRestriction(leaf, string.Format("для створок DM25 личинка замка (ключ) устанавливается при ВСФ > {0}мм", TGMK0520)));

					// штульповая створка только пов, активная только п/о
					switch (leaf.ShtulpOpenType)
					{
						case ShtulpOpenType.ShtulpOnLeaf:
							if (leaf.OpenType != OpenType.Поворотное)
								list.Add(new TitanRestriction(leaf, "пассивная штульповая створка только поворотное открывание"));
							break;
						case ShtulpOpenType.NoShtulpOnLeaf:
							if (leaf.OpenType != OpenType.Поворотно_откидное && restriction)
								list.Add(new TitanRestriction(leaf, "активная штульповая створка только поворотно-откидное открывание"));
							break;
					}

					// более 2 петель только на откидных и поворотных
					if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
						list.Add(new TitanRestriction(leaf, string.Format("{0} петели можно установить толька на Поворотные и Откидные створки", leaf.HingeCount)));

					//                    // тольо оконная ручка в модели и в userParam
					//                    if (leaf.HandleType != HandleType.Оконная && leaf.HandleType != HandleType.Нет_ручки)
					//                        list.Add(new SiegeniaClassicRestriction(leaf, "доступны только Оконные ручки"));
					//
					//                    clsUserParam ht = leaf.UserParameters.GetParam("Тип ручки");
					//                    if (ht != null && ht.StringValue != "Оконная ручка")
					//                        list.Add(new SiegeniaClassicRestriction(leaf, "доступны только Оконные ручки"));


					// размеры
					int hF = leaf.BaseRegion.Lines[0].HeightInt;
					int wF = leaf.BaseRegion.Lines[0].WidthInt;

					// ШСФ/ВСФ <=1.5 опасно отменять это ограничение ибо на нем держатся некоторые угловые в штульповом исполнении стандартных окон
					if (leaf.OpenType == OpenType.Поворотно_откидное || leaf.OpenType == OpenType.Поворотное)
					{
						if ((double) wF / (double) hF > 1.5)
							if (restriction)
								list.Add(new TitanRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушено ограничение по соотношению сторон, ширина не должна превышать высоту более чем в 1,5 раза", wF, hF)));
					}

					/// Топология
					/// 
					/// стандарт
					if (isStandart(leaf))
					{
						// виды открывания
						if (leaf.OpenType != OpenType.Поворотно_откидное && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
						{
							list.Add(new TitanRestriction(leaf, string.Format("тип открывания {0} не предусмотрен", leaf.OpenType)));
						}
						else
						{
							int[] limits = getLimits(leaf);

							if (limits != null && (wF > limits[2] || hF > limits[3]))
							{
								if (restriction)
									list.Add(new TitanRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены максимальные размеры {7} {6} створки {9}: ширина до {4}, высота до {5} по фальцу",
										wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
										)));
							}

							if (limits != null && (wF < limits[0] || hF < limits[1]))
							{
								if (restriction)
									list.Add(new TitanRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены минимальные размеры {7} {6} створки DM{8} {9}: ширина от {2}, высота от {3} по фальцу",
										wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
										)));
							}
						}
					}
					else
						{
						/// нестандарт, играем так 1)открывание, 2)количество вершин, 3)углы радиусы, 
						/// опираемся на :
						/// 1) todo одно направление одна балка // еще не реализовано
						/// 2) todo только выпуклые многоугольники
						switch (leaf.OpenType)
						{
							case OpenType.Поворотное:
								// поворотное штульповое 4-ёх угольное канает по П/О вараинту
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.Count == 4)
									{
									goto case OpenType.Поворотно_откидное;
									}

								// todo
								if (restriction)
								list.Add(new TitanRestriction(leaf, string.Format("не реализовано {0} (временно) попробуйте П/О открывание", leaf.OpenType)));

								break;

							case OpenType.Поворотно_откидное:

							//if (leaf.OpenType == )

							// петлевая вертикаль
							if (!isVertical(leaf.HingBeem))
							{
							list.Add(new TitanRestriction(leaf, "петлевая балка не вертикальна"));
							break;
							}

							// низ гоизонт жостко, требуем наличия горизонтали 
							clsBeem bottom = getFirst(leaf, ItemSide.Bottom);
							if (bottom == null)
							{
							list.Add(new TitanRestriction(leaf, "основание не горизонтально"));
							break;
							}

							// основание и петлевая должно быть соединено под прямым углом
							if (getNextBottom(leaf.HingBeem) != bottom)
							{
							list.Add(new TitanRestriction(leaf, "основание и петлевая должно быть соединено под прямым углом"));
							break;
							}

							/// Qu = 3 // треугольное 
							if (leaf.Count == 3)
							{
							// ручка наклонная
							if (leaf.HandleBeem.PositionBeem != ItemSide.Other)
							{
							list.Add(new TitanRestriction(leaf, "балка ручки не наклонная"));
							}

							// угол ручки наклонной 35-45
							if (restriction)
							{
							const int minA = 35;
							const int maxA = 45;
							double a = getBeemAngleOtherHorizont(leaf.HandleBeem);
							if (a < minA || a > maxA)
							{
							list.Add(new TitanRestriction(leaf, string.Format("угол наклона балки ручки = {0} градусов, вне допустимых {1}-{2}", a, minA, maxA)));
							}
							}

							// радиусность
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "радиусная балка в треугольнке"));
							}

							// размеры
							addLimitsError(list, leaf, new int[] {575, 575, 1200, 1200});
							}
							else if (leaf.Count == 4)
							{
							//Label: 
							if (getNextBottom(leaf.HandleBeem) != bottom)
							{
							list.Add(new TitanRestriction(leaf, "балка ручки не соединена с основанием"));
							}

							// разные топологии наклона ручки
							if (isVertical(leaf.HandleBeem))
							{
							/// вертикальая балка ручки

							// угол наклонной 15-165 к вертикали => 0-45 к горизонту
							foreach (clsBeem beem in leaf)
							{
							if (beem.PositionBeem != ItemSide.Other)
							continue;

							const int minA = 0;
							const int maxA = 60;
							double a = getBeemAngleOtherHorizont(beem);
							if (a < minA || a > maxA)
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("угол наклона балки = {0} градусов к горизонтали, вне допустимых {1}-{2}", a, minA, maxA)));
							}
							}

							// допустима 1 радиусная балка вверху или other
							if (getRadiusQu(leaf) > 1)
							{
							list.Add(new TitanRestriction(leaf, "не более одной радиусной балки"));
							// далее уже можно не проверять, выходим из switch (leaf.OpenType)
							break;
							}

							clsBeem ark = getRadiusFirst(leaf);
							if (ark != null && ark.PositionBeem != ItemSide.Top && ark.PositionBeem != ItemSide.Other)
							{
							list.Add(new TitanRestriction(leaf, "радиусная балка доступна только в верхнем или наклонном положении"));
							// далее уже можно не проверять, выходим из switch (leaf.OpenType)
							break;
							}

							if (ark != null && ark.R1 < 0)
							{
							list.Add(new TitanRestriction(leaf, "вогнутая радиусная балка"));
							}

							// размеры
							if (addLimitsError(list, leaf, new int[] {420, 460, ark != null ? 1000 : 1200, 2400})) ; // 900 правильно
							{
							// косое мин петлевая 815
							if (ark == null)
							{
							const int min = 815;
							if (leaf.HingBeem.LineE.LengthInt < min)
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("косое, минимальная длинна петлевой балки {0}", min)));
							}

							// мин балка ручки
							int minHandleF = leaf[0].Profile.C <= 60 ? 260 : 600;
							if (leaf.HandleBeem != null && leaf.HandleBeem.LineE.LengthInt < minHandleF)
							{
							list.Add(new TitanRestriction(leaf, string.Format("косое, минимальная длинна балки с ручкой {0}", minHandleF)));
							}
							}
							else
							{
							int a = (int) Math.Round(leaf.HandleBeem.LGabarit.AngelBeetwenLines(ark.LGabarit));
							if (a == 180)
							{
							// циркульное соединение 
							const int min = 510;
							if (leaf.HandleBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("циркуль, минимальная длинна балки ручки {0}", min)));
							}
							else
							{
							// угловое 
							const int min = 550;
							if (leaf.HandleBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("угловое, минимальная длинна балки ручки {0}", min)));
							}

							int b = (int) Math.Round(leaf.HingBeem.LGabarit.AngelBeetwenLines(ark.LGabarit));
							if (b == 180)
							{
							// циркульное соединение 
							const int min = 495;
							if (leaf.HingBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("циркуль, минимальная длинна петлевой балки {0}", min)));
							}
							else
							{
							// угловое 
							const int min = 565;
							if (leaf.HingBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("угловое, минимальная длинна петлевой балки {0}", min)));
							}
							}
							}
							}
							else
							{
							// верх = горизонтально, должен касацца петлевой 
							clsLeafBeem top = getFirst(leaf, ItemSide.Top);
							if (top == null)
							{
							list.Add(new TitanRestriction(leaf, "верхняя балка не горизонтальная"));
							break;
							}

							// радиусные лесом
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "радиусная балка в неположенном месте"));
							// далее уже можно не проверять, выходим из switch (leaf.OpenType)
							break;
							}

							// размеры
							if (addLimitsError(list, leaf, new int[] {420, 460, 1200, 2400}))
							{
							const int min = 230;
							if (top.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("косое, минимальная длинна балки ножниц {0}", min)));
							}
							}
							}
							else if (leaf.Count == 5)
							{
							// ручка соединена с горизонтальным оснванием под прямым углом
							if (getNextBottom(leaf.HandleBeem) != bottom || !isVertical(leaf.HandleBeem))
							{
							list.Add(new TitanRestriction(leaf, "балка ручки должная бвть соединена с основанием под прямым улом"));
							break;
							}

							// радиусные лесом
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "радиусная балка в неположенном месте"));
							}

							// мин балка ручки
							int minHandleF = leaf[0].Profile.C <= 60 ? 260 : 600;
							if (leaf.HandleBeem != null && leaf.HandleBeem.LineE.LengthInt < minHandleF)
							{
							list.Add(new TitanRestriction(leaf, string.Format("косое, минимальная длинна балки с ручкой {0}", minHandleF)));
							}

							// мин петлевая балка основано на мин длине TSKR1050
							const int minHingeF = 485;
							if (leaf.HingBeem != null && leaf.HingBeem.LineE.LengthInt < 485)
							{
							list.Add(new TitanRestriction(leaf, string.Format("косое, минимальная длинна петлевой балки {0}", minHingeF)));
							}

							// размеры
							addLimitsError(list, leaf, new int[] {420, 460, 1200, 2400});
							}
							else
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("нестандарт {0} {1} сторон не предусмотрен", leaf.OpenType, leaf.Count)));
							break;
							}

							break;

							case OpenType.Откидное:
							case OpenType.Подвесное:

							// требуем горизонтальна петли
							if (leaf.HingBeem.PositionBeem != ItemSide.Bottom && leaf.HingBeem.PositionBeem != ItemSide.Top)
							{
							list.Add(new TitanRestriction(leaf, string.Format("петлевая балка не горизонтальна")));
							break;
							}

							if (leaf.Count <= 4)
							{
							// размеры
							addLimitsError(list, leaf, new int[] {200, 200, 2400, 800});

							// длинна ручки на трапецци не менее 410
							if (leaf.HandleBeem.LineE.Length < 510)
							list.Add(new TitanRestriction(leaf, string.Format("Длина балки ручки не менне 510 мм")));

							break;
							}
							else
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("нестандарт {0} {1} вершин не предусмотрен", leaf.OpenType, leaf.Count)));
							break;
							}

							default:
							list.Add(new TitanRestriction(leaf, string.Format("тип открывания {0} не предусмотрен", leaf.OpenType)));
							break;
							}
						}
					}

				return list;
				}

			private static bool addLimitsError(List<Invalidance> list, clsLeaf leaf, int[] limits)
			{
			int hF = leaf.BaseRegion.Lines[0].HeightInt;
			int wF = leaf.BaseRegion.Lines[0].WidthInt;
			bool restriction = getRestriction(leaf.Model);

			if (wF < limits[0] || hF < limits[1])
			if (restriction)
			{
			list.Add(new TitanRestriction(leaf, string.Format("ширина {0} x высота {1} по фальцу, нарушены минимальные размеры {6} створки: ширина от {2}, высота от {3} по фальцу",
			wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
			)));
			return false;
			}

			;

			if (wF > limits[2] || hF > limits[3])
			if (restriction)
			{
			list.Add(new TitanRestriction(leaf, string.Format("ширина {0} x высота {1} по фальцу, нарушены максимальные размеры {6} створки: ширина до {4}, высота до {5} по фальцу",
			wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
			)));
			return false;
			}

			return true;
			}

			private static clsLeafBeem getRadiusFirst(clsLeaf leaf)
			{
			foreach (clsLeafBeem beem in leaf)
			{
			if ((int) beem.R1 != 0)
			return beem;
			}

			return null;
			}

			private static int getRadiusQu(clsLeaf leaf)
			{
			int qu = 0;
			foreach (clsBeem beem in leaf)
			if ((int) beem.R1 != 0)
			qu++;

			return qu;
			}

			private static clsLeafBeem getFirst(clsLeaf leaf, ItemSide side)
			{
			foreach (clsLeafBeem beem in leaf)
			{
			if (beem.PositionBeem == side)
			return beem;
			}

			return null;
			}

			private static int getBeemAngleOtherHorizont(clsBeem leafBeem)
			{
			return (int) Math.Round(leafBeem.AngleView < 90 ? leafBeem.AngleView : 180 - leafBeem.AngleView);
			}

			private static clsLeafBeem getNextBottom(clsLeafBeem leafBeem)
			{
			if (leafBeem.Beem1.PositionBeem == ItemSide.Bottom)
			return (clsLeafBeem) leafBeem.Beem1;
			if (leafBeem.Beem2.PositionBeem == ItemSide.Bottom)
			return (clsLeafBeem) leafBeem.Beem2;
			return null;
			}

			private static bool isVertical(clsLeafBeem leafBeem)
			{
			return leafBeem.PositionBeem == ItemSide.Left || leafBeem.PositionBeem == ItemSide.Right;
			}

			private static bool isStandart(clsLeaf leaf)
			{
			foreach (clsBeem beem in leaf)
			{
			if (leaf.Count != 4 || beem.PositionBeem == ItemSide.Other || (int) beem.R1 != 0 || (int) beem.R2 != 0)
			{
			return false;
			}
			}

			return true;
			}

			private static int[] getLimits(clsLeaf leaf)
			{
			bool wk1 = getWk1(leaf);

			switch (leaf.OpenType)
			{
			case OpenType.Поворотно_откидное:
			case OpenType.Поворотное:
			// безопасные размеры без частностей
			int[] limits = new int[] {330, 460, 1560, 2400};

			/// модификаторы крайних положений

			// в дереве используется .C размер 15-50 / 51-60 / 61-120 = DM8 / DM15 / DM25
			// leaf[0].Profile.C;
			// DM8-15 leaf.Dornmas <= 15
			if (leaf[0].Profile.C <= 60)
			{
			// одиночная створка
			if (leaf.ShtulpOpenType == ShtulpOpenType.NoShtulp)
			{
			int hF = leaf.BaseRegion.Lines[0].HeightInt;
			int wF = leaf.BaseRegion.Lines[0].WidthInt;

			// разворот 4020 вертикально кроме WK1
			// п/о от 461 и * поворотные можно сделать ШСФ от 230
			if (!wk1 && leaf.HandlePositionType == HandlePositionType.Центральное && (hF >= 461 || leaf.OpenType == OpenType.Поворотное))
			limits[0] = 230;

			// поворотное-откидное
			if (leaf.OpenType == OpenType.Поворотно_откидное)
			{
			// центральное 260, используя TGMK4010+5090+4020 но не WK1 изза TEUL4200 по oсевой / костантное 310 = TGKK3010+4020 и WK1
			limits[1] = leaf.HandlePositionType == HandlePositionType.Центральное && !wk1 ? 260 : 310;
			}
			// поворотное
			else
			{
			// центральное 230 используя AGMD1150 / костантное 300
			limits[1] = leaf.HandlePositionType == HandlePositionType.Центральное ? 230 : 300;
			}
			}
			// штульповые любые не могут быть ниже 310 изза использования TGKS0010 + 4020
			else
			{
			limits[1] = 310;
			}
			}
			// DM25++
			else
			{
			// можно до 3000 по высоте, остальным тож еможно но со снятыми ограничениями
			limits[3] = 3000;

			// низ от 600, кроме константного у которого 8000
			limits[1] = leaf.HandlePositionType == HandlePositionType.Центральное ? 600 : 800;
			}


			return limits;

			case OpenType.Откидное:
			case OpenType.Подвесное:
			return new int[] {200, 200, 2400, 800};
			}

			return null;
			}

			private static bool getWk1(clsLeaf leaf)
			{
			clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("Комплектация фурнитуры");
			return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
			}

			private static string keepRussian(OpenType openType)
			{
			switch (openType)
			{
			case OpenType.Поворотное:
			return "поворотной";
			case OpenType.Поворотно_откидное:
			return "поворотно-откидной";
			case OpenType.Откидное:
			return "откидной";
			case OpenType.Подвесное:
			return "подвесной";
			default:
			return openType.ToString();
			}
			}


			private static string keepRussian(ShtulpOpenType leafShtulpOpenType)
			{
			switch (leafShtulpOpenType)
			{
			case ShtulpOpenType.NoShtulp:
			return string.Empty;
			case ShtulpOpenType.NoShtulpOnLeaf:
			return "активной штульповой";
			case ShtulpOpenType.ShtulpOnLeaf:
			return "пассивной штульповой";
			default:
			return string.Empty;
			}
			}
		}

		///
		private class RotoNTRestriction : Invalidance
		{
		private static readonly List<string> profSystemsToUse = new List<string>(new string[]
		{
		ProfSystBEZ,

		Pimapen,
		Rehau60,
		ThermoLock,
		Classic,
		DECOR,

		RehauEuro70,
		Rehau70,
		Thermo70,
		Thermo76,
		NEO_80,
		Solar,
		Vario,
		EVO,

		EuroDesign,
		SibDesign,
		RehauDeluxe
		});

		private static readonly List<string> designoSystemsToUse = new List<string>(new string[]
		{
		Rehau70,
		Solar,
		Vario,
		EVO,
		Thermo70,
		Thermo76,
		NEO_80,
		Gealan
		});


		private readonly string _message;

		private RotoNTRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsLeaf leaf = subject as clsLeaf;
		if (leaf != null)
		return string.Format("{0} {1} {2}", leaf.Name, leaf.Model.FurnitureSystem.Name, _message);

		clsModel model = subject as clsModel;
		return model != null ? string.Format("{0} {1}", model.FurnitureSystem.Name, _message) : _message;
		}

		public override string uniq()
		{
		return base.uniq() + _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// родная фурнитурная система
		if (model.FurnitureSystem.Name != RotoNT && model.FurnitureSystem.Name != RotoNTDesigno)
		return list;

		// ограничения
		bool restriction = getRestriction(model);

		// выключить RotoNT // Весенин 09/09/2019
		if (restriction)
		{
		list.Add(new RotoNTRestriction(model, "недоступна"));
		}

		// выключить RotoNTDesigno // Леонтьев 09/07/2019
		if (restriction && model.FurnitureSystem.Name == RotoNTDesigno)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} недоступна, замена на {1}", model.FurnitureSystem.Name, SiegeniaAxxent), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaAxxent); }));
		}

		// родная профильная стситема
		if (model.FurnitureSystem.Name == RotoNT && !profSystemsToUse.Contains(model.ProfileSystem.Name))
		list.Add(new RotoNTRestriction(model, string.Format("не доступна в профильной системе {0}", model.ProfileSystem.Name)));
		if (model.FurnitureSystem.Name == RotoNTDesigno && !designoSystemsToUse.Contains(model.ProfileSystem.Name))
		list.Add(new RotoNTRestriction(model, string.Format("не доступна в профильной системе {0}", model.ProfileSystem.Name)));

		// пробег по створкам
		foreach (clsLeaf leaf in model.Leafs)
		{
		bool wk1 = getWk1(leaf);

		// варио тольо WK1
		if (model.ProfileSystem.Name == Vario && !wk1)
		list.Add(new SimpleInvariant(model, string.Format("в профильной системе {0} используется только противовзломное исполнение", model.ProfileSystem.Name), delegate
		{
		clsUserParam upWk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
		if (upWk1 != null)
		upWk1.StringValue = "Противовзломная";
		}));
		}

		return list;
		}

		private static bool getWk1(clsLeaf leaf)
		{
		clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("Комплектация фурнитуры");
		return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
		}
		}


		/// TODO начато и брошено :( пока не эксплуатируется
		//        private class AccessoriesColorAutomat : Invalidance
		//        {
		//            private AccessoriesColorAutomat(clsModel subj, string message)
		//                : base(subj)
		//            {
		//                _message = message;
		//            }
		//
		//            private readonly string _message;
		//
		//            public override string message()
		//            {
		//                return this._message;
		//            }
		//
		//            public static IEnumerable<Invalidance> test(clsModel model)
		//            {
		//                List<Invalidance> list = new List<Invalidance>(0);
		//
		//                getWhat(model.ColorInside);
		//
		//
		//                return list;
		//            }
		//
		//            private static int getWhat(clsConstructionColor constructionColor)
		//            {
		//                Color color = Color.FromArgb(constructionColor.Index);
		//
		//                color.GetHue();
		//
		//                return 0;
		//            }
		//        }
		private class GealanRestriction : Invalidance
		{
		private const int defaulThickness = 44;
		private const string defaultFill = "4x16x4x16xИ4";
		private static readonly List<string> profSystemList = new List<string>(new string[] {Gealan});
		private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaTitan, RotoNTDesigno});

		private readonly string _message;

		private GealanRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// свой профиль
		if (!profSystemList.Contains(model.ProfileSystem.Name))
		return list;

		// фурнитура === Titan
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		list.Add(new SimpleInvariant
		(model,
		string.Format("В профсистеме {0} используется фурнитурные системы {1}", model.ProfileSystem.Name, string.Join(",", furnSystemList.ToArray())),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
		));
		}

		/// толщина заполнения === defaulThickness = 44
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != defaulThickness)
		{
		clsFill fill = getDefaultFill(model);
		if (fill != null)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model,
		string.Format("В профсистеме {0} можно использовать только заполнения {1} мм", model.ProfileSystem.Name, defaulThickness),
		delegate { region1.Fill = fill; }
		));
		}
		else
		{
		list.Add(new GealanRestriction(region, string.Format("только заполнения {0} мм", defaulThickness)));
		}
		}
		}

		// только скошенный штапик с серым уплотнением
		const string upShtapicName = "Штапик";
		const string upColorName = "Цвет уплотнений";
		const string skew = "Скошенный";
		const string grey = "Серый";
		clsUserParam upShtapic = model.UserParameters.GetParam(upShtapicName);
		clsUserParam upColor = model.UserParameters.GetParam(upColorName);
		if ((upShtapic != null && upShtapic.StringValue != skew) || (upColor != null && upColor.StringValue != grey))
		{
		list.Add(new SimpleInvariant(
		model,
		string.Format("В профсистеме {0} используется только {1} штапик и {2} цвет уплотнения", model.ProfileSystem.Name, skew, grey),
		delegate
		{
		model.UserParameters[upShtapicName].StringValue = skew;
		model.UserParameters[upColorName].StringValue = grey;
		}));
		}

		// без арок все углы стандартные
		clsBeem beem = getSkewOrArcBeem(model);
		if (beem != null)
		{
		if (getRestriction(model))
		list.Add(new GealanRestriction(beem, string.Format("{0}, арки недоступны, только стандартные углы", beem.Name)));
		}

		return list;
		}

		private static clsFill getDefaultFill(clsModel model)
		{
		return model.ProfileSystem.GetFillByMarking(defaultFill);
		}
		}

		private class ProductDefault : SimpleInvariant
		{
		private static readonly List<string> products = new List<string>(new string[] {ThermoLock, Thermo70, Thermo76, Estet, Solar, Vario, Classic, DECOR, RehauDeluxe, EVO, NEO_80});

		public ProductDefault(clsModel model, string message, FixDelegate fixDelegate) : base(model, message, fixDelegate)
		{
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>();

		if ((model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon) && (products.Contains(model.ProfileSystem.Name) || isSpecialCase(model)))
		{
		// включаем микропроветривание по умолчанию для каждой створки
		foreach (clsLeaf leaf in model.Leafs)
		{
		const string Да = "Да";
		const string Микропроветривание = "Микрощелевое проветривание";
		clsUserParam up = leaf.UserParameters.GetParam(Микропроветривание);
		if (up != null && up.StringValue != Да)
		{
		list.Add(new ProductDefault(model, string.Format("Включаем {0}", Микропроветривание), delegate { up.StringValue = Да; }));
		}
		}
		}

		return list;
		}

		private static bool isSpecialCase(clsModel model)
		{
		// ну вообще это бред ибо микропроветривание не функция профиля а функция фурнитуры, не да бог с им пользователи думают именно в этих категориях
		if (isPvh(model))
		{
		// пробой слоя плохо кода построитель лезет за факторами в заказ, очевидно оные факторы заказ должен сообщать заранее ибо можлива ситуация кода построить израдно буедет далек от заказа
		OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
		if (order != null && order.DocRow != null && !order.DocRow.IsidsellerNull() && order.DocRow.idseller == 457)
		return true;
		}

		return false;
		}
		}

		private class AxorRestriction : Invalidance
		{
		private static readonly List<string> profSystemsToUse = new List<string>(new string[]
		{
		RehauBlitzNew,
		EuroDesign,
		Eco60,
		FORWARD,
		BAUTEK,
		BAUTEKmass
		});

		private readonly string _message;

		public AxorRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsLeaf leaf = subject as clsLeaf;
		if (leaf != null)
		return string.Format("{0} {1} {2}", leaf.Name, leaf.Model.FurnitureSystem.Name, _message);

		clsModel model = subject as clsModel;
		return model != null ? string.Format("{0} {1}", model.FurnitureSystem.Name, _message) : _message;
		}

		public override string uniq()
		{
		return base.uniq() + _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// родная фурнитурная система
		if (model.FurnitureSystem.Name != Axor)
		return list;

		if (model.Leafs.Count == 0)
		return list;

		// ограничения
		bool restriction = getRestriction(model);

		// родная профильная стситема
		const int ПредварительныйРасчётЗаказа = 33;
		int iddocoper = getiddocoper(model) ?? 0;
		if (!profSystemsToUse.Contains(model.ProfileSystem.Name) && iddocoper != ПредварительныйРасчётЗаказа)
		list.Add(new AxorRestriction(model, string.Format("не доступна в профильной системе {0}", model.ProfileSystem.Name)));

		// пробег по створкам
		foreach (clsLeaf leaf in model.Leafs)
		{
		bool wk1 = getWk1(leaf);

		// тольо ~WK1
		if (wk1)
		list.Add(new SimpleInvariant(model, string.Format("фурнитура {0} не умеет противовзломность", model.FurnitureSystem.Name), delegate
		{
		clsUserParam upWk1 = model.UserParameters.GetParam("Комплектация фурнитуры");
		if (upWk1 != null)
		upWk1.StringValue = "Основная";
		}));

		// только центральное положение ручки
		if (leaf.HandlePositionType != HandlePositionType.Центральное)
		list.Add(new AxorRestriction(leaf, string.Format("доступно только {0} положение ручки", HandlePositionType.Центральное)));

		// штульповая створка только пов, активная только п/о
		switch (leaf.ShtulpOpenType)
		{
		case ShtulpOpenType.ShtulpOnLeaf:
		if (leaf.OpenType != OpenType.Поворотное)
		list.Add(new AxorRestriction(leaf, "пассивная штульповая створка только поворотное открывание"));
		break;
		case ShtulpOpenType.NoShtulpOnLeaf:
		if (leaf.OpenType != OpenType.Поворотно_откидное && leaf.OpenType != OpenType.Поворотное)
		list.Add(new AxorRestriction(leaf, "активная штульповая створка только поворотно-откидное или поворотное открывание"));
		break;
		}

		// более 2 петель только на откидных и поворотных
		if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
		list.Add(new AxorRestriction(leaf, string.Format("{0} петели можно установить толька на Поворотные и Откидные створки", leaf.HingeCount)));


		// размеры
		int hF = leaf.BaseRegion.Lines[0].HeightInt;
		int wF = leaf.BaseRegion.Lines[0].WidthInt;

		// ШСФ/ВСФ <=1.5 ??!!
		if (leaf.OpenType == OpenType.Поворотно_откидное || leaf.OpenType == OpenType.Поворотное)
		{
		if ((double) wF / (double) hF > 1.5)
		if (restriction)
		list.Add(new AxorRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушено ограничение по соотношению сторон, ширина не должна превышать высоту более чем в 1,5 раза", wF, hF)));
		}

		/// Топология
		/// 
		/// стандарт
		if (!isStandart(leaf))
		{
		list.Add(new AxorRestriction(leaf, string.Format("{0} только прямоугольные створки", model.FurnitureSystem.Name)));
		}

		// контроль размиров стандартной топология
		{
		// виды открывания
		if (leaf.OpenType != OpenType.Поворотно_откидное && leaf.OpenType != OpenType.Поворотное && leaf.OpenType != OpenType.Откидное && leaf.OpenType != OpenType.Подвесное)
		{
		list.Add(new AxorRestriction(leaf, string.Format("тип открывания {0} не предусмотрен", leaf.OpenType)));
		}
		else
		{
		int[] limits = getLimits(leaf);

		if (limits != null && (wF > limits[2] || hF > limits[3]))
		{
		if (restriction)
		list.Add(new AxorRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены максимальные размеры {7} {6} створки {9}: ширина до {4}, высота до {5} по фальцу",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
		)));
		}

		if (limits != null && (wF < limits[0] || hF < limits[1]))
		{
		list.Add(new AxorRestriction(leaf, string.Format("ширина {0}, высота {1} по фальцу, нарушены минимальные размеры {7} {6} створки DM{8} {9}: ширина от {2}, высота от {3} по фальцу",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
		)));
		}
		}
		}
		}

		return list;
		}

		private static bool addLimitsError(List<Invalidance> list, clsLeaf leaf, int[] limits)
		{
		int hF = leaf.BaseRegion.Lines[0].HeightInt;
		int wF = leaf.BaseRegion.Lines[0].WidthInt;
		bool restriction = getRestriction(leaf.Model);

		if (wF < limits[0] || hF < limits[1])
		if (restriction)
		{
		list.Add(new AxorRestriction(leaf, string.Format("ширина {0} x высота {1} по фальцу, нарушены минимальные размеры {6} створки: ширина от {2}, высота от {3} по фальцу",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
		)));
		return false;
		}

		;

		if (wF > limits[2] || hF > limits[3])
		if (restriction)
		{
		list.Add(new AxorRestriction(leaf, string.Format("ширина {0} x высота {1} по фальцу, нарушены максимальные размеры {6} створки: ширина до {4}, высота до {5} по фальцу",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
		)));
		return false;
		}

		return true;
		}

		private static bool isStandart(clsLeaf leaf)
		{
		if (leaf.Count != 4)
		return false;

		foreach (clsBeem beem in leaf)
		{
		if (beem.PositionBeem == ItemSide.Other || (int) beem.R1 != 0 || (int) beem.R2 != 0)
		{
		return false;
		}
		}

		return true;
		}

		/// [minX, minY, maxX, maxY]
		private static int[] getLimits(clsLeaf leaf)
		{
		bool shtup = leaf.ShtulpOpenType != ShtulpOpenType.NoShtulp;

		switch (leaf.OpenType)
		{
		case OpenType.Поворотно_откидное:
		return new int[] {shtup ? 400 : 300, shtup ? 550 : 450, 1520, 2390};

		case OpenType.Поворотное:
		int wF = leaf.BaseRegion.Lines[0].WidthInt;
		return new int[] {280, shtup ? 550 : wF > 700 ? 450 : 300, 1520, 2400};

		case OpenType.Откидное:
		case OpenType.Подвесное:
		int hF = leaf.BaseRegion.Lines[0].HeightInt;
		return new int[] {hF > 700 ? 450 : 200, 280, 2000, 1600};
		}

		return null;
		}

		private static bool getWk1(clsLeaf leaf)
		{
		clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("Комплектация фурнитуры");
		return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
		}

		private static string keepRussian(OpenType openType)
		{
		switch (openType)
		{
		case OpenType.Поворотное:
		return "поворотной";
		case OpenType.Поворотно_откидное:
		return "поворотно-откидной";
		case OpenType.Откидное:
		return "откидной";
		case OpenType.Подвесное:
		return "подвесной";
		default:
		return openType.ToString();
		}
		}


		private static string keepRussian(ShtulpOpenType leafShtulpOpenType)
		{
		switch (leafShtulpOpenType)
		{
		case ShtulpOpenType.NoShtulp:
		return string.Empty;
		case ShtulpOpenType.NoShtulpOnLeaf:
		return "активной штульповой";
		case ShtulpOpenType.ShtulpOnLeaf:
		return "пассивной штульповой";
		default:
		return string.Empty;
		}
		}
		}

		private class RuhauGrazioRestriction : Invalidance
		{
		private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan});

		private readonly string _message;

		private RuhauGrazioRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// свой профиль
		if (model.ProfileSystem.Name != RehauGrazio)
		return list;

		// ограничения
		bool restriction = getRestriction(model);

		// фурнитура 
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
		list.Add(new RuhauGrazioRestriction(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
		}

		// толщина заполнения === 32 | 40
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != 32 && region.Fill.Thikness != 40 && region.Fill.Thikness != 0)
		{
		list.Add(new RuhauGrazioRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения 32мм или 40мм", model.ProfileSystem.Name)));
		}
		}

		clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
		clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");
		const string Скошенный = "Скошенный";
		const string Черный = "Черный";
		const string Серый = "Серый";

		if (upShtap != null && upShtap.StringValue != Скошенный)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upShtap.Name, Скошенный), delegate { upShtap.StringValue = Скошенный; }));
		}

		// пробой слоя = правила зависят от продавца, БЛДЪ!
		int idseller = getIdseller(model);
		bool isRbnGrazioGreySeller = isRbnGrazioGrey(idseller);

		if (upRubberColor != null)
		{
		if (isRbnGrazioGreySeller)
		{
		if (upRubberColor.StringValue != Черный && upRubberColor.StringValue != Серый)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upRubberColor.Name, Черный), delegate { upRubberColor.StringValue = Черный; }));
		}
		}
		else
		{
		if (upRubberColor.StringValue != Черный)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upRubberColor.Name, Черный), delegate { upRubberColor.StringValue = Черный; }));
		}
		}
		}

		return list;
		}
		}

		private class RuhauBlitzNewRestriction : Invalidance
		{
		private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, Vorne, Axor});

		private readonly string _message;

		private RuhauBlitzNewRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// свой профиль
		if (model.ProfileSystem.Name != RehauBlitzNew)
		return list;

		// ограничения
		bool restriction = getRestriction(model);

		// докоперистость
		int? iddocoper = getiddocoper(model);

		// пробой слоя = правила зависят от продавца, БЛДЪ!
		int idseller = getIdseller(model);

		// теперь этот флажок работает только на стороне дилера
		bool isRbnVorneSeller = Settings.isDealer && isRbnVorne(idseller);
		bool isRbnGrazioGreySeller = isRbnGrazioGrey(idseller);

		// фурнитура
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (Settings.isDealer && idseller != 798)
		{
		if (isRbnVorneSeller)
		{
		if (model.FurnitureSystem.Name != Vorne)
		list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, Vorne), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(Vorne); }));
		}
		else
		{
		if (model.FurnitureSystem.Name != SiegeniaClassic)
		list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, SiegeniaClassic), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaClassic); }));
		}
		}
		else
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
		{
		list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }));
		}
		}
		}

		/// толщина заполнения === 24 | 32
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
		{
		list.Add(new RuhauBlitzNewRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения 24мм или 32мм", model.ProfileSystem.Name)));
		}
		}

		// только скошенный черный штапик
		clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
		clsUserParam upRubberColor = model.UserParameters.GetParam("Цвет уплотнений");
		const string Скошенный = "Скошенный";
		const string Черный = "Черный";
		const string Серый = "Серый";

		if (upShtap != null && upShtap.StringValue != Скошенный)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upShtap.Name, Скошенный), delegate { upShtap.StringValue = Скошенный; }));
		}

		if (upRubberColor != null)
		{
		if (isRbnGrazioGreySeller)
		{
		if (upRubberColor.StringValue != Черный && upRubberColor.StringValue != Серый)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upRubberColor.Name, Черный), delegate { upRubberColor.StringValue = Черный; }));
		}
		}
		else
		{
		if (upRubberColor.StringValue != Черный)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} только {1}", upRubberColor.Name, Черный), delegate { upRubberColor.StringValue = Черный; }));
		}
		}
		}

		/// маркетинговое ограничение, это место будет сильно меняться по мере ввода системы в эксплуатацию
		/// 2) для заказов кроме Окна ДА (iddocoper == 67) и К (iddocoper == 3) доступно только тип конструкции Балконное остекление и заполнения 4-16-4 и 24 сендвич
		if (Settings.isDealer && idseller != 798 && !isRbnVorneSeller && iddocoper != idddocoperOknaDa && iddocoper != idddocoperRekl && iddocoper != idddocoperKrupn && Settings.idpeople != 74 && Settings.idpeople != 159)
		{
		if (model.ConstructionType.Name != _balconGlazing)
		{
		if (restriction)
		{
		list.Add(new RuhauBlitzNewRestriction(model, string.Format("в профильной системе {0} доступен только тип конструкции {1}", model.ProfileSystem.Name, _balconGlazing)));
		}
		}
		else
		{
		foreach (clsRegion region in model.VisibleRegions)
		{
		const string defEmpty = "Без_стекла_и_штапика";
		const string defEmpty24 = "Без_стекла_24";
		const string defGlass = "4х16х4";
		const string defSandwich = "COS0001.07";
		if (region.Fill.Marking != defEmpty && region.Fill.Marking != defEmpty24 && region.Fill.Marking != defGlass && region.Fill.Marking != defSandwich)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, string.Format("В типе конструкции {0} доступны только заполнения {1} и {2}", model.ConstructionType.Name, "4х16х4", "COS0001.07"), delegate
		{
		switch (region1.Fill.FillType)
		{
		case FillType.NotFill:
		region1.Fill = region1.Model.ProfileSystem.GetFillByMarking(defEmpty24);
		break;
		case FillType.Sandwich:
		region1.Fill = region1.Model.ProfileSystem.GetFillByMarking(defSandwich);
		break;
		default:
		case FillType.GlassPack:
		region1.Fill = region1.Model.ProfileSystem.GetFillByMarking(defGlass);
		break;
		}
		}));
		}
		}
		}
		}
		}

		return list;
		}

		private static bool isRbnVorne(int idseller)
		{
		object[] objects = CalcProcessor.Modules["isRbnVorne"](new object[] {idseller});
		return objects != null && objects.Length > 0 && objects[0] is bool && (bool) objects[0];
		}
		}

		// SCHTANDART START
		// SCHTANDART COMFORT
		// SCHTANDART PREMIUM
		private class SchtandartRestriction : Invalidance
		{
		private static readonly List<string> furnSystemList60 = new List<string>(new string[] {Vorne, SiegeniaClassic});
		private static readonly List<string> furnSystemList70 = new List<string>(new string[] {Vorne, SiegeniaClassic, SiegeniaTitan, SiegeniaAxxent});

		private readonly string _message;

		private SchtandartRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
		}

		public override string uniq()
		{
		return _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// свой профиль
		if (model.ProfileSystem.Name != SCHTANDART_START && model.ProfileSystem.Name != SCHTANDART_COMFORT && model.ProfileSystem.Name != SCHTANDART_PREMIUM)
		return list;

		// ограничения
		bool restriction = getRestriction(model);

		// фурнитура
		if (model.FurnitureSystem.Name != FurnSystBEZ && restriction)
		{
		List<string> furnSystemList = model.ProfileSystem.Name != SCHTANDART_PREMIUM ? furnSystemList60 : furnSystemList70;
		if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		list.Add(new SimpleInvariant(model, string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }));
		}

		/// TODO REFACTOR:: звести это в системы и както гармонизировать, а то много дублирования и внутри класса и одинаковый код в разных классах
		/// толщина заполнения === 24, 32, 32|40
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness > 0)
		{
		switch (model.ProfileSystem.Name)
		{
		case SCHTANDART_START:
		if (region.Fill.Thikness != 24)
		list.Add(new SchtandartRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения {1}мм", model.ProfileSystem.Name, 24)));
		break;

		case SCHTANDART_COMFORT:
		if (region.Fill.Thikness != 32)
		list.Add(new SchtandartRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения {1}мм", model.ProfileSystem.Name, 32)));
		break;

		case SCHTANDART_PREMIUM:
		if (region.Fill.Thikness != 32 && region.Fill.Thikness != 40)
		list.Add(new SchtandartRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения {1}, {2}мм", model.ProfileSystem.Name, 32, 40)));
		break;
		}
		}
		}

		// уплотнение = foo(от систем) от штапика не зависит, наоборот штапик зависмит от цвет уплотнения в модели
		const string Черный = "Черный";
		const string Серый = "Серый";
		const string цветУплотнений = "Цвет уплотнений";
		clsUserParam upRubberColor = model.UserParameters.GetParam(цветУплотнений);
		switch (model.ProfileSystem.Name)
		{
		case SCHTANDART_START:
		if (upRubberColor != null && upRubberColor.StringValue != Черный)
		list.Add(new SimpleInvariant(model, string.Format("для этой конструкции применяется {0} {1}", upRubberColor.Name, Черный), delegate { upRubberColor.StringValue = Черный; }));
		break;
		default:
		if (upRubberColor != null && upRubberColor.StringValue != Серый)
		list.Add(new SimpleInvariant(model, string.Format("для этой конструкции применяется {0} {1}", upRubberColor.Name, Серый), delegate { upRubberColor.StringValue = Серый; }));
		break;
		}

		// проверяем чтоб заполенией с указанной тольщиной >0 было не более 1 разного
		if(getThicknesses(model).Count > 1)
		list.Add(new SchtandartRestriction(model, "различные толщины заполнений в пределах одной модели"));

		// штапик имеет смысл только для заполнений с указанной толщиной > 0
		if (getThickness(model) > 0)
		{
		List<clsProfile> shtapikProfiles = getShtapikProfiles(model);
		if (shtapikProfiles == null || shtapikProfiles.Count == 0)
		{
		list.Add(new SchtandartRestriction(model, string.Format("не удалось подобрать доступный штапик для заполнеия толщиной {0}мм с цветом уплотений {1}", getThickness(model), upRubberColor != null ? upRubberColor.StringValue : string.Empty)));
		}
		else
		{
		clsUserParam upShtap = model.UserParameters.GetParam("Штапик");
		if (upShtap != null)
		{
		clsProfile match = shtapikProfiles.Find(delegate(clsProfile profile) { return profile.Steels.Count > 0 && profile.Steels[0] == upShtap.StringValue; });
		if (match == default(clsProfile))
		{
		string shtapik = shtapikProfiles[0].Steels[0];
		list.Add(new SimpleInvariant(model, string.Format("для этой конструкции применяется {0} {1}", upShtap.Name, shtapik), delegate { upShtap.StringValue = shtapik; }));
		}
		}
		}
		}

		// коммерция - микропроветривание
		const string микрощелевоеПроветривание = "Микрощелевое проветривание";
		foreach (clsLeaf leaf in model.Leafs)
		{
		const string да = "Да";
		const string нет = "Нет";
		bool microVent = model.ProfileSystem.Name != SCHTANDART_START && leaf.OpenType == OpenType.Поворотно_откидное;
		clsUserParam upMicroVent = leaf.UserParameters.GetParam(микрощелевоеПроветривание);
		if (upMicroVent != null && upMicroVent.StringValue != (microVent ? да : нет))
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} = {1}", upMicroVent.Name, microVent ? да : нет), 
		delegate { upMicroVent.StringValue = microVent ? да : нет; }));
		}
		}

		return list;
		}

		private static List<int> getThicknesses(clsModel model)
		{
		List<int> thicknesses = new List<int>();
		foreach (clsRegion region in model.VisibleRegions)
		{
		if(region.Thickness > 0 && !thicknesses.Contains(region.Thickness))
		thicknesses.Add(region.Thickness);
		}

		return thicknesses;
		}

		// доступные штапики как функция от модели толщины заполнения
		private static List<clsProfile> getShtapikProfiles(clsModel model)
		{
		List<clsProfile> list = new List<clsProfile>();

		int thickness = getThickness(model);
		const string цветУплотнений = "Цвет уплотнений";
		clsUserParam upRubberColor = model.UserParameters.GetParam(цветУплотнений);
		string rubberColor = upRubberColor != null ? upRubberColor.StringValue : null;

		foreach (clsProfile profile in model.ProfileSystem.colProfileShtapik)
		if (profile != null 
		&& profile.F1 <= thickness && thickness <= profile.F2
		&& profile.Comment == rubberColor
		)
		list.Add(profile);

		return list;
		}

		private static int getThickness(clsModel model)
		{
		int thickness = 0;
		foreach (clsRegion region in model.VisibleRegions)
		if (region.Thickness > thickness)
		thickness = region.Thickness;

		return thickness;
		}
		}

		public class SlidingRestriction : Invalidance
		{
		private readonly string _message;

		public SlidingRestriction(object subj, string message) : base(subj)
		{
		this._message = message;
		}

		public override string message()
		{
		return _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>();

		if (model.ProfileSystem.Name == SystSlayding60)
		{
		OrderClass order = model.WinDraw.DocClass as OrderClass;
		bool restriction = getRestriction(model);
		bool reklamacia = order != null && order.DocRow != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 59;

		const string Без_стекла_6 = "Без_стекла_6";
		const string standart = "M1 04-H";
		const string triplex = "3+3 резка";
		const string triplexMated = "3+3 матовая пленка";
		const string sndwich24 = "COS0001.07";

		// геометрия, только прямоугольные вариации
		if (!orthogonal(model))
		list.Add(new SlidingRestriction(model, string.Format("{0}, только прямоугольные конструкции.", model.ProfileSystem.Name)));

		/// спёрто из алютеха
		/// ограничения на длинну балки
		const int max = 4000;
		const int min = 200;

		foreach (clsBeem beem in model.Frame)
		{
		if (beem.Lenght < min || beem.Lenght > max)
		if (restriction)
		list.Add(new SlidingRestriction(model, string.Format("Длина балки рамы {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, min, max)));
		}

		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsBeem beem in leaf)
		{
		if (beem.Lenght < min || beem.Lenght > max)
		list.Add(new SlidingRestriction(model, string.Format("Длина балки створки {3} {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, min, max, leaf)));
		}
		}

		// .Lenght - длина импоста по осям, .Inside_Lenght - видимая часть внутри рамы, beem.LineC1.Length - по резке
		foreach (clsBeem beem in model.Imposts)
		if (beem.LineC1.Length < min || beem.LineC2.Length < min || beem.Lenght > max)
		if (restriction)
		list.Add(new SlidingRestriction(model, string.Format("Длина импоста {0} выходит за допустимые границы: от {1} до {2} мм", beem.Name, 200, max)));

		// не положено импостов внутри створки
		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsImpost impost in leaf.Imposts)
		{
		list.Add(new SlidingRestriction(model, string.Format("{0}, {1} в створке не ставится", model.ProfileSystem.Name, impost.Name)));
		break;
		}
		}

		// заполнения
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region._Leaf == null)
		{
		// свободный глухарь без импостов
		if (region.FatherImpost == null)
		{
		if (region.Fill.FillType == FillType.GlassPack)
		{
		if (region.RegionHeight <= 1700)
		{
		if (region.Fill.Thikness != 4 && region.Fill.Thikness != 6 || region.Fill.FillType != FillType.GlassPack)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "Используются только заполнения толщиной 4мм или 6мм или сендвич", delegate { region1.Fill = model.ProfileSystem.Fills[standart]; }));
		}
		}
		}
		else if (1700 < region.RegionHeight)
		{
		if (region.Fill.Marking != triplex && region.Fill.Marking != triplexMated)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, string.Format("При высоте заполнения более {0}, используется только триплекс", 1700), delegate { region1.Fill = model.ProfileSystem.Fills[triplex]; }));
		}
		}
		}
		}
		else if (region.Fill.FillType == FillType.Sandwich)
		{
		if (region.Fill.Thikness != 24 && restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "Используются только сендвичи толщиной 24мм", delegate { region1.Fill = model.ProfileSystem.Fills[sndwich24]; }));
		}
		}
		else // if (region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown || region.Fill.FillType == FillType.PuzzleFill)
		{
		// унификация до Без_стекла_6
		if (region.Fill.Marking != Без_стекла_6)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, null, delegate { region1.Fill = model.ProfileSystem.Fills[Без_стекла_6]; }));
		}
		}
		}
		else
		{
		// регион с импостом , далее все сложнее кто-то раньше этого места рулит импосты (без импоста .a=0 / с импостом .a >0)
		// на регионы с импостом .a==0 сносим стекло
		if (region.Fill.Marking != Без_стекла_6 && haveFalseImpost(region))
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, null, delegate { region1.Fill = model.ProfileSystem.Fills[Без_стекла_6]; }));
		}
		}
		}
		else
		{
		clsLeaf leaf = region._Leaf;
		if ((region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown) && region.Fill.Marking != Без_стекла_6)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "Унификация до Без_стекла_6", delegate { region1.Fill = model.ProfileSystem.Fills[Без_стекла_6]; }));
		}

		if (leaf.Height <= 1700)
		{
		if (region.Fill.Thikness != 4 && region.Fill.Thikness != 6 || region.Fill.FillType != FillType.GlassPack)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "Используются только заполнения толщиной 4мм или 6мм", delegate { region1.Fill = model.ProfileSystem.Fills[standart]; }));
		}
		}
		}
		else if (1700 < leaf.Height)
		{
		if (region.Fill.Marking != triplex && region.Fill.Marking != triplexMated)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, string.Format("При высоте створки более {0}, используется только триплекс", 1700), delegate { region1.Fill = model.ProfileSystem.Fills[triplex]; }));
		}
		}
		}
		}
		}

		const int maxLeafQu = 4;
		if (model.Leafs.Count > maxLeafQu)
		list.Add(new SlidingRestriction(model, string.Format("{0} не более {1} створок", model.ProfileSystem.Name, maxLeafQu)));

		if (2300 < model.Frame.Height && restriction)
		{
		list.Add(new SlidingRestriction(model, string.Format("{0}, максимальная высота конструкции {1}", model.ProfileSystem.Name, 2300)));
		}
		}

		return list;
		}

		// BUG return (int)region.FatherImpost.Profile.A == 0; // || (int)region.ChildImpost.Profile.A == 0;
		private static bool haveFalseImpost(clsRegion region)
		{
		foreach (clsLine clsLine in region.FillConture)
		{
		clsBeem beem = clsLine.Beem;
		if (beem != null && (int) beem.Profile.A == 0)
		return true;
		}

		return false;
		}

		public static bool orthogonal(clsModel model)
		{
		foreach (clsBeem beem in model.Frame)
		{
		if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
		return false;
		}

		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsBeem beem in leaf)
		{
		if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
		return false;
		}
		}

		foreach (clsBeem beem in model.Imposts)
		{
		if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
		return false;
		}

		return true;
		}
		}

		/// <summary>
		/// ограничения на раскладку
		/// </summary>
		private class SpreadingRestriction : Invalidance
		{
		private readonly string _message;

		private SpreadingRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		return _message;
		}

		public override string uniq()
		{
		return _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// кладем на неограниченных
		if (!getRestriction(model))
		return list;

		foreach (clsRegion region in model.VisibleRegions)
		{
		Atechnology.winDraw.Spreading_v2.Spreading spreading = region.SpreadingV2;
		if (spreading != null)
		{
		// кеш уже рассчитанных соединений
		List<Vector2F> cache = new List<Vector2F>();

		foreach (Beam beam in spreading.Beams)
		{
		if (beam is ConnectorBeam)
		continue;

		// хотя в таблице укзано полная ширина внутри себя АТ оперирует половиной указанной ширины профиля, на манер импостов
		int profileWidth = (int) Math.Round(spreading.ProfileWidth * 2);

		// длина луча, не факт что всё так просто
		int length = (int) Math.Round(beam.Length);

		// макс зависит от разного...
		int maxLength = profileWidth <= 8 || model.ConstructionType.Name != _window ? 1000 : profileWidth <= 18 ? 1200 : 1500;

		if (length > maxLength)
		{
		list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: длина луча = {1} (максимально допустимо {2} для данного профиля {3} и типа конструкции)", region.Part, length, maxLength, spreading.Profile)));
		}

		// мин зависит от рамного соединения 
		// Atechnology.winDraw.Spreading_v2.Spreading.cs:1302 connectorLeg.Length = 20.0; // привет Кубань! magic numger rulez foreвa 
		// поэтому для сравнени с миималкой надо зать куда луч прицеплен, а это будет понятно позже при трахе с рамой
		int minLength = profileWidth <= 8 ? 70 : 90;

		//                            if (length < minLength)
		//                            {
		//                                list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: длина луча = {1} (минмально допустимо {2} для данного профиля {3})", region.Part, length, minLength, spreading.Profile)));
		//                            }

		// минимальный радиус зависит от толщины в описвании профиля неуказуем 
		int minRadius = profileWidth <= 8 ? 150 : profileWidth <= 18 ? 300 : int.MaxValue;
		if ((int) beam.Radius != 0 && Math.Abs(beam.Radius) < minRadius)
		{
		list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: радиус изгиба луча {1:F0} (минимально допустимо {2})", region.Part, beam.Radius, minRadius)));
		}

		// контроль угла прилегания между раскладой и рамкой
		foreach (Connector connector in beam.Connectors)
		{
		int lag = 40;

		List<IPrimitive> framePrimitives = getConnectedFramePrimitives(connector);

		// Baned
		// if (framePrimitives.Count > 1)
		//    list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: луч не должен упираться в угол рамки", region.Part)));

		if (framePrimitives.Count > 0)
		{
		foreach (IPrimitive framePrimitive in framePrimitives)
		{
		if (framePrimitive != null)
		{
		lag = 20;

		foreach (Vector3D point in getNearPointsOf(framePrimitive, connector.Beam.Primitive))
		{
		double connectorRelative = connector.Beam.Primitive.GetRelativePoint(point);
		double framePrinmitiveRelative = framePrimitive.GetRelativePoint(point);

		Vector2F v0 = framePrimitive.GetTangent(framePrinmitiveRelative).To2D().To2F();
		Vector2F v1 = connector.Beam.Primitive.GetTangent(connectorRelative).To2D().To2F();

		// Vector2F v0 = (framePrimitive.End - framePrimitive.Start).To2D().To2F();
		// Vector2F v1 = (connector.Beam.Primitive.End - connector.Beam.Primitive.Start).To2D().To2F();
		// // поубивывбы, метод .ComputeAngle работает с нормироваными векторами направлений при подаче ненормированных несёт бред, а почему потроха класса вываливаются наружу расскажут кранадарцы
		// v0.Normalize();
		// v1.Normalize();

		// радианы
		double a = Angle.ComputeAngle(v1, v0);

		// привести к интервалу [0,pi/2]
		if (a < 0)
		a += Angle.TWO_PI;

		if (a > Angle.PI)
		a -= Angle.PI;

		if (a > Angle.HALF_PI)
		a = Angle.PI - a;

		// градусы
		a = Math.Round(a * 180 / Angle.PI);

		const double min = 30;
		if (a < min)
		{
		list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: угол между лучом раскладки и рамкой = {1}° (минимально допустимо {2}°)", region.Part, a, min)));
		}
		}
		}
		}
		}
		else
		{
		// контроль углов на соединениях,  > 8мм
		if (profileWidth > 8)
		{
		// перебор соединителей 20 мм, обычно 1 шт
		foreach (Connector connectedConnector in connector.ConnectedConnectors)
		{
		Connector center = connectedConnector.Opposite;

		// этих бывает больше 1
		List<Connector> connected = new List<Connector>(center.ConnectedConnectors);

		// если больше 1 и еще не рассчитан то продолжаем жевать, todo однако тут сложнее они потенциально могут немного разбегаться, надо будет прикруть огрубитель Vector2F
		Vector2F hash = center.Coord.To2D().To2F();
		if (connected.Count > 1 && !cache.Contains(hash))
		{
		foreach (Connector center1 in connected)
		{
		// пока нам важно докопаться только до ортогональности, далее будет полная задница ибо лучи абс свободны и необразуют никакой топологи от слова совсем
		Vector2F v0 = (center.Beam.Primitive.End - center.Beam.Primitive.Start).To2D().To2F();
		Vector2F v1 = (center1.Beam.Primitive.End - center1.Beam.Primitive.Start).To2D().To2F();
		// поубивывбы, метод .ComputeAngle работает с нормироваными векторами направлений при подаче ненормированных несёт бред, а почему потроха класса вываливаются наружу расскажут кранадарцы
		v0.Normalize();
		v1.Normalize();

		// градусы [-360;+360] потом придёт понимание что 89° = 90°, с радианами это видится сложнее, а с гадусами просто (int)
		int a = (int) Math.Round(Angle.ComputeAngle(v1, v0) * 180 / Angle.PI);

		if (a < 0)
		a += 360;

		if (a > 180)
		a = 360 - a;

		// критерий ортогональности = 90° +- 1°
		if (Math.Abs(a > 90 ? 180 - a : a % 90) > 1)
		{
		list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: не оргтогональный угол в узле соединения = {1}° (требование {2}°)", region.Part, a, 90)));
		cache.Add(hash);
		break;
		}
		}

		cache.Add(hash);
		}
		}
		}
		}

		// тперь зная лаг можно мерится длиннами
		if (length < minLength - lag)
		{
		list.Add(new SpreadingRestriction(region, string.Format("Раскладка {0}: длина луча = {1} (минмально допустимо {2} для данного профиля {3})", region.Part, length + lag, minLength, spreading.Profile)));
		}
		}
		}
		}
		}

		return list;
		}

		private static IEnumerable<Vector3D> getNearPointsOf(IPrimitive primitive, IPrimitive vector)
		{
		List<Vector3D> list = new List<Vector3D>();

		if (primitive.Distance(vector.Start, false, false) < 0.1)
		list.Add(vector.Start);

		if (primitive.Distance(vector.End, false, false) < 0.1)
		list.Add(vector.End);

		return list;
		}

		private static IPrimitive getConnectedFramePrimitive(Connector connector)
		{
		foreach (IPrimitive primitive in connector.Beam.Spreading.Conture.Frame.Primitives)
		{
		if (primitive.Distance(connector.Coord, false, false) < 0.1)
		return primitive;
		}

		return null;
		}

		private static List<IPrimitive> getConnectedFramePrimitives(Connector connector)
		{
		List<IPrimitive> list = new List<IPrimitive>();
		foreach (IPrimitive primitive in connector.Beam.Spreading.Conture.Frame.Primitives)
		{
		if (primitive.Distance(connector.Coord, false, false) < 0.1)
		list.Add(primitive);
		}

		return list;
		}
		}

		// автомат москитных сеток
		private class AccessoriesColorAutomat : Invalidance
		{
		private AccessoriesColorAutomat(clsModel subj, string message) : base(subj)
		{
		_message = message;
		}

		private readonly string _message;

		public override string message()
		{
		return this._message;
		}

		private const string РучноеУправлениеЦветом = "Ручное управление цветом";
		private const string МоскитнаяСетка = "Москитная сетка";

		public enum ColorType
		{
		Белый,
		Коричневый,
		unpredictable
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// если тип конструкции м/с то лесом проверки сами полечат
		if (model.ConstructionType.Name == МоскитнаяСетка)
		return list;

		// если стоит ручное кправление то лесом-с
		clsUserParam upManual = model.UserParameters.GetParam(РучноеУправлениеЦветом);
		if (upManual != null && upManual.StringValue != upManual.DefaultValue.StrValue)
		return list;

		Color outsideColor = Color.FromArgb(model.ColorOutside.Index);
		ColorType colorTypeOutside = getColorType(model.ColorOutside.Index);

		// москитные сетки
		foreach (clsLeaf leaf in model.Leafs)
		{
		clsUserParam up = leaf.UserParameters[МоскитнаяСетка];
		if (up == null)
		continue;

		if (colorTypeOutside != ColorType.unpredictable)
		{
		if (up.StringValue2 != colorTypeOutside.ToString())
		{
		if (leaf.IsMoskit != IsMoskit.Нет)
		{
		clsUserParam up1 = up;
		list.Add(new SimpleInvariant(model, string.Format("Внешний цвет изделия '{0}', рекомендуется {1} цвет москитной сетки", model.ColorOutside.ColorName, colorTypeOutside), delegate { up1.StringValue2 = colorTypeOutside.ToString(); }));
		}
		else
		{
		clsUserParam up1 = up;
		list.Add(new SimpleInvariant(model, null, delegate { up1.StringValue2 = colorTypeOutside.ToString(); }));
		}
		}
		}
		else
		{
		if (leaf.IsMoskit != IsMoskit.Нет)
		list.Add(new Warning(model, string.Format("Внешний цвет изделия '{0}', цвет м/с `{1}`. Проконтролируйте цвет м/с самостоятельно!", model.ColorOutside.ColorName, up.StringValue2)));
		}
		}

		return list;
		}

		private static ColorType getColorType(int color)
		{
		// оттенок 300 = магрнета, 120 зеленый
		// http://www.colory.ru/colorwheel/
		// https://en.wikipedia.org/wiki/HSL_and_HSV#/media/File:HSV-RGB-comparison.svg
		float hue = Color.FromArgb(color).GetHue();

		float b = Color.FromArgb(color).GetBrightness();

		if (b > 0.89)
		return ColorType.Белый;

		if (Color.FromArgb(color).GetSaturation() < 0.15)
		return ColorType.Коричневый;

		// если оттенок холодный то не
		// return !(hue > 120 && hue < 300);
		return !(hue > 90 && hue < 330) ? ColorType.Коричневый : ColorType.unpredictable;
		}
		}

		private class DeceuninckRestriction : Invalidance
		{
		private static readonly List<string> profSystemList = new List<string>(new string[] {Eco60, EnwinMinima, FORWARD, BAUTEK, BAUTEKmass, FavoritSpace});
		private static readonly List<string> furnSystemList = new List<string>(new string[] {SiegeniaClassic, SiegeniaTitan, Vorne, Axor, SiegeniaAxxent});

		private readonly string _message;

		private DeceuninckRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public override string message()
		{
		// подмена З-1 на Г-1 ибо на картинке показана Г-1
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("Заполнение {0} {1}", region.Part, _message) : _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// свой профиль
		if (!profSystemList.Contains(model.ProfileSystem.Name))
		return list;

		// потом отрубить
		tryFixProfile(model);

		bool restriction = getRestriction(model);

		int idseller = getIdseller(model);

		// мягкое закрытие, что с одной тороны будет пинать менеджера, не даст пересчитывать старые заказы с другой стороны можно снять ограничения если очень надо
		if (restriction && model.ProfileSystem.Name == Eco60)
		{
		list.Add(new DeceuninckRestriction(model, string.Format("Профильная система {0} не доступна", Eco60)));
		}

		// инвариант порога делается на лету без сообщения
		foreach (clsFrame frame in model.Frame)
		{
		if (frame.bType == ComponentType.Porog && frame.ConnectType != ConnectType.Длинное)
		{
		// C# 2.0 behaviour
		clsFrame frame1 = frame;
		list.Add(new SimpleInvariant(model, null, delegate { frame1.ConnectType = ConnectType.Длинное; }));
		}
		}

		// фурнитура === Classic | Дверная | ПСК*
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (_outdoor == model.ConstructionType.Name)
		{
		if (FurnSystDver != model.FurnitureSystem.Name)
		list.Add(new SimpleInvariant
		(model,
		null, //string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, furnSystemList[0]),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(FurnSystDver); }
		));
		}
		else if (_pskportal == model.ConstructionType.Name)
		{
		// SELECT * FROM system WHERE name LIKE 'Фурнитура PSK%'
		if (!model.FurnitureSystem.Name.StartsWith("Фурнитура PSK"))
		{
		list.Add(new SimpleInvariant
		(model,
		null,
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(PSK_160_COMFORT); }
		));
		}
		}
		else
		{
		if (idseller == 798)
		{
		if (model.ProfileSystem.Name == Eco60 && model.FurnitureSystem.Name != Vorne)
		{
		list.Add(new SimpleInvariant
		(model,
		string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, Vorne),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(Vorne); }
		));
		}
		}
		else if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		{
		list.Add(new SimpleInvariant
		(model,
		string.Format("В профсистеме {0} используется {1}", model.ProfileSystem.Name, furnSystemList[0]),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
		));
		}
		}
		}

		// толщина заполнения === 0 | 4 | 24 | 32 | 42
		List<int> avaliableThicknesses = getAvaliableThicknesses(model);

		if (avaliableThicknesses.Count > 0)
		{
		foreach (clsRegion region in model.VisibleRegions)
		{
		//region.Fill.Thikness != 4 && region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && 
		if (region.Fill.Thikness > 0 && !avaliableThicknesses.Contains(region.Fill.Thikness))
		{
		list.Add(new DeceuninckRestriction(region, string.Format("В профсистеме {0} можно использовать только заполнения толщиной {1} мм", model.ProfileSystem.Name, string.Join(", ", avaliableThicknesses.ConvertAll<string>(delegate(int input) { return input.ToString(); }).ToArray()))));
		}
		}
		}
		else
		{
		list.Add(new DeceuninckRestriction(model, string.Format("Не найдено настроек доступных толщин заполнений для системы {0}. Проверьте поле Thikness в настройках систем", model.ProfileSystem.Name)));
		}

		// только скошенный серый! штапик // пока по колхозному но потенциально это можно навесить на системы 
		const string upShtapicName = "Штапик";
		const string upColorName = "Цвет уплотнений";
		const string skew = "Скошенный";
		string rightRubberColor = model.ProfileSystem.Name == BAUTEKmass ? "Черный" : "Серый";
		clsUserParam upShtapic = model.UserParameters.GetParam(upShtapicName);
		clsUserParam upColor = model.UserParameters.GetParam(upColorName);
		if ((upShtapic != null && upShtapic.StringValue != skew) || (upColor != null && upColor.StringValue != rightRubberColor))
		{
		list.Add(new SimpleInvariant(
		model,
		string.Format("В профсистеме {0} используется только {1} штапик и {2} цвет уплотнения", model.ProfileSystem.Name, skew, rightRubberColor),
		delegate
		{
		model.UserParameters[upShtapicName].StringValue = skew;
		model.UserParameters[upColorName].StringValue = rightRubberColor;
		}));
		}

		// без арок
		{
		clsBeem beem = getArcBeem(model);
		if (beem != null)
		{
		if (restriction)
		list.Add(new DeceuninckRestriction(beem, string.Format("{0}, арки недоступны", beem.Name)));
		}
		}

		// на пороги не положено ставить импост // копия из RehauProfileRestiction // надо обобщить
		foreach (clsBeem beem in model.Frame)
		{
		if (beem.BalkaType == ModelPart.Porog)
		{
		foreach (clsBeem impost in beem.ConnectedImposts)
		{
		if (impost.BalkaType == ModelPart.Impost)
		{
		list.Add(new RehauProfileRestriction(model, "Не возможно установить импост на порог"));
		break;
		}
		}
		}
		}

		return list;
		}

		/// приведение /D /P к общему string.Empty, пока кроме ECO 60 и сотоварищи
		private static void tryFixProfile(clsModel model)
		{
		if (model.ProfileSystem.Name == FORWARD || model.ProfileSystem.Name == BAUTEK || model.ProfileSystem.Name == BAUTEKmass || model.ProfileSystem.Name == FavoritSpace)
		{
		foreach (clsBeem beem in model.Frame)
		tryFix(beem);

		foreach (clsLeaf leaf in model.Leafs)
		foreach (clsBeem beem in leaf)
		tryFix(beem);

		foreach (clsBeem beem in model.Imposts)
		tryFix(beem);
		}
		}

		private static void tryFix(clsBeem beem)
		{
		try
		{
		if (beem.Profile.Marking.EndsWith(D) || beem.Profile.Marking.EndsWith(P) || beem.Profile.Marking.EndsWith(RD))
		{
		fixProfile(beem);
		}
		}
		catch (Exception ex)
		{
		WdLog.Add("code", "deceuninckRestriction", ex.ToString());
		}
		}

		private const string D = "/D";
		private const string P = "/P";
		private const string RD = "/RD";

		private static void fixProfile(clsBeem beem)
		{
		string marking = beem.Profile.Marking.Replace(D, string.Empty).Replace(P, string.Empty).Replace(RD, string.Empty);
		clsProfile rightProfile = beem.Model.ProfileSystem.GetcolProfile(beem.Profile.ProfType)[marking];
		beem.Profile = rightProfile;
		}

		private static List<int> getAvaliableThicknesses(clsModel model)
		{
		List<int> list = new List<int>();

		// т.к. тут нет Thickness, f1, f2 придётся лезть в кроличью нору
		ProfileSystem profileSystem = SettingsLoad.currentSettings.GetProfileSystemByName(model.ProfileSystem.Name);

		if (profileSystem != null)
		{
		// упрощённо считается что штапики различаются только толщиной (в рехау они еще различаются формой и цветом уплотнения, что потенциально добавляемо в уловие проверки)
		foreach (Profile profile in profileSystem.ProfileList)
		if (profile != null && profile.ProfileType == ProfileType.Shtapic && profile.Thickness > 0)
		list.Add((int) profile.Thickness);
		}

		list.Sort();

		return list;
		}
		}

		private class TopologyRestriction : Invalidance
		{
		private readonly string _message;

		public override string message()
		{
		return _message;
		}

		public override string uniq()
		{
		return _message;
		}

		private TopologyRestriction(object subj, string message) : base(subj)
		{
		_message = message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>();


		return list;
		}
		}

		private class InactiveProfileRestriction : Invalidance
		{
		private readonly string _message;

		public InactiveProfileRestriction(clsBeem subj, string message) : base(subj)
		{
		this._message = message;
		}

		public override string message()
		{
		return _message;
		}

		public override string uniq()
		{
		return _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>();

		if (getRestriction(model))
		{
		foreach (clsBeem beem in model.Frame)
		{
		if (beem.Profile.Marking.StartsWith("554002") && Settings.idpeople != 255 && Settings.idpeople != 74)
		{
		list.Add(new InactiveProfileRestriction(beem, string.Format("профиль {0} не активен", beem.Profile.Marking)));
		}
		}

		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsBeem beem in leaf)
		{
		if (beem.Profile.Marking.StartsWith("554012"))
		{
		list.Add(new InactiveProfileRestriction(beem, string.Format("профиль {0} не активен", beem.Profile.Marking)));
		}
		}
		}
		}

		return list;
		}
		}

		////
	}
}
