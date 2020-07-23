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

		private const string Vorne = "��������� VORNE"; // ������������ ����������� �������
		// in line
		private const string FurnSystBEZ = "��������� ���";

		private const string FurnSystDver = "��������� �������";

		private const string SiegeniaAxxent = "��������� Siegenia Axxent";
		private const string SiegeniaClassic = "��������� Siegenia Classic";
		private const string SiegeniaTitan = "��������� Siegenia Titan Special";
		private const string SiegeniaTitanWK1 = "��������� Siegenia Titan Special WK-1";
		private const string TitanDebug = "��������� Titan // debug";
		private const string Axor = "��������� AXOR KOMFORT LINE K3";

		// ���������� ������� ����������� �������
		private const string DefaultWindowFurnitureSystem = SiegeniaClassic;

		private const string RotoNT = "��������� ROTO NT";
		private const string RotoOK = "��������� ROTO OK";
		private const string RotoNTDesigno = "��������� ROTO NT Designo";
		private const string GEISSE = "��������� GIESSE";
		private const string FurnitureSlayding60 = "��������� ��������-60";
		// add	
		private const string PSK_Vorne = "��������� PSK Vorne";
		private const string PSK_160_COMFORT = "��������� PSK 160 COMFORT";
		private const string PSKPORTAL100 = "��������� PSK PORTAL 100";
		private const string PSKPORTAL160 = "��������� PSK PORTAL 160";
		private const string PSKPORTAL200 = "��������� PSK PORTAL 200";

		// in line
		private const string ProfSystBEZ = "��� �������";
		private const string Pimapen = "Pimapen";
		private const string Rehau70 = "Rehau 70";
		private const string Rehau70mm = "Rehau 70��";
		private const string Vario = "Vario";
		private const string ThermoLock = "Thermolock (Rehau)";
		private const string Classic = "Classic";
		private const string Thermo70 = "THERMO 70";
		private const string Solar = "Solar";
		private const string RehauEuro70 = "Rehau Euro-70";
		private const string Rehau60 = "Rehau 60��";
		private const string SystSlayding60 = "��������-60";
		private const string KP45 = "������ ��45";
		private const string KPT74 = "������ ���74";
		private const string EVO = "EVO";
		private const string ALUTECH62 = "ALUTECH ALT W62";
		private const string ALUTECH_48 = "ALUTECH ALT C48";
		private const string ALT_F50 = "ALT F50";
		private const string ALT_F50Cover = "ALT F50 ������";
		private const string ALT100 = "ALT100";
		private const string DECOR = "DECOR";
		private const string ProfSystGlass = "���������� �����������";
		private const string EuroDesign = "EURO-DESIGN";

		// ����� RBN
		private const string SCHTANDART_START = "SCHTANDART START";
		private const string SCHTANDART_COMFORT = "SCHTANDART COMFORT";
		private const string SCHTANDART_PREMIUM = "SCHTANDART PREMIUM";

		// Deceunink
		private const string Eco60 = "ECO 60";
		private const string FORWARD = "�������";
		private const string BAUTEK = "������";
		private const string BAUTEKmass = "������ � �����";
		private const string FavoritSpace = "������� �����";

		private const string EnwinMinima = "ENWIN MINIMA";
		private const string RehauOptima = "REHAU OPTIMA";
		private const string RehauDeluxe = "REHAU DELUXE";
		private const string RehauBlitzNew = "Rehau Blitz New";
		// 70
		private const string SibDesign = "SIB-DESIGN";
		private const string RehauMaxima = "REHAU MAXIMA";
		private const string RehauGrazio = "Rehau Grazio";
		private const string Thermo76 = "THERMO 76";
		// ���� T76 �� ������ � �������� ��������
		private const string Estet = "Estet";
		// 80
		private const string NEO_80 = "NEO 80";
		// Gealan
		private const string Gealan = "Gealan S 9000";

		// ���� �����������
		private const string _window = "����";
		private const string _balcon = "��������� �����";
		private const string _indoor = "����� ������������";
		private const string _outdoor = "����� �������";
		private const string _moskit = "��������� �����";
		private const string _glass = "�����������";
		private const string _swingdoor = "����� �����������";
		private const string _wcdoor = "��������� �����";
		private const string _pskportal = "��� ������";

		private const string _nalichnik = "��������";
		private const string _facade = "�����";
		private const string _manual = "����������� �� ������� �������";
		private const string _gate = "������ (������ ������)";
		private const string _wicket = "������� (������ ������)";
		private const string _balconGlazing = "��������� ����������";

		// todo global 
		const int idddocoperKrupn = 3;
		const int idddocoperPred = 33;
		const int idddocoperOknaDa = 67;
		const int iddocoperLerua = 34;
		//59	����������
		const int idddocoperRekl = 59;

		// ���� ������ ����������� // 17-12-2013 ����������
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

		/// ������������� �� ���� ��������� �����������: ���������; ���, ��������, ������, etc. :: /// deWrapper
		private static bool isPvh(int idprofsys)
		{
			return (bool) CalcProcessor.Modules["isPvh"].Invoke(new object[] {idprofsys})[0];
		}


		// ReSharper disable once UnusedMember.Global
		public void RunSpreading(dbconn db, Atechnology.winDraw.Spreading_v2.Spreading spreading, ConnectorDefinition conn)
		{
			Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = false;
			if (spreading.Type == SpreadingType.Shpross) // ������ ��� ��������
			{
				if (conn != null) // ��������� �� ������ �� �����������
				{
					if (conn.MasterConture != null) // ������ ��� �������� �������
					{
						foreach (ConnectorLeg Leg in conn.Legs)
						{
							Leg.Length = 0.01; // ������������� �� ����� = 1, 0 �� ����, ��� ������ ��������.
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
				DataRow[] drdocsign = order.ds.docsign.Select("sign_name = '�����' and signvalue_str = '������ �����������' and deleted is NULL");
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

		private const string Al = "��������";
		private const string White = "�����";

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

		// ��������� �����
		public void Run(dbconn _db, string name, clsModel model)
		{
			try
			{
				Model = model;

				try
				{
					// �� ������ ��� ��������� � �������� �������� ��� ��� ��������� ������ ������� ��������� � ��������� = ������ ������ :( ������ ���� �������
					// Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = name != "BeforeAddSpreading" && name != "ChangeDornmas" && name != "ChangeSteel";
					// ������ �������� =  ��� ������ ������� - ������, ��� ������ ������ - ��������

					// ���� ������ �� ����� // �������� �������� ��� 550711 -> 554008(550000)
					Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = false;
					
					//                    Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = (name == "Draw" && model.SelectedBeems.Count >0);
					//				    Atechnology.winDraw.Model.Settings.Settings.DrawProfilePictures = !(model.SelectedRegions.Count > 0);

					// ���������� � �������� �������
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
					// ������� / �������� ������� == 1
					Tools.ChangeSetting("BuilderSettings", "ChoiceProfile", 1);

					// ����������� ���������� ��� ����������/�������� �������
					Tools.ReadSetting("BuilderSettings", "ImplementGlassFill", true);
					model.WinDraw.isImplementGlassFill = true;

					Tools.ChangeSetting("Visibility", "���", true);
					Tools.ChangeSetting("Visibility", "����������", true);
					Tools.ChangeSetting("Visibility", "����������", true);
					Tools.ChangeSetting("Visibility", "��� � �������", false);
					Tools.ChangeSetting("Visibility", "�����", true);
					Tools.ChangeSetting("Visibility", "�������", false);
					Tools.ChangeSetting("Visibility", "��������� �����", false);
					Tools.ChangeSetting("Visibility", "��������������", true);
					Tools.ChangeSetting("Visibility", "�����", true);
					Tools.ChangeSetting("Visibility", "��������� ������", true);
					Tools.ChangeSetting("Visibility", "��� ������", true);
					Tools.ChangeSetting("Visibility", "������������", false);
					Tools.ChangeSetting("Visibility", "������ ���������", false);
					Tools.ChangeSetting("Visibility", "����������", false);
					Tools.ChangeSetting("Visibility", "_arm", false);

					// ��� ����� ������ ���� ���� ����
					Tools.ChangeSetting("Visibility", "������", isArc(Model));
					// ������ ��� ����� ��������, ������� � ����� ���������� �� ������
					Tools.ChangeSetting("Visibility", "������ ���������� ������", false);
					Tools.ChangeSetting("Visibility", "������ ���������� �������", false);

					// ������ �������
					Tools.ChangeSetting("Visibility", "��������� �����", isRetail(Model));
					Tools.ChangeSetting("Visibility", "������ � �������", isRetail(Model));

					Tools.ChangeSetting("Visibility", "�����������", Model.ProfileSystem.Name != ALUTECH62 && Model.ProfileSystem.Name != ALUTECH_48);
					Tools.ChangeSetting("Visibility", "������ ALUTECH", foo_visiblity_osnova_alutech(Model));

					Tools.ChangeSetting("Enabled", "��������� �������", false); // RO
					Tools.ChangeSetting("Enabled", "������������", false); // RO
					Tools.ChangeSetting("Enabled", "������ ���������", false); // RO
					Tools.ChangeSetting("Enabled", "����������", false); // RO
					

					// GIESSE
					// todo as up.visible
					//Tools.ChangeSetting("Visibility", "�������� �� �����", Model.FurnitureSystem.Name != FurnSystGEISSE);
					//Tools.ChangeSetting("Visibility", "������������ �������������", Model.FurnitureSystem.Name != FurnSystGEISSE);
					//Tools.ChangeSetting("Visibility", "����������� �����������", Model.FurnitureSystem.Name != FurnSystGEISSE);
				}
				catch
				{
					// ignored
				}

				/// 91->94 ������� ��� ������ id ������� �� system.serial, � �� idsystem �������� ��
				//				try
				//				{
				//					patch_9194((clsModel) model);
				//				}
				//				catch
				//				{
				//					// ignored
				//				}

				restriction = getRestriction((clsModel) model);

				UserProperties(); //��������� ���������������� ������������� (������� 08-12-2010)

				if (shtapik == "")
					shtapik = Model.UserParameters["������"].StringValue;
				//				if(uplotnenie_color=="")
				uplotnenie_color = Model.UserParameters["���� ����������"].StringValue;


				//// ����� ��������� ������, ���������� ����� � ������������� ����������, �� ��� ����� ������ �� �������� name - �� ��� �� � ��� ��������
				newDoorsFlagAndRecogniserSubroutine((clsModel) model, (wdAction) Enum.Parse(typeof(wdAction), name));

				//// ����� ����������� ������ ���������� / RO-����� �����������
				disableEnable((clsModel) model, (wdAction) Enum.Parse(typeof(wdAction), name));

				// ����� �����
				disableEnableHandleOptions((clsModel) model);

				// ���������� ������������� // todo ����� � ���� ���������� wdAction
				wdAction wdAction = (wdAction) Enum.Parse(typeof(wdAction), name);
				if (wdAction == wdAction.AddPerimetr || wdAction == wdAction.AddConstruction)
				{
					if (model.ModelConnections.Count > 0 || model.WinDraw.ModelConstruction.GetActiveModel() == model) // || ���� ���� �������
					{
						// ����������� �������� ������������ ��� ����
						tryAddBottomConnector(model);
					}
				}

				// ���������: ��� Rising ����� �� �����!
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
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						ChangeUserParamValue();
						break;
					case "RemoveLeaf":
						RemoveLeaf();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						break;
					case "ShowModel":
						ChangeLeafUserParamList();
						ChangeZashchelka2();
						AddDoorMessageIfDoor(Model);
						break;
					case "SelectModel": // select �����������
						//ChangeLeafUserParamList ();
						break;
					case "ChangeConstructionType":
						ChangeConstructionType();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						AddDoorMessageIfDoor(Model);
						break;
					case "AddPerimetr":
						AddPerimetr();
						ChangeProfileSystem();
						ChangeFurnitureSystem();
						ChangeUserParamValue();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						ChangeRadius(); //06.09.2013 ������ �.�.
						break;
					case "AddConstruction":
						AddConstruction();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						AddDoorMessageIfDoor(Model);
						break;
					case "AddImpost":
						AddImpost();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						break;
					case "RemoveImpost":
						RemoveImpost();
						break;
					case "ChangeHandleType": //����� ���� �����
						ChangeLeafUserParamList();
						ChangeZashchelka();
						break;
					case "ChangeOpenType":
						ChangeOpenType();
						ChangeLeafSize();
						break;
					case "ChangeOpenSide": // ������� 2009-04-17
						ChangeOpenSide();
						break;
					case "ChangeOpenView": // ������� 31-07-2011 (������ �������)
						ChangeOpenView();
						break;
					case "ChangeRadius":
						ChangeRadius();
						break;
					case "ChangeHingecount": //���������� ������ �.�. 22.12.2011
						ChangeHinge();
						break;
					case "ChangeHingePosition": //���������� ������ �.�  22.12.2011
						ChangeHinge();
						break;
					case "ChangeHingeType": //���������� ������ �.�  22.12.2011
						ChangeHinge();
						break;
					case "ChangeFill": // ��������� 2009-04-17
						ChangeFill();
						break;
					case "BeforeParametersForm":
						break;
					case "ChangeUserParamValue":
						//��������� ���������������� �������������  ������� 18-09-2011 (������ �������)
						ChangeUserParamValue();
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						break;
					case "ChangeInsColor":
						ChangeColor();
						break;
					case "ChangeOutColor":
						ChangeColor();
						break;
					// ��������� 19.12.2011 - ������ �.�.
					case "ChangeLeafSize":
						ChangeLeafSize();
						ChangeHandlePosition();
						break;
					case "ChangeConnectionType":
						ChangeConnectionType();
						break;
					case "ChangeGabarit": //06.12.2012 ������ �.�.
						ChangeMarkingSteel(); //06.12.2012 ������ �.�.
						ChangeHandlePosition();
						recalcHingeCount();
						break;
					case "AddConnector": //20.12.2012 ������ �.�.
						ConnectorType();
						break;
					case "ChangeHandlePosition":
						ChangeHandlePosition();
						break;
					case "ChangeHandlePositionType":
						ChangeHandlePosition();
						break;

					case "DivisionBeem": // 20-06-2014 ����������
						DivisionBeemProhibit();
						break;

					case "ChangeSteel": // 19-08-2015
						ChangeSteel(Model, wdAction.ChangeSteel);
						break;
				}

				/// ��������� � ��������������, �� ��� ����� �������� ����������, �� ��� ��� �� ���� ������ ��-�������� ����������� �� ������ �������
				/// ������ ������� �������� � ����� ������� � Enum wdAction
				wdAction action = (wdAction) Enum.Parse(typeof(wdAction), name);

				/// ������������� ��������� ����� � ���� ����� �������, 
				/// ��� ����������� ��������������� ��������� ������� �� ���� ������� �� �� ���� ��� ��� ���������������� - ������� ��� ������ � ��� ����� � ����� �������, 
				/// �� ��� ������� ��� � ���������� ����� � ���������� action
				switch (action)
				{
					/// �� ������ �� �������������:

					// ��������� 
					case wdAction.Draw:
						// ����� ��������� 
					case wdAction.AfterModelCalc:
						// ����� ������ �������������
					case wdAction.BeforeParametersForm:
						// ���������� ��������
					case wdAction.AddPerimetr:
						// � �����������
					case wdAction.AddConstruction:
						break;

					/// ������ �������� ��� �������������

					// ����� ����� ���������������� ����������
					case wdAction.ChangeUserParamValue:
						// ����� ������ �������� �������������
					case wdAction.SelectModel:
						// ������� �������� ht < up, �.�. ������������ ����� �������� ��� ����� � ������ ~todo + ����� ������� �� �����������, ������� ���������� ����� ���� � UP ����������� ������ ����������� ���������� ��������, ��� �� �������� ������ �������
					case wdAction.BeforeModelCalc:
						htup((clsModel) model);
						break;

					/// ������ ��� ������������� up < ht
					/// ����� ��������� � �����������, ����� ��� �������, ��� ���� �� ���� �� ����� ����������� 
					default:
						upht((clsModel) model);
						break;
				}

				// ����������, ����� �����������
				switch (action)
				{
					case wdAction.Draw:
					case wdAction.AfterModelCalc:
						break;
					default:
						// ����������
						setInvariants((clsModel) model);
						break;
				}

				// ������� �������� � ������ ��������� �������, ��� �� ����� ����� ����� � ����� � ������ �������� ���������� � ������ classNative
				switch (action)
				{
					/// todo ������� ����������� .AddConstruction � .AddPerimeter ������� ��������, 
					/// ������ ����������� ������ (��������) ��� �� � ���� ��������, �� � ������ ������� 
					/// ���� ���� ���������� ������� �� �������� � ���������, 
					/// �������� �������� �������� ����� � ��� ����� �� ���������� ��������� => ���� ����������
					/// ������� �����������

					// �� �������� ��������� ��� ���������, 
					case wdAction.Draw:
					case wdAction.AddConstruction:
					case wdAction.BeforeParametersForm:
						break;

					// ����� �� �����������: ��������� �������� ������ � ������ �������� �� ������� � SystemScript 
					case wdAction.BeforeModelCalc:
						BeforeModelCalc(model);
						break;

					// ����� ������� �������� ������ � ���������
					case wdAction.AfterModelCalc:
						AfterModelCalc(model);
						break;

					// ��������� ������ ������ �����������, � ����� �������� �������� ����� �����������!, ��� �� ����������� ��� ������� ������� ��� �����������
					default:
						validationProcessor((clsModel) model);
						break;
				}
			}
			catch (Exception ex)
			{
				addErrorUniq(model, string.Format("�������������� ���������� � ������� ����������� {2}\n{0}\n{1}", ex.Message, ex.StackTrace, name));
			}
		}

		private void disableEnableHandleOptions(clsModel model)
		{
			foreach (clsLeaf leaf in model.Leafs)
			{
				clsUserParam upWindowHandleOptions = leaf.UserParameters.GetParam("���������� ������� �����");
				if (upWindowHandleOptions != null)
				{
					// BUG upWindowHandleOptions.Visible = leaf.HandleType == HandleType.�������  && isPVH(model);
					// ���� �������� ����� ����� ����������� �������� ������� ���������������� ����������, ��� �� ���� ����������� ����� ������ ����������� ���� �����
					upWindowHandleOptions.Visible &= leaf.HandleType == HandleType.�������;

					if (leaf.HandleType != HandleType.������� || !upWindowHandleOptions.Visible)
					{
						upWindowHandleOptions.StringValue = upWindowHandleOptions.DefaultValue.StrValue;
					}
				}
			}
		}


		#region �������� ������� �� �������

		#region ������ ������� ����

		private void DivisionBeemProhibit()
		{
			foreach (clsBeem beem in Model.Frame)
			{
				if (beem.R1 > 0 && beem.Selected && restriction)
				{
					MessageBox.Show("���� �� ������!");
					Model.Undo.Undo();
					break;
				}
			}
		}

		#endregion

		#region ���������� ������������� �������		

		protected void tryAddBottomConnector(clsModel model)
		{
			if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balconGlazing)
			{
				if (!isBottomFree(model))
					return;

				if (haveModelBottomThis(model))
					return;

				// ����� ������������ �������
				foreach (clsProfile profile in model.ProfileSystem.colProfileConnectors)
				{
					if (profile.Visible && profile.ConnectortypeName != null && profile.ConnectortypeName == "������������ �������")
					{
						// ��������� ��� � ������
						foreach (clsBeem beem in model.Frame)
							// �������� ���� ������
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

		#endregion ���������� ������������� ������� 

		protected void AddDoorMessageIfDoor(clsModel model)
		{
			if (model.ConstructionType.Name == _outdoor)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "����� ������� ��������� `��� �������`";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}

			if (model.ConstructionType.Name == _moskit)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "��������� ����� ��������� �� ��������� �����";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}

			// TODO REFACTOR ��� ������
			if (model.ProfileSystem.Name == Gealan)
			{
				WinDrawMessage message = new WinDrawMessage();
				message.ID = "door";
				message.Message = "          " + "�� ��������� � Gealan ������������ 2 ������� ����������!";
				message.Model = model;
				message.CanClose = true;
				model.WinDraw.AddMessage(message);
			}
		}

		#region ���������� ���������

		public void AddPerimetr()
		{
			// TODO remove
			// Model.UserParameters["����������"].StringValue = "��";
			// TODO don't remove
			setNewMech(Model, true);

			// todo refactor in FillRestriction
			// �� ��������� ��� Vario
			if (Model.ProfileSystem.Name == Vario || Model.ProfileSystem.Name == DECOR || Model.ProfileSystem.Name == RehauDeluxe)
				Model.UserParameters["�������� �� �����"].StringValue = "����������";

			if (Model.ProfileSystem.Name != SystSlayding60 && Model.ConstructionType.Name != _swingdoor)
			{
				// todo refactor to MoskitRestriction
				if (Model.ConstructionType.Name.ToLower().Contains("��������� �����"))
				{
					Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystBEZ);
					Model.VisibleRegions[0].Selected = true;
					Model.Leafs.Add();

					clsLeaf lf = Model.Leafs[0];
					lf.OpenType = OpenType.������;
					lf.IsMoskit = IsMoskit.����;
					// refactored Model.VisibleRegions[0].Fill = Model.ProfileSystem.Fills["���_������_�_�������"];
					foreach (clsUserParam up in lf.UserParameters)
						if (!up.Name.Contains("�����"))
							up.Visible = false;
				}

				if (Model.ProfileSystem.Name == "Rehau DD" || Model.ProfileSystem.Name == "Rehau SD" || Model.ProfileSystem.Name == "Rehau BRD" || Model.ProfileSystem.Name == "Rehau 70��")
				{
					Model.UserParameters["������"].StringValue = "�����������";
				}
				else if (Model.ProfileSystem.Name == Solar || Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
				{
					ChangeUserParamValue(); //�������� 18-09-2011 (������ �������)
				}
				else
				{
					Model.UserParameters["������"].StringValue = "���������";
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

		#region ���������� �����������

		public void AddConstruction()
		{
			// ��� �� ������������� �� ��������� ������
			if (Model.ConstructionType.Name == _balcon)
			{
				clsUserParam up = Model.UserParameters.GetParam("�������������");
				if (up != null)
					up.StringValue = "������";
			}

			if (Model.ProfileSystem.Name == SystSlayding60 || Model.ConstructionType.Name == _swingdoor)
			{
				ChangeProfile();
				ChangeFill();
				ChangeColor();
			}
		}

		#endregion

		#region ���������� �������

		public void AddImpost()
		{
			ProfileConnection();

			if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
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

		#region ������������ ��������

		public void ImpostAlignment()
		{
			foreach (clsImpost imp in Model.Imposts)
			{
				imp.Selected = true;
			}

			Model.Imposts.Alignment(AlignType.LightOpening);
		}

		#endregion

		#region �������� �������

		public void RemoveImpost()
		{
			if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
			{
				ChangeFill();
			}
		}

		#endregion

		#region ���������������� ��������� �������

		void ChangeLeafUserParamList()
		{
			if (Model.ProfileSystem.Name != SystSlayding60 && Model.ConstructionType.Name != _swingdoor)
			{
				foreach (clsLeaf lf in Model.Leafs)
				{
					ChangeLeafUserParam(lf);
				}
			}
			else if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
			{
				ChangeProfile();
			}
		}

		#endregion

		#region ���������������� ��������� ������� - ����� �������

		void ChangeZashchelka()
		{
			//��������� ����� ���� ����� � ������� � ���������
			foreach (clsLeaf lf in Model.Leafs)
			{
				//MessageBox.Show("���������������� ��������� �������"+lf.UserParameters["������� ������"].NumericValue.ToString()+lf.HandleType.ToString());
				if (
					Model.ConstructionType.Name.Contains("����� �������") ||
					Model.ConstructionType.Name.Contains("����� ������������") ||
					Model.ConstructionType.Name.Contains("����� �����������")
					)
				{
					if (lf.HandleType.ToString() == "���_�����")
					{
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "�������")
					{
						MessageBox.Show("��� ''����� �������'' - ��� ����� ''�������'' - �� ������������!");
						lf.HandleType = HandleType.���_�����;
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "��������_��������")
					{
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "�������")
					{
						lf.UserParameters["��� �������"].StringValue = "���������";
					}
				}
			}
		}

		#endregion

		#region ���������������� ��������� ������� - ����� ������� ��� ������ �������

		void ChangeZashchelka2()
		{
			//��������� ����� ���� ����� � ������� � ���������
			foreach (clsLeaf lf in Model.Leafs)
			{
				//MessageBox.Show("���������������� ��������� �������"+lf.UserParameters["������� ������"].NumericValue.ToString()+lf.HandleType.ToString());
				if (
					(
					Model.ConstructionType.Name.Contains("����� �������") ||
					Model.ConstructionType.Name.Contains("����� ������������") ||
					Model.ConstructionType.Name.Contains("����� �����������")
					)
					&&
					(lf.UserParameters["��� �������"].StringValue != "�������") &&
					(lf.UserParameters["��� �������"].StringValue != "���������") /*&&
					(lf.UserParameters["��� �������"].StringValue!="���_�������")*/)
				{
					if (lf.HandleType.ToString() == "���_�����")
					{
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "�������")
					{
						MessageBox.Show("��� ''����� �������'' - ��� ����� ''�������'' - �� ������������!");
						lf.HandleType = HandleType.���_�����;
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "��������_��������")
					{
						lf.UserParameters["��� �������"].StringValue = "�������";
					}

					if (lf.HandleType.ToString() == "�������")
					{
						lf.UserParameters["��� �������"].StringValue = "���������";
					}
				}
			}
		}

		#endregion

		#region ��������� ���������������� ������������� �������

		void ChangeLeafUserParam(clsLeaf lf)
		{
			// � ��������� ����� : lf.HandleBeem == null // 22-05-2015 ����������
			if (Model.ConstructionType.Name == "��������� �����")
			{
				foreach (clsUserParam up in lf.UserParameters)
					if (up.Name != "��������� �����")
						up.Visible = false;
			}
			else
			{
				//��� ������� ������� �������� �������� �� ��������� ����� 40 ��. ��������� 2011-07-24 (������ �������)
				if (lf.HandleBeem.Profile.Marking == "541130" || lf.HandleBeem.Profile.Marking == "541150" || lf.HandleBeem.Profile.Marking == "541340" || lf.HandleBeem.Profile.Marking == "541350" || lf.HandleBeem.Profile.Marking == "550170-701" || lf.HandleBeem.Profile.Marking == "550170-908")
				{
					lf.UserParameters["������� �����"].StringValue2 = "������� 40 ��";
				}

				if (lf.HandleBeem.Profile.Marking == "550510-701" || lf.HandleBeem.Profile.Marking == "550510-908" || lf.HandleBeem.Profile.Marking == "550400-701" || lf.HandleBeem.Profile.Marking == "550400-908")
				{
					lf.UserParameters["������� �����"].StringValue2 = "������� 28 ��";
				}
			}


			// +DM25 ��� ROTO NT ���������� 17-03-2014
			if (lf.UserParameters["��� �����"] != null)
				lf.UserParameters["��� �����"].Visible =
					(Model.FurnitureSystem.Name == RotoNT || Model.FurnitureSystem.Name == RotoNTDesigno)
					&& Model.ConstructionType.Name == "��������� �����"
					&& lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf
					&& Convert.ToInt32(lf.HandleBeem.Profile.A) - 40 >= 54;

			if (lf.UserParameters["������� �����"] != null)
				lf.UserParameters["������� �����"].Visible =
					(Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && (Convert.ToInt32(lf.HandleBeem.Profile.A) < 94 || Model.FurnitureSystem.Name == SiegeniaTitan || Model.FurnitureSystem.Name == SiegeniaTitanWK1); // 27-03-2015 // 02.04.2014 ����������

			if (lf.UserParameters["�������� ���������"] != null)
				lf.UserParameters["�������� ���������"].Visible = (Model.ConstructionType.Name.Contains("�����") && lf.HandleType == HandleType.��������_�������� && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			if (lf.UserParameters["������� �����"] != null)
				lf.UserParameters["������� �����"].Visible = (Model.ConstructionType.Name.Contains("�����") && lf.HandleType == HandleType.������� && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			lf.UserParameters["��������� �����"].Visible = (Model.ConstructionType.Name == "��������� �����" && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			lf.UserParameters["�������� �������"].Visible =
				Model.ConstructionType.Name.ToLower().Contains("�����")
				&& lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf
				&& (lf.HandleBeem.Profile.A >= 94 || Model.FurnitureSystem.Name.ToLower().Contains("��������"));


			lf.UserParameters["������������ �������"].Visible = (Model.ConstructionType.Name == "����" && lf.OpenType == OpenType.��������);
			lf.UserParameters["������������ �������������"].Visible = ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.OpenType == OpenType.���������_��������);
			lf.UserParameters["����������� �����������"].Visible = ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.OpenType == OpenType.���������_��������);
			lf.UserParameters["��������� ���������� �������"].Visible = ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf);
			lf.UserParameters["������� �����"].Visible = (Model.ConstructionType.Name.Contains("�����") && ((Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 || Model.ProfileSystem.Name.ToLower().Contains("������")) && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			//lf.UserParameters["������������� �����"].Visible = ((Model.ConstructionType.Name == "����� ������������" || Model.ConstructionType.Name == "����� �������") && (Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf);

			if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
			{
				if (Model.ProfileSystem.Name.ToLower().Contains("������"))
					lf.UserParameters["������� �����"].StringValue2 = "������� 35 ��";


				if (lf.OpenType != OpenType.����������)
					lf.OpenType = OpenType.����������;

				lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;

				lf.HingeType = HingeType.DoorHinge;

				if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
					lf.HingePosition = HingePosition.HingeBorderEven;
				else
					lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;
			}
		}

		#endregion

		#region ����� ���� ����������

		void ChangeOpenType()
		{
			//MessageBox.Show("����� ���� ����������");
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (!Model.ProfileSystem.Name.ToLower().Contains("��������-60") && !Model.ConstructionType.Name.Contains("�������"))
				{
					lf.HingeCount = (int) lf.UserParameters["������� ������"].NumericValue;

					if (lf.SelectedBeems.Count == 0)
						continue;

					lf.UserParameters["������������ �������������"].Visible = lf.OpenType == OpenType.���������_��������;

					lf.UserParameters["������������ �������"].Visible = lf.OpenType == OpenType.��������;

					lf.UserParameters["����������� �����������"].Visible = ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.OpenType == OpenType.���������_��������);

					if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
					{
						//////����� ������������� ��������� ��� ������
						if (Model.ConstructionType.Name.Contains("�������"))
							lf.HandleType = HandleType.�������;

						if (lf.OpenType != OpenType.����������)
							lf.OpenType = OpenType.����������;

						lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;

						lf.HingeType = HingeType.DoorHinge;

						if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
							lf.HingePosition = HingePosition.HingeBorderEven;
						else
							lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;
					}
					else
					{
						if (Model.FurnitureSystem.Name.ToLower().Contains("�������") && !Model.ConstructionType.Name.ToLower().Contains("������������") && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG ����� ������ ��������� ��� ����� ���������� ��� ������� ���� ����������� - ��� �� ���������

						lf.UserParameters["������� ������"].Visible = false;

						if (Model.ConstructionType.Name.ToLower().Contains("������������"))
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
				else if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
				{
					lf.OpenType = OpenType.����������;
				}
			}
		}

		#endregion

		#region ����� ����������� ��������� �������

		// http://yt:8000/issue/dev-253
		void ChangeOpenSide()
		{
			// ��������� 2009-04-17
			//MessageBox.Show("����� ����������� ��������� �������");
			if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
			{
				foreach (clsLeaf lf in Model.Leafs)
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Frame)
					{
						lf.OpenSide = (lf.OpenSide == OpenSide.�����) ? OpenSide.������ : OpenSide.�����;
					}
					else if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Impost && lf.HandleBeem.ConnectBeemLine.Beem.bType == ComponentType.Impost)
					{
						if (lf.HandleBeem.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("���"))
							lf.OpenSide = (lf.OpenSide == OpenSide.�����) ? OpenSide.������ : OpenSide.�����;
					}

					foreach (clsLeafBeem lfb in lf)
					{
						if (lf.OpenSide == OpenSide.������)
						{
							//������� ���������� ����� �������
							switch (lfb.PositionBeem.ToString())
							{
								case "Bottom":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.��������;
									break;

								case "Left":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									lfb.ConnectType = ConnectType.�������;
									break;

								case "Top":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.��������;
									break;

								case "Right":
									if (lfb.ConnectBeemLine.Beem.bType.ToString() == "Impost" && !lfb.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("���"))
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									else
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5064.07"]; // ROS0679.07
									lfb.ConnectType = ConnectType.�������;
									break;
							}
						}
						else if (lf.OpenSide == OpenSide.�����)
						{
							switch (lfb.PositionBeem.ToString())
							{
								case "Bottom":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.��������;
									break;

								case "Left":
									if (lfb.ConnectBeemLine.Beem.bType.ToString() == "Impost" && !lfb.ConnectBeemLine.Beem.Profile.Marking.ToLower().Contains("���"))
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									else
										lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5064.07"]; // ROS0679.07
									lfb.ConnectType = ConnectType.�������;
									break;

								case "Top":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5065.07"]; // ROS0680.07
									lfb.ConnectType = ConnectType.��������;
									break;

								case "Right":
									lfb.Profile = Model.ProfileSystem.colProfileLeaf["ROS5063.07"]; // ROS0678.07
									lfb.ConnectType = ConnectType.�������;
									break;
							}
						}
					}
				}
			}
		}

		#endregion

		#region ����� ���� ��������� ������� ������/������

		// ������ '% T%' � �� '%T%' ��� ����������� '������� Z60 BrillianTDesign' � '������� Z60 AnTik'
		private const string T = " T";
		private const string Z = " Z";

		private void ChangeOpenView()
		{
			if (Model.ConstructionType.Name.ToLower().Contains("����� �������") || Model.ConstructionType.Name.ToLower().Contains("����� ������������"))
			{
				foreach (clsLeaf leaf in Model.Leafs.SelectedLeafs)
				{
					if (leaf.OpenView == OpenView.������ && leaf.HingBeem.Profile.Comment.ToUpper().Contains(T))
					{
						// ������ ������� �� Z
						invertProfile(leaf);
					}

					if (leaf.OpenView == OpenView.������ && leaf.HingBeem.Profile.Comment.ToUpper().Contains(Z))
					{
						// ������ ������� �� T
						invertProfile(leaf);
					}
				}
			}
		}

		void invertProfile(clsLeaf leaf)
		{
			try
			{
				/// TODO !! ��� ������ ����� � �������, ���������� �� �� �������
				clsProfile profile2 = getConjugateProfile(leaf.Model, leaf.HingBeem.Profile);
				foreach (clsBeem beem in leaf)
				{
					beem.Profile = profile2;
				}

				MessageBox.Show(string.Format("���������� {0} ������������� ������� {1} {2}", leaf.OpenView, profile2.Marking, profile2.Comment), "����������c��� ����� ���� ������� Z | T", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (ArgumentException)
			{
				MessageBox.Show(string.Format("� ������� {0} {1} �� ������� ����������� ��� ���������� {2}", leaf.HingBeem.Profile.Marking, leaf.HingBeem.Profile.Comment, leaf.OpenView), "����������c��� ����� ���� ������� Z | T", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Model.Undo.Undo();
			}
		}

		static clsProfile getConjugateProfile(clsModel model, clsProfile profile) // throws  NoConjugateProfileEx()
		{
			// �������� ���������� -908 ��� string.Empty
			string ext = getProfileMarkingExt(profile);

			// ���
			string TZ = string.Empty;
			if (profile.Comment.ToUpper().Contains(T))
				TZ = Z;
			else if (profile.Comment.ToUpper().Contains(Z))
				TZ = T;
			else
				throw new ArgumentException();

			foreach (clsProfile p in profile.ProfileSystem.colProfileLeaf)
			{
				// ���� ���� ���������� ��������� ��� � �����������
				if (ext != string.Empty && getProfileMarkingExt(p) != ext)
					continue;

				// ������� .A
				if (profile.RefA != p.RefA)
					continue;

				// ������� � ���� ����������� ����� � �� ������ � ������ ��������� ��� ����� ���� ����������� => ����� !
				if (p.Comment.ToUpper().Contains(TZ) && model.ConstructionType._Leaf.Contains(p))
					return p;
			}

			// ���
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

		#region ���������� �������

		void AddLeaf()
		{
			foreach (clsLeaf lf in Model.Leafs.SelectedLeafs)
			{
				// ��� Vario �� ��������� ���� ����� ����������
				if (Model.ProfileSystem.Name == Vario || Model.ProfileSystem.Name == DECOR || Model.ProfileSystem.Name == RehauDeluxe)
				{
					lf.UserParameters["��������� �����"].StringValue2 = "����������";
					lf.UserParameters["������� �����"].StringValue = "����������";
					lf.UserParameters["��������� �����"].StringValue = "����������";
					lf.UserParameters["������� �����"].StringValue = "����������";
					lf.UserParameters["�������� ���������"].StringValue = "����������";

					lf.UserParameters[__mechanizm].StringValue2 = __brown;
				}

				if (!Model.ProfileSystem.Name.ToLower().Contains("��������-60") && !Model.ConstructionType.Name.Contains("�������"))
				{
					if ((Model.ConstructionType.Name == "����� �������" || Model.ConstructionType.Name == "����� ������������") && lf.OpenType != OpenType.����������)
						lf.OpenType = OpenType.����������;

					if (Model.ConstructionType.Name == "����� �������")
					{
						if (lf.HandleType == HandleType.�������)
							lf.HandleType = HandleType.��������_��������;

						if (!Model.ColorInside.ToString().ToLower().Contains("�����") || !Model.ColorOutside.ToString().ToLower().Contains("�����"))
							lf.UserParameters["������� ������"].StringValue2 = "����������";
						else
							lf.UserParameters["������� ������"].StringValue2 = "�����";
					}

					if ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.HandleType != HandleType.�������)
						lf.HandleType = HandleType.�������;

					if (Model.ConstructionType.Name == "����")
					{
						lf.UserParameters["������� �����"].Visible = true;
						lf.UserParameters["��� �����"].Visible = false;
						lf.UserParameters["�������� �������"].StringValue = "���";
						if (lf.OpenType == OpenType.���������_��������)
							lf.UserParameters["������������ �������������"].Visible = true;
						else
							lf.UserParameters["������������ �������������"].Visible = false;
						if (lf.OpenType == OpenType.��������)
							lf.UserParameters["������������ �������"].Visible = true;
						else
							lf.UserParameters["������������ �������"].Visible = false;
					}
					else if (Model.ConstructionType.Name == "��������� �����")
					{
						lf.UserParameters["�������� �������"].StringValue = "���";
						lf.UserParameters["������� �����"].Visible = false;

						if (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
						{
							lf.UserParameters["��� �����"].Visible = false;
							lf.UserParameters["��������� �����"].Visible = false;
							lf.UserParameters["��������� �����"].StringValue = "���";
						}
						else
						{
							lf.UserParameters["��� �����"].Visible = true;
							lf.UserParameters["��������� �����"].Visible = true;
							//lf.UserParameters["��������� �����"].StringValue = "�����";
						}
					}
					else if (Model.ConstructionType.Name == "����� ������������")
					{
						lf.UserParameters["�������� �������"].StringValue = "�������";
					}
					else if (Model.ConstructionType.Name == "����� �������")
					{
						lf.UserParameters["�������� �������"].StringValue = "�������";
					}
					else if (Model.ConstructionType.Name == "��������� �����")
					{
						if (lf.OpenType != OpenType.������)
							lf.OpenType = OpenType.������;
						lf.IsMoskit = IsMoskit.����;
						// refactored Model.VisibleRegions[0].Fill = Model.ProfileSystem.Fills["���_������_�_�������"];
					}

					if (Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == _balconGlazing || Model.ConstructionType.Name == "��������� �����")
						lf.UserParameters["��������� �����"].StringValue = "�������";
					else
						lf.UserParameters["��������� �����"].StringValue = "�������";

					if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
					{
						if (!Model.ProfileSystem.Name.ToLower().Contains("������"))
						{
							if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
								Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");
						}
						else
						{
							if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || !Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
								Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� ������� (��������)");
							lf.UserParameters["������� ������"].StringValue = "GIESSE";
						}

						if (lf.OpenType != OpenType.����������)
							lf.OpenType = OpenType.����������;

						if (Model.FurnitureSystem.Name.ToLower().Contains("��������") || Model.ConstructionType.Name.ToLower().Contains("�������"))
							lf.HingeCount = 3;
						lf.HingeType = HingeType.DoorHinge;

						if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
							lf.HingePosition = HingePosition.HingeBorderEven;
						else
							lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

						lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;

						// ������ ��������� ���  ������ ������ Alutech
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
						// ��������� �����
					else if (Model.ConstructionType.Name == _wcdoor)
					{
						// ���������

						// ��������� �������
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystDver);

						// ����� ������� RotoNT 3 ��
						lf.HingeCount = 3;
						lf.HingePosition = HingePosition.HingeProportional;
						lf.HingeType = HingeType.WindowHinge;

						// �� ��������� �������� ������������ ����� ����/����
						lf.HandleType = HandleType.��������_��������;
						lf.HandlePositionType = HandlePositionType.�����������;
						lf.UserParameters[__mechanizm].StringValue = __keykey1;

						// ������ ����������
						lf.OpenType = OpenType.����������;
					}
						// ������ ������������ ������� �������������� ?
					else
					{
						if (Model.FurnitureSystem.Name.ToLower().Contains("�������") && !Model.ConstructionType.Name.ToLower().Contains("������������") && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG ��� �� �������� �������� ���� �������� � ���� ��� ����� ����� �� ���������� ������� �������, ��� ?
						else if (Model.ConstructionType.Name.ToLower().Contains("������������"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");

						lf.UserParameters["������� ������"].Visible = false;

						if (Model.ConstructionType.Name == _indoor)
						{
							lf.HingeCount = 3;
							lf.HingePosition = HingePosition.HingeProportional;
							lf.HingeType = HingeType.WindowHinge;

							// �� ��������� �������� ������������ ����� ����/����
							lf.HandleType = HandleType.��������_��������;
							lf.HandlePositionType = HandlePositionType.�����������;
							lf.UserParameters[__mechanizm].StringValue = __keykey1;
						}
						else
						{
							lf.HingeType = HingeType.WindowHinge;
						}
					}

					ChangeLeafUserParam(lf);
				}
				else if (Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
				{
					//lf.OpenType = OpenType.����������;

					ChangeOpenType();
					ChangeOpenSide();
					ChangeFill();
					ImpostAlignment();
					ChangeProfile();
					//AtMessageBox.Show(Model.SelectedRegions.ToString());
				}
				else
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.bType == ComponentType.Leaf && Model.ConstructionType.Name.Contains("�������"))
					{
						MessageBox.Show("�������2");
						lf.Selected = true;
						Model.Leafs.Remove();
					}

					RemoveLeaf();
				}
			}

			// ��������� 19.12.2011 - ������ �.� 
			ChangeLeafSize();
			ChangeFill();
		}

		#endregion

		#region �������� �������

		void RemoveLeaf()
		{
			if (Model.ProfileSystem.Name.ToLower().Contains("��������-60") && !Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
			{
				//MessageBox.Show("��������! ������ ��������� ������� � ��������� �� ��������� ������������� �������.");
				foreach (clsLeaf lf in Model.Leafs)
				{
					lf.Selected = true;
				}

				Model.Leafs.Remove();
			}
			else if (Model.ProfileSystem.Name.ToLower().Contains("��������-60") && Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
			{
				ImpostAlignment();
			}
		}

		#endregion

		#region ����� ����� �����������

		private void ChangeConstructionType()
		{
			// ���� ��������� � ���� - �������� ������������
			if (Model.ConstructionType.Name == _window || Model.ConstructionType.Name == _balconGlazing)
				tryAddBottomConnector(Model);

			//			AtMessageBox.Show("����� ����� �����������");
			ChangeProfile();

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.HandleBeem.Profile.Marking == "550170-908" || lf.HandleBeem.Profile.Marking == "550170-701")
				{
					lf.OpenView = OpenView.������;
					//					MessageBox.Show("�������������� ����� ���� ����������! ��� ������� T ������������� ���������� '������ (�� ����)'");
				}

				//<--12.01.2012 �.�������
				if (Model.ConstructionType.Name == "����� �������" || Model.ConstructionType.Name == "����� ������������"
					|| Model.ConstructionType.Name == "��������� �����")
					lf.UserParameters["�������� �������"].StringValue = "�������";
				//-->
				if ((Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == "��������� �����") && lf.HandleType != HandleType.������� && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
				{
					lf.HandleType = HandleType.�������;
				}

				if ((Model.ConstructionType.Name == "����� �������" || Model.ConstructionType.Name == "����� ������������") && lf.HandleType == HandleType.�������)
					lf.HandleType = HandleType.��������_��������;

				if ((Model.ConstructionType.Name == "����� �������" || Model.ConstructionType.Name == "����� ������������") && lf.OpenType != OpenType.����������)
				{
					lf.OpenType = OpenType.����������;
				}

				if (Model.ConstructionType.Name == "����" || Model.ConstructionType.Name == _balconGlazing || Model.ConstructionType.Name == "��������� �����")
				{
					lf.UserParameters["��������� �����"].StringValue = "�������";
				}
				else
				{
					lf.UserParameters["��������� �����"].StringValue = "�������";
				}

				if (Model.ConstructionType.Name == "��������� �����" && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
					lf.UserParameters["��� �����"].Visible = true;
				else
					lf.UserParameters["��� �����"].Visible = false;

				if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("������"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || !Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� ������� (��������)");
						lf.UserParameters["������� ������"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.����������)
						lf.OpenType = OpenType.����������;

					lf.UserParameters["������� ������"].Visible = true;
					lf.HingeCount = 3;
					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;
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

					lf.UserParameters["������� ������"].Visible = false;

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

		#region ����� �������

		void ChangeProfile()
		{
			/// Default ==> ThermoLock + Antik => model.shtapic = �������  
			/// ����� ����� ���� �������� �� �������� ����� ��������
			if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
			{
				foreach (clsLeaf leaf in Model.Leafs.SelectedLeafs)
				{
					if (leaf.BaseRegion.Fill.Thikness == 24)
					{
						if (leaf[0].Profile.Marking == RehauZ60AntikInvariant.__AntikMarking)
						{
							Model.UserParameters["������"].StringValue = "�����������";
							foreach (clsModel otherModel in Model.GetMasterConstruction().GetModelArray())
							{
								if (otherModel != Model && otherModel.Leafs.Count == 0)
								{
									otherModel.UserParameters["������"].StringValue = "�����������";
								}
							}
						}
						else if (!RehauZ60AntikInvariant.isAntik(Model))
						{
							Model.UserParameters["������"].StringValue = "��������";
							foreach (clsModel otherModel in Model.GetMasterConstruction().GetModelArray())
							{
								if (otherModel != Model && otherModel.Leafs.Count == 0)
								{
									otherModel.UserParameters["������"].StringValue = "��������";
								}
							}
						}
					}
				}
			}

			/// ��� ��������� ( in / out ) � ����������� �� �������
			/// ������� �� ����������� � ��������
			foreach (clsLeaf lf in Model.Leafs.SelectedLeafs)
			{
				bool f = false;

				if (lf.HandleBeem.Profile.Comment.ToUpper().Contains(Z) && lf.OpenView != OpenView.������)
				{
					lf.OpenView = OpenView.������;
					f = true;
				}

				if (lf.HandleBeem.Profile.Comment.ToUpper().Contains(T) && lf.OpenView != OpenView.������)
				{
					lf.OpenView = OpenView.������;
					f = true;
				}

				if (f)
					MessageBox.Show(string.Format("��� ������� {0} {1} ������������� ���������� {2}", lf.HandleBeem.Profile.Marking, lf.HandleBeem.Profile.Comment, lf.OpenView), "�������������� ����� ���� ����������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}


			if (Model.ProfileSystem.Name.ToLower().Contains("��������-60") && !Model.FurnitureSystem.Name.ToLower().Contains("��������-60") && Model.Leafs.Count > 0)
			{
				MessageBox.Show("��������! ������ ��������� ������� � ��������� �� ��������� ������������� �������.");
				RemoveLeaf();
			}

			/// �������� / ����������� ������������ �������� ������� ��� ��������� �������,  ��� ���������� :((( ����������
			if (!Model.ProfileSystem.Name.ToLower().Contains("��������-60") && !Model.ConstructionType.Name.Contains("�������") && Model.ConstructionType.Name != _pskportal)
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
								im.ConnectedLeaf[0].OpenType = OpenType.���������_��������;
							else
								im.ConnectedLeaf[1].OpenType = OpenType.���������_��������;
						}
						else
						{
							im.ConnectedLeaf[0].OpenType = OpenType.����������;
							im.ConnectedLeaf[1].OpenType = OpenType.����������;
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

			//��������� ������ ��������� 31.05.2011 - ������ �.�.
			ProfileConnection();
			// ��������� 19.12.2011 - ������ �.� 
			ChangeLeafSize();
			ChangeConnectionType();
		}

		#endregion

		#region ����� ����������

		private void ChangeFill()
		{
			OrderClass order = Model.WinDraw.DocClass as OrderClass;
			bool reklamacia = order != null && order.DocRow != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 59;

			//����������� ����������� ������������ ���������� � ���������� ������� (������� 02-02-2011) // ��� �� �������� ��� ������ ������� � ������ !!! ������� !
			if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 24 && r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("�") && !r.Fill.Marking.ToLower().Contains("top n") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("SG") && !r.Fill.Marking.Contains("4ULTRA") && !r.Fill.Marking.Contains("CGS") && !r.Fill.Marking.Contains("LifeClass"))
					{
						if (!reklamacia)
						{
							MessageBox.Show("��������! ����� ������������ ������ �/� c ����������������� ������� �4 ��� �������");
							r.Fill = Model.ProfileSystem.Fills["4�16��4"];
							// ������ ��������� ������ � ���� ����������	
						}
					}
					else if (r.Fill.Thikness == 32 || r.Fill.Marking.ToLower().Contains("elm5047.07"))
						Model.UserParameters["������"].StringValue = "���������";
					else if (RehauZ60AntikInvariant.isAntik(Model))
					{
						if (Model.UserParameters["������"].StringValue != "�����������" && Model.UserParameters["������"].StringValue != "��������")
						{
							Model.UserParameters["������"].StringValue = "�����������";
						}
					}
					else
						Model.UserParameters["������"].StringValue = "��������";
				}
			}
			else if (Model.ProfileSystem.Name == RehauEuro70)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("�4") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("CGS")) /// && !r.Fill.Marking.ToLower().Contains("cgs")  
					{
						if (!reklamacia)
						{
							MessageBox.Show("��������! ����� ������������ ������ �/� 32�� � ����������������� ������� �4 ��� �������");
							r.Fill = Model.ProfileSystem.Fills["4�24��4"];
						}
					}
				}
			}
			else if (Model.ProfileSystem.Name == Solar)
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if ((r.Fill.Thikness != 32) || !r.Fill.Marking.ToLower().Contains("�") && !r.Fill.Marking.ToLower().Contains("top n") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047.07") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.ToLower().Contains("sg s") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.Contains("EN") && !r.Fill.Marking.Contains("4MF") && !r.Fill.Marking.Contains("6MF") && !(r.Fill.IDGroup == 80 && r.Thickness == 32) && !r.Fill.Marking.Contains("CGS")) /// && !r.Fill.Marking.ToLower().Contains("cgs") 
					{
						if (!reklamacia)
							if (restriction)
							{
								MessageBox.Show("����� ������������ ������ �/� 32�� � ����������������� ������� �4 ��� �������", "��������!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
								r.Fill = Model.ProfileSystem.Fills["4�10�4�10��4"];
							}
					}
				}
			}
			else if (Model.ProfileSystem.Name == Vario)
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if (r.Fill.Thikness != 32 || (!r.Fill.Marking.ToLower().Contains("�") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.ToLower().Contains("cgs") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.ToLower().Contains("sg s") && !r.Fill.Marking.Contains("EN") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("CGS") && !r.Fill.Marking.Contains("ULTRA")))
					{
						if (!reklamacia)
							if (restriction)
							{
								MessageBox.Show("����� ������������ ������ �/� 32�� � ����������������� ������� �4 ��� �������", " ��������!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
								r.Fill = Model.ProfileSystem.Fills["4�10�4�10��4"];
							}
					}
				}
			//			else if (Model.ProfileSystem.Name == ProfSystThermo70)
			//				foreach (clsRegion r in Model.VisibleRegions)
			//				{
			//					if (r.Fill.Thikness != 32 || (!r.Fill.Marking.ToLower().Contains("�") && !r.Fill.Marking.ToLower().Contains("cos") && !r.Fill.Marking.ToLower().Contains("elm5047") && !r.Fill.Marking.Contains("StNeo") && !r.Fill.Marking.ToLower().Contains("4(sg rb)") && !r.Fill.Marking.ToLower().Contains("4(sg s)") && !r.Fill.Marking.ToLower().Contains("4�10�4�10�4") && !r.Fill.Marking.Contains("MF") && !r.Fill.Marking.Contains("4ULTRA")  && !r.Fill.Marking.Contains("CGS")))  /// && !r.Fill.Marking.ToLower().Contains("cgs") 
			//					{
			//						if (!reklamacia && restriction)
			//						{
			//							MessageBox.Show("����� ������������ ������ �/� 32�� � ����������������� ������� �4 ��� �������", " ��������!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
			//							r.Fill = Model.ProfileSystem.Fills["4�10�4�10��4"];
			//						}
			//					}
			//				}

			int addthick = 0;
			//������ � ����� ����������� ���������/�������� 18-09-2011 (������ �������)
			// if (Model.ProfileSystem.Name.ToLower().Contains("rehau") || Model.ProfileSystem.Name.ToLower().Contains("thermolock") /*||Model.ProfileSystem.Name.ToLower().Contains("solar")*/)
			if (Model.ProfileSystem.Name == Rehau60
				|| Model.ProfileSystem.Name == Rehau70
				|| Model.ProfileSystem.Name == Rehau70mm
				|| Model.ProfileSystem.Name == RehauEuro70
				|| Model.ProfileSystem.Name == ThermoLock
				|| Model.ProfileSystem.Name == Classic)
			{
				if (Model.UserParameters["���� ����������"].StringValue == "�����")
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
						Model.UserParameters["������"].StringValue = "�����������";
						shtapik = Model.UserParameters["������"].StringValue;
					}
					else if (Model.VisibleRegions[0].Fill.Thikness >= 22 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 25 + addthick && Model.ProfileSystem.Name != Solar && Model.ProfileSystem.Name != Rehau70)
					{
						/// 30-03-2015 ����������
						/// ��� Rehau Z60 Antik ����� ��������� ������� ����� ������ 14.5 ��, ������� ��� ������ ������ �� ����� ��������� �������� �������
						/// ��� ��������� ��� �� ��������
						if (Model.ProfileSystem.Name == Rehau60 || Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic)
						{
							if (Model.UserParameters["������"].StringValue != "��������" && Model.UserParameters["������"].StringValue != "�����������")
								Model.UserParameters["������"].StringValue = "��������";
						}
						else
						{
							Model.UserParameters["������"].StringValue = "��������";
						}

						shtapik = Model.UserParameters["������"].StringValue;
					}
					else if (Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick && Model.ProfileSystem.Name.ToLower().Contains("rehau"))
					{
						Model.UserParameters["������"].StringValue = "���������";
						shtapik = Model.UserParameters["������"].StringValue;
					}
					else if (Model.ProfileSystem.Name != ThermoLock && Model.ProfileSystem.Name != Classic && Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick)
					{
						MessageBox.Show("��� ������ ����������� ��� ������� � ����� �����������! ��������� ������� � ���������� ����� ����������� �� ���������.");
						ChangeProfileSystem();
					}
				}
			}

			// Solar ��������� ��������
			if (Model.ProfileSystem.Name == Solar)
			{
				if (Model.VisibleRegions[0].Fill.Thikness >= 31 + addthick && Model.VisibleRegions[0].Fill.Thikness <= 33 + addthick && Model.ProfileSystem.Name == Solar)
				{
					Model.UserParameters["������"].StringValue = "�����������";
					shtapik = Model.UserParameters["������"].StringValue;
				}
			}
		}

		#endregion

		#region ����� ���������� �������

		private void ChangeProfileSystem()
		{
			ChangeColor();

			//����������� ����������� ������������ ���������� ������� � ��������� (������� 02-02-2011)
			if (Model.ProfileSystem.Name.ToLower().Contains("���") || Model.FurnitureSystem.Name.ToLower().Contains("���"))
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
				Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� ��� ���������� �����������");
			}

			// ������� �����....  ������ �������� !!!
			if (Model.ProfileSystem.Name == "Rehau DD" || Model.ProfileSystem.Name == "Rehau SD" || Model.ProfileSystem.Name == "Rehau BRD" || Model.ProfileSystem.Name == "Rehau 70��")
			{
				Model.UserParameters["������"].StringValue = "�����������";
			}
			else if (Model.ProfileSystem.Name == ThermoLock || Model.ProfileSystem.Name == Classic || Model.ProfileSystem.Name == Solar || Model.ProfileSystem.Name == Vario)
			{
				ChangeUserParamValue(); //�������� 18-09-2011 (������ �������)
			}
			else if (Model.ProfileSystem.Name == SCHTANDART_START || Model.ProfileSystem.Name == SCHTANDART_COMFORT || Model.ProfileSystem.Name == SCHTANDART_PREMIUM)
			{
			}
			else
			{
				Model.UserParameters["������"].StringValue = "���������";
				Model.UserParameters["���� ����������"].StringValue = "������";
			}

			foreach (clsLeaf lf in Model.Leafs)
			{
				lf.UserParameters["��������� ���������� �������"].Visible = (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf);
				if ((Convert.ToInt32(lf.HandleBeem.Profile.A) - 40) >= 54 && Model.ConstructionType.Name.ToLower().Contains("�����") && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf)
				{
					lf.UserParameters["�������� �������"].Visible = true;
				}
				else
				{
					lf.UserParameters["�������� �������"].Visible = false;
				}

				if (Model.ConstructionType.Name == "����")
				{
					lf.UserParameters["�������� �������"].StringValue = "���";
				}
				else if (Model.ConstructionType.Name == "��������� �����")
				{
					//lf.UserParameters["�������� �������"].StringValue = "���";
					if (lf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
					{
						lf.UserParameters["��������� �����"].Visible = false;
						lf.UserParameters["��������� �����"].StringValue = "���";
					}
					else
					{
						lf.UserParameters["��������� �����"].Visible = true;
						//lf.UserParameters["��������� �����"].StringValue = "�����";
					}
				}
				else if (Model.ConstructionType.Name == "����� ������������")
				{
					lf.UserParameters["�������� �������"].StringValue = "�������";
				}
				else if (Model.ConstructionType.Name == "����� �������")
				{
					lf.UserParameters["�������� �������"].StringValue = "�������";
				}

				if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("������"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");
						lf.UserParameters["������� ������"].StringValue = "Roto";
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || !Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� ������� (��������)");
						lf.UserParameters["������� ������"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.����������)
						lf.OpenType = OpenType.����������;

					lf.UserParameters["������� ������"].Visible = true;
					lf.HingeCount = 3;
					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;
				}
				else
				{
					if (Model.FurnitureSystem.Name.ToLower().Contains("�������") && !Model.ConstructionType.Name.ToLower().Contains("������������") && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG ����� ������ ��������� ��� ����� ������� ��� ������� ���� ����������� - ��� �� ���������

					lf.UserParameters["������� ������"].Visible = false;

					if (Model.ConstructionType.Name.ToLower().Contains("������������"))
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

			// ��������� 19.12.2011 - ������ �.� 
			ChangeLeafSize();
			// ��������� 31.05.2013 - ������ �.� 			
			ChangeFill();
		}

		#endregion

		#region ����� ������� ���������

		private void ChangeFurnitureSystem()
		{
			// �� ������ ������ �������� ����� �������� ���������
			Model.UserParameters["������������ ���������"].StringValue = "��������";

			//����������� ����������� ������������ ���������� ������� � ��������� (������� 02-02-2011)
			if (Model.FurnitureSystem.Name.ToLower().Contains("���"))
				return;

			if (Model.ProfileSystem.Name == Rehau70)
			{
				if (Model.FurnitureSystem.Name != FurnSystDver)
				{
					if (Model.ConstructionType.Name.ToLower().Contains("����� �������") || Model.ConstructionType.Name.ToLower().Contains("������������"))
					{
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");
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
			else if (Model.FurnitureSystem.Name.ToLower().Contains("��������� ��� ���������� �����������"))
			{
				Model.ProfileSystem = Model.WinDraw.ProfileSystems.FromName("���������� �����������");
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
                    MessageBox.Show("��������� Siegenia Titan ����� ������������ ������ � ���������� ������� 70��", "��������!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(FurnSystRotoNT); 
                }
            }
            */

            //			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial || Model.FurnitureSystem.Name == FurnSystSigeniaTitanWK1) && Model.UserParameters["������������ ���������"].StringValue != "���������������")
            //			{
            //				Model.UserParameters["������������ ���������"].StringValue = "���������������";
            //			}
            //
            //			if (Model.ProfileSystem.Name == ProfSystVario && (Model.FurnitureSystem.Name == FurnSystRotoNTDesigno || Model.FurnitureSystem.Name == FurnSystRotoNT))
            //			{
            //				Model.UserParameters["������������ ���������"].StringValue = "���������������";
            //			}
		}

		#endregion

		#region ��������� ���������������� �������������

		//������� 18-09-2011 (������ �������)
		void ChangeUserParamValue()
		{
			//MessageBox.Show("���� ���������� - " + uplotnenie_color);
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (Model.ConstructionType.Name.Contains("��������� �����") && lf.UserParameters["��������� ���������� �������"].StringValue == "���������� ��������")
				{
					MessageBox.Show("�� ���������� ��������� ����� � ���������� ����������, ��������� ������� ����� �������������!!!", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
				}

				// todo refactor ��� ��������� ��� �������� ������ � ���� ��������
				if (Model.ProfileSystem.Name == SystSlayding60)
				{
					if (lf.UserParameters["��������� �����"].StringValue != "��������")
					{
						lf.UserParameters["��������� �����"].StringValue = "��������";
						// 20180606 // MessageBox.Show("���������� ��� ��������� ����� - ��������!", "��������!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					}
				}
			}

			//������ �.�. 29.10.12			
			if (Model.ProfileSystem.ID == id_action_rehau_70 || Model.ProfileSystem.ID == id_rehau_euro_70)
			{
				//��� ���� ������ ������ ������ ���������, ���������� ������ ������
				UserProprietesAction();
				ChangeFill();
			}

			if (Model.ProfileSystem.Name == Vario)
			{
				Model.UserParameters["������"].StringValue = "�����������";
				Model.UserParameters["���� ����������"].StringValue = "������";
			}

			if (Model.ProfileSystem.Name == Thermo70 || Model.ProfileSystem.Name == Thermo76 || Model.ProfileSystem.Name == NEO_80)
			{
				if (Model.UserParameters["������"].StringValue != "��������" || Model.UserParameters["���� ����������"].StringValue != "�����")
				{
					Model.UserParameters["������"].StringValue = "��������";
					Model.UserParameters["���� ����������"].StringValue = "�����";
				}
			}

			if (Model.ProfileSystem.Name == Estet)
			{
				if (Model.UserParameters["������"].StringValue != "�������� ������" || Model.UserParameters["���� ����������"].StringValue != "�����")
				{
					Model.UserParameters["������"].StringValue = "�������� ������";
					Model.UserParameters["���� ����������"].StringValue = "�����";
				}
			}

			if (Model.ProfileSystem.Name != Solar && Model.ProfileSystem.Name != ThermoLock && Model.ProfileSystem.Name != Classic)
			{
				if (shtapik != Model.UserParameters["������"].StringValue)
				{
					shtapik = Model.UserParameters["������"].StringValue;
					ChangeFill();
				}
				else if (uplotnenie_color != Model.UserParameters["���� ����������"].StringValue)
				{
					uplotnenie_color = Model.UserParameters["���� ����������"].StringValue;
					ChangeFill();
				}
			}
			else
			{
				if (shtapik != Model.UserParameters["������"].StringValue)
				{
					Model.UserParameters["���� ����������"].StringValue = "�����"; //��� Solar � Thermolock ���� ���������� ������ �����
					uplotnenie_color = Model.UserParameters["���� ����������"].StringValue;
					shtapik = Model.UserParameters["������"].StringValue;
					ChangeFill();
				}

				if (uplotnenie_color != "�����")
				{
					Model.UserParameters["���� ����������"].StringValue = "�����"; //��� Solar � Thermolock ���� ���������� ������ �����
					uplotnenie_color = Model.UserParameters["���� ����������"].StringValue;
					ChangeFill();
				}
			}

			//			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanWK1) && Model.UserParameters["������������ ���������"].StringValue != "���������������")
			//			{
			//				Model.UserParameters["������������ ���������"].StringValue = "���������������";
			//			}
			//
			//			if ((Model.FurnitureSystem.Name == FurnSystSigeniaTitanSpecial) && Model.UserParameters["������������ ���������"].StringValue == "���������������")
			//			{
			//				Model.UserParameters["������������ ���������"].StringValue = "��������";
			//			}
			//			
			//			if ((Model.FurnitureSystem.Name == FurnSystRotoNT || Model.FurnitureSystem.Name == FurnSystRotoNTDesigno) && Model.ProfileSystem.Name == ProfSystVario && Model.UserParameters["������������ ���������"].StringValue != "���������������")
			//			{
			//				Model.UserParameters["������������ ���������"].StringValue = "���������������";
			//			}
			//
			//			if (Model.FurnitureSystem.Name == ProfSystSlayding60)
			//			{
			//
			//			}
			//
			//			if (Model.ProfileSystem.Name == ProfSystVario && Model.FurnitureSystem.Name == FurnSystRotoNTDesigno && Model.UserParameters["������������ ���������"].StringValue != "���������������")
			//			{
			//				Model.UserParameters["������������ ���������"].StringValue = "���������������";
			//			}
		}

		#endregion

		#region ���������������� �������������� ��� ������� �����

		void UserProprietesAction()
		{
			Model.UserParameters["������"].StringValue = "���������";
			Model.UserParameters["���� ����������"].StringValue = "������";
		}

		#endregion

		#region ��������� ���������������� �������������

		//������� 08-12-2010
		void UserProperties()
		{
			// legacy ����� ����� ���-�� ��� ���������� ����������� �����, � ����� = ����
			if (Model.ConstructionType.Name.Contains("�������"))
			{
				foreach (clsRegion r in Model.VisibleRegions)
				{
					if (r.Fill.Marking.ToString().ToLower().Contains("���"))
						r.Fill = Model.ProfileSystem.Fills["���_������_�_�������"];
					else
					{
						if (Model.ProfileSystem.ToString().ToLower().Contains("���������� �����������"))
							r.Fill = Model.ProfileSystem.Fills["M1 10"];
					}
				}
			}

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.UserParameters["��������� �����"] != null)
					lf.UserParameters["��������� �����"].Visible = !Model.ConstructionType.Name.Contains("�������");
				if (lf.UserParameters["������� ������"] != null)
					lf.UserParameters["������� ������"].Visible = !Model.ConstructionType.Name.Contains("�������") && !Model.ConstructionType.Name.ToLower().Contains("������������");
				if (lf.UserParameters["��������"] != null)
					lf.UserParameters["��������"].Visible = Model.ConstructionType.Name.Contains("�����") && lf.ShtulpOpenType != ShtulpOpenType.ShtulpOnLeaf && !Model.ConstructionType.Name.Contains("�������");

				//����������� �����
				if (Model.ConstructionType.Name.Contains("�������"))
				{
					lf.HingeCount = 0;
					lf.OpenType = OpenType.�����������;
				}
			}
		}

		#endregion

		#region ��������� ������� ����������

		// ������� 21-12-2010 // 27-05-2019 http://yt:8000/issue/dev-253
		void ProfileConnection()
		{
			//����
			foreach (clsFrame fr in Model.Frame)
			{
				if (Model.ProfileSystem.Name.ToLower().Contains("���������� �����������") && Model.ConstructionType.Name.Contains("�������"))
				{
					switch (fr.PositionBeem.ToString())
					{
						case "Bottom":
							fr.ConnectType = ConnectType.�������;
							break;

						case "Left":
							fr.ConnectType = ConnectType.��������;
							break;

						case "Top":
							fr.ConnectType = ConnectType.�������;
							break;

						case "Right":
							fr.ConnectType = ConnectType.��������;
							break;
					}
				}
				else if (Model.ProfileSystem.Name.ToLower().Contains("��������-60") || Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
				{
					if (!Model.FurnitureSystem.Name.ToLower().Contains("��������-60"))
					{
						fr.Profile = Model.ProfileSystem.colProfileFrame["ROS0681.07"]; // ROS0681.07
						fr.ConnectType = ConnectType.������;
					}
					else
					{
						switch (fr.PositionBeem.ToString())
						{
							case "Bottom":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2277.07"]; // ROS0676.07
								fr.ConnectType = ConnectType.��������;
								break;

							case "Left":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2278.07"]; // ROS0677.07
								fr.ConnectType = ConnectType.�������;
								break;

							case "Top":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2276.07"]; // ROS0675.07
								fr.ConnectType = ConnectType.��������;
								break;

							case "Right":
								fr.Profile = Model.ProfileSystem.colProfileFrame["ROS2278.07"]; // ROS0677.07
								fr.ConnectType = ConnectType.�������;
								break;
						}
					}
				}
				else
				{
					if (fr.bType == ComponentType.Porog)
						fr.ConnectType = ConnectType.��������;
					else if (fr.Beem1.bType != ComponentType.Porog)
						fr.Connect1 = ConnectType.������;
					else if (fr.Beem2.bType != ComponentType.Porog)
						fr.Connect2 = ConnectType.������;
				}
			}
		}

		#endregion

		#region ��������� ������� ���� / ������� / �������

		public void ChangeRadius()
		{
			foreach (clsRegion r in Model.VisibleRegions)
			{
				#region ��� ����������

				if (r.Fill.Marking.Contains("3+3"))
				{
					//����
					foreach (clsFrame fr in Model.Frame)
					{
						if (r.IsConnectBeem(fr) && (fr.R1 != 0 || fr.R2 != 0))
						{
							MessageBox.Show("���������� ������� ����������� � �������������� ���������� 3+3 ���������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							fr.R1 = 0;
							fr.R2 = 0;
						}
					}

					// ������
					foreach (clsImpost imp in Model.Imposts)
					{
						if (r.IsConnectBeem(imp) && (imp.R1 != 0 || imp.R2 != 0))
						{
							MessageBox.Show("���������� ������� ����������� � �������������� ���������� 3+3 ���������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							imp.R1 = 0;
							imp.R2 = 0;
						}
					}
				}

				#endregion
			}

			if (Model.ProfileSystem.Name == Pimapen || Model.ProfileSystem.Name == RehauEuro70)
			{
				//MessageBox.Show("��������� ������� ����������");
				//����
				foreach (clsFrame fr in Model.Frame)
				{
					if (fr.R1 != 0 || fr.R2 != 0)
					{
						MessageBox.Show("������ �������� � ������ ���������� ������� ���������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						fr.R1 = 0;
						fr.R2 = 0;
					}
				}

				// ������
				foreach (clsImpost imp in Model.Imposts)
				{
					if (imp.R1 != 0 || imp.R2 != 0)
					{
						MessageBox.Show("������ �������� � ������ ���������� ������� ���������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						imp.R1 = 0;
						imp.R2 = 0;
					}
				}
			}
		}

		#endregion

		#region ��������� ����� �����������

		public void ChangeColor()
		{
			foreach (clsLeaf lf in Model.Leafs)
			{
				if (Model.ConstructionType.Name == "����� �������")
				{
					if (!Model.ColorInside.ToString().ToLower().Contains("�����") || !Model.ColorOutside.ToString().ToLower().Contains("�����"))
						lf.UserParameters["������� ������"].StringValue2 = "����������";
					else
						lf.UserParameters["������� ������"].StringValue2 = "�����";
				}
			}
		}

		#endregion

		#region ��������� ������� �������

		public void ChangeLeafSize()
		{
			ChangeFill();
			recalcHingeCount();
		}

		#endregion

		#region ��������� ������

		public void ChangeHinge()
		{
			// ������ �������� ������� ������� ����������� ��� �������� �������
			AddLeaf();

			foreach (clsLeaf lf in Model.Leafs)
			{
				if (lf.HingeCount > 4)
				{
					MessageBox.Show("������������ ���������� ������ - 4", "��������", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
					lf.HingeCount = 4;
				}

				lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;

				if (Model.ConstructionType.Name.Contains("�����") && !Model.ConstructionType.Name.ToLower().Contains("������������"))
				{
					if (!Model.ProfileSystem.Name.ToLower().Contains("������"))
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� �������");
					}
					else
					{
						if ((!Model.FurnitureSystem.Name.ToLower().Contains("�������") || !Model.FurnitureSystem.Name.ToLower().Contains("��������")) && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
							Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName("��������� ������� (��������)");
						lf.UserParameters["������� ������"].StringValue = "GIESSE";
					}

					if (lf.OpenType != OpenType.����������)
						lf.OpenType = OpenType.����������;

					lf.UserParameters["������� ������"].Visible = true;

					lf.HingeType = HingeType.DoorHinge;

					if ((lf.HingeCount % 2) == 0 || lf.HingeCount == 2)
						lf.HingePosition = HingePosition.HingeBorderEven;
					else
						lf.HingePosition = isPvh(Model) ? HingePosition.HingeProportional : HingePosition.HingeBorderTop2;

					lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;
				}
				else
				{
					if (Model.FurnitureSystem.Name.ToLower().Contains("�������") && !Model.ConstructionType.Name.ToLower().Contains("������������") && !Model.FurnitureSystem.Name.ToLower().Contains("���"))
						Model.FurnitureSystem = Model.WinDraw.FurnitureSystems.FromName(DefaultWindowFurnitureSystem); // BUG ����� ������ ��������� ��� ����� ������ ��� �� ����������

					lf.UserParameters["������� ������"].Visible = false;

					if (Model.ConstructionType.Name.ToLower().Contains("������������") || Model.ConstructionType.Name.ToLower().Contains("���������"))
					{
						lf.HingePosition = HingePosition.HingeProportional;
						if (lf.OpenType != OpenType.���������� && lf.HingeCount > 2)
						{
							MessageBox.Show("����� 2� ����� �������� ������ � ���������� ���� ����������", "��������", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
							lf.HingeCount = 2;
							lf.HingePosition = HingePosition.HingeProportional;
						}
					}

					if (lf.HingeType != HingeType.WindowHinge)
					{
						//MessageBox.Show("���������� ���������� ������� �����!", "��������", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
						lf.HingeType = HingeType.WindowHinge;
					}
				}

				// �� �������� ������ ���������������� �����
				if ((lf.OpenType == OpenType.�������� || lf.OpenType == OpenType.���������) && lf.HingePosition != HingePosition.HingeProportional)
					lf.HingePosition = HingePosition.HingeProportional;
			}

			recalcHingeCount();
		}

		#endregion

		#region ��������� ���� ����������

		public void ChangeConnectionType()
		{
			foreach (clsFrame fr in Model.Frame)
			{
				// ����� ������� ����������, ������ ���� ������ ������� ������� ������������ � ���������� ������� � ������ ����, ����� ��� ��� �� ������������ � ������ ������� ������ ������� � ������ ���������� �����
				if (fr.bType == ComponentType.Porog && (Model.ProfileSystem.Name == Pimapen || Model.ProfileSystem.Name == FORWARD || Model.ProfileSystem.Name == BAUTEK || Model.ProfileSystem.Name == BAUTEKmass))
					fr.ConnectType = ConnectType.�������;

				if (fr.bType == ComponentType.Porog && (Model.ProfileSystem.Name.ToLower().Contains("rehau 60") || Model.ProfileSystem.Name.ToLower().Contains("������")))
					fr.ConnectType = ConnectType.��������;
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

		#region ��������� ���� ��������� �����

		public void ChangeHandlePosition()
		{
			//  ������ ������������ �����
			foreach (clsLeaf leaf in Model.Leafs)
			{
				if (leaf.HandlePositionType == HandlePositionType.������������)
				{
					leaf.HandlePositionType = HandlePositionType.�����������;
				}
			}

			// ���������� ����������� ������� ��� ��������� SigeniaTitan
			if (Model.FurnitureSystem.Name == SiegeniaTitanWK1 || Model.FurnitureSystem.Name == SiegeniaTitan || Model.FurnitureSystem.Name == TitanDebug)
			{
				foreach (clsLeaf leaf in Model.Leafs)
				{
					if (leaf.HandlePositionType == HandlePositionType.�������������)
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
				// ROTO NT ����������� ������
				// ��������� ����� ��� ������� �� ������ ����� ����� � �������� ������� ����� ���� ������� �� ��������, ������� ���� ������������� ��� ���� ��� �� ����
			else if (Model.FurnitureSystem.Name == RotoNT || Model.FurnitureSystem.Name == RotoNTDesigno)
			{
				foreach (clsLeaf leaf in Model.Leafs)
				{
					if (leaf.HandlePositionType == HandlePositionType.�������������)
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
						if (leaf.HandlePositionType != HandlePositionType.�����������)
							leaf.HandlePositionType = HandlePositionType.�����������;
					break;
			}
		}

		#endregion

		#region ��������� �����������

		public void ChangeMarkingSteel()
		{
			string furn_komplekt = Model.UserParameters["������������ ���������"].StringValue;
			//��������� ���� � � ���������� �� ����� ����� ���������� �����������
			foreach (clsFrame fr in Model.Frame)
			{
				if (fr.Lenght > 2000 || furn_komplekt == "���������������")
				{
					if (fr.MarkingSteel.ToString() == "245536")
						fr.MarkingSteel = "239583";
						//�����
					else if (fr.MarkingSteel.ToString() == "245536_")
						fr.MarkingSteel = "239583_";
				}
				else if (fr.MarkingSteel.ToString() == "239583")
					fr.MarkingSteel = "245536";
					//�����
				else if (fr.MarkingSteel.ToString() == "239583_")
					fr.MarkingSteel = "245536_";
			}

			//��������� �������
			foreach (clsImpost imp in Model.Imposts)
			{
				if (imp.Lenght > 2000 || furn_komplekt == "���������������")
				{
					if (imp.MarkingSteel.ToString() == "245536")
						imp.MarkingSteel = "239583";
						//�����
					else if (imp.MarkingSteel.ToString() == "245536_")
						imp.MarkingSteel = "239583_";
				}
				else if (imp.MarkingSteel.ToString() == "239583")
					imp.MarkingSteel = "245536";
					//�����
				else if (imp.MarkingSteel.ToString() == "239583_")
					imp.MarkingSteel = "245536_";
			}

			// ���� �������� ����� �� ���� ��� ��������� ����������� � �����
			if (Model.ConstructionType.Name.ToLower().Contains("�����"))
				foreach (clsLeaf lf in Model.Leafs)
				{
					if (lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel.ToString() == "245536")
						lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel = "239583";
						//�����
					else if (lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel.ToString() == "245536_")
						lf.HingBeem.ConnectBeemLine.Beem.MarkingSteel = "239583_";
				}
		}

		#endregion

		#region ����������� � �����������

		public void ConnectorType()
		{
			/// �������� �������
			/// �� ����������� ������
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
					//������� �� ��� �����
					con.Selected = true;
					Model.Connectors.Remove(con);
					Model.ClearSelected();
					DelConnect = true;
					break;
				}
			}

			// � ������ �������� �� ��� ��������� ���� ����� �� ��� �� ��������� :)
			if (DelConnect)
			{
				foreach (clsFrame f in Model.Frame)
					if (f.Guid.ToString() == guid)
					{
						f.SelPointCenter.Selected = true;
						foreach (clsProfile conn in Model.WinDraw.ProfileSystems.FromName(Model.ProfileSystem.Name).colProfileConnectors)
						{
							if (conn.ConnectortypeName != null && conn.ConnectortypeName == "������������ �������")
								Model.Connectors.Add(conn);
						}
					}
			}
		}

		#endregion

		#endregion

		public static void BeforeModelCalc(clsModel model)
		{
			// ����� ��������������
			if (model.ProfileSystem.Name == Vario && model.FurnitureSystem.Name == RotoNTDesigno)
				model.UserParameters["������������ ���������"].StringValue = "���������������";

			foreach (clsLeaf lf in model.Leafs)
			{
				lf.UserParameters["������� ������"].NumericValue = lf.HingeCount;
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

			// ������� ��������� ��������-60
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
							throw new ArgumentException(string.Format("{0} �� ������ ������", beem.Profile.Marking));
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
								throw new ArgumentException(string.Format("{0} �� ������ ������", beem.Profile.Marking));
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

		/// ���������� � ��� ����� � �� ������ ��� ����� ���������������� �������������, �����, etc...
		public void AfterModelCalc(clsModel model)
		{
			/// �������� ������� �� ������� � �������
			/// ��������������� ����������� �� ���� ������, ������� �/� ��� � ����� ��������� �� �����
			foreach (Moskit moskit in model.Construction.MoskitList)
			{
				if (moskit.Stvorka == null)
					continue;

				UserParam up = moskit.Stvorka.GetUserParam("��������� �����");
				if (up != null && (up.StrValue == "�������" || up.StrValue == "�������"))
				{
					/// ~todo XXX ���� ������ ������������ �� �������� �������
					/// ������� +50 �� �������� � ������ ������� +40 �� �������� � �����
					/// ��� ������� �� ������������� �������
					/// ������� ������� ����� ������� � �������� 
					/// �� ���� ��� ������� ��������� �����
					int gap = up.StrValue == "�������" ? 50 : 40;
					moskit.Height += gap;
					moskit.Width += gap;
					moskit.Perimetr = 2 * (moskit.Height + moskit.Width);
					moskit.Sqr = moskit.Height * moskit.Width;
					// moskit.SqrSetka = ������� �����, ����� ������������� ��� ����
				}
			}


			/// ����� ���� �� �� ����
			/// ��� ����� ������� �� �� ����������� ���������� � ������� � ���������� ������� !!!! �����
			/// ������� �������� �� ����� ������� 
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

			// ���������� ������ ���� AT
			// fix ������������� ����������� ����� ����������, ��� ����������� ����� ����������� ������ ���������� ��� ������ �� ����� ���������� 0,001 ������� ��� ��������� ������ 
			fixGlassFormType(model);

			// ���������� clsModel.Nalichnik -> Construction.Nalichnik
			NalichnikRestriction.calc(model);

			// ��������� ������� ���� �������� ����� ��������� � ��������� , ������� ����� ���-�� �������� �� ��� ����� ����� ��������������� ������ ��������� ����������
			markConjuctionRamaItem(model);

			// 04-07-2018 ������ ������������� ������� ������ �� 100 �� ������ ��� ������� � �������� ������, ������������ "������" ��������� ����������, ����� �� ���� ����� ���������� � ���� ����������� ����������� �������
			fixPodstavochikThick(model);

			// ����� ���� ������� � ����������� (class)
			// ������ ����������, ����� � ��� ���� �� ��� �������� ������ � ��� � ������������, ����!! �� ������ ���� ��� , ���, ���������� ����� .DateModyfied >= .creat
			// Model.Construction.DateCreate = Model.CreateTime;
			model.Construction.DateModyfied = DateTime.Now;

			// ��� �������� �� ����� ������ (����) ������ �� ��� ������������ ������ ������� ��� ��� � �� � ����� ��������������� �������� 
			{
				// �� ��������� ������ ������ ������� ������ � ������� �� 920 �� 1120 // 900 - 1170
				if (model.ConstructionType.Name == _wcdoor)
				{
					// ��� ������� ��������� ������� � clsModel
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
										addErrorUniq(model, string.Format("� ����������� {0} ������ ������� �� ������ �������������� � ����� ����� �� ������ �� {1} �� {2} ��, (� ������ ������� ������� {4}��). ������� ��������� {3}��.",
											model.ConstructionType.Name, x0, x1, x, a * 2));
							}
						}
					}
				}

				// ������ �� ������� ������ ��������� ����� :( �� ��������� �� ����� ���� ���������� ������ ����� ������� ������
				foreach (Impost impost in model.Construction.ImpostList)
				{
					const int delta = 12;
					if (impost.BalkaEnd1 != impost.BalkaEnd2 && Math.Abs(impost.CutAngleEnd1 - impost.CutAngleEnd2) > delta || impost.BalkaStart1 != impost.BalkaStart2 && Math.Abs(impost.CutAngleStart1 - impost.CutAngleStart2) > delta)
					{
						if (restriction)
							addErrorUniq(model, string.Format("������ {0} - ��� ����� ���� � ������ �� ������, ���������� ����� � ��������� � ����� ������ ����", impost.ID));
					}
				}
			}

			/// ��������� �������� ���������������� �������� ���������������� ��� ���� ��� ���������������� ����������� ������
			{
				const string ProfileDependent = "����������������";
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
			// ���������� �������� ����������� ������, M$ �����
			message = message.Replace("'", string.Empty);


			// if (!SystemScript.RunCalc.errors.Contains(message))
			//     SystemScript.RunCalc.errors.Add(message);
			// CalcProcessor.Modules["addError"](new object[] { model.dr_model, message });

			// ����������� ������ ��������� ������ ������
			OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];

			ds_order.orderitemRow orderitemRow = model.dr_model as ds_order.orderitemRow;

			if (orderitemRow != null)
			{
				// ������������ ������ ������ ��� ���� �������
				if (order.ds.ordererror.Select(string.Format("idorderitem = {0} and addinfo = '{1}'", orderitemRow.idorderitem, message)).Length == 0)
				{
					ds_order.ordererrorRow ordererrorRow = order.ds.ordererror.CreateRow(order.idorder, orderitemRow.idorderitem);
					ordererrorRow.addinfo = message;
				}
			}
			else
			{
				// ������������ ������ ������ ��� ������
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
				// 1	������������ �������
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
				// �� ���� �������� ����� ����� ri.W
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

		// ������ � ri.W � ��� ����� ������ ����� ����� ����������� � ����������� �� �������� ������, ��� �� ����� ��������� ��� �������� � ����������, �� ����� �������� ����������� �����������S
		private void markConcreteBeam(clsFrame beem, double length)
		{
			Balka balka = beem.Model.Construction.GetBalkaByID(beem.VisibleID, ModelPart.RamaItem);
			if (balka != null)
				balka.W = Math.Round(length);
		}

		// TODO
		private double getCommonLength(clsFrame beem, clsConnector connector)
		{
			// ���������� ����� ����� �������� ���� � �����������
			double c = connector.Lenght;
			double r = connector.GetFrameRoot().Lenght;
			if (connector.Position < 0)
				c += connector.Position;
			else if (connector.Position > 0)
				r -= connector.Position;

			double common = Math.Min(c, r);

			// ���������� ����� ����� ����� �� ����������� ���� � ��������� ���� 
			double p = beem.Lenght;
			double mp = beem.Model.ConstructionOffset;

			if (mp < 0)
				p += mp;
			else if (mp > 0)
				common -= mp;

			double result = Math.Min(p, common);

			return result;
		}

		// � ���� ����� - ������ ������ - ��� // ������� �� ����������� ������ ������ ��������
		public static void fixGlassFormType(clsModel model)
		{
			if (model.Construction != null)
			{
				foreach (Atechnology.winDraw.Model.Glass glass in model.Construction.GlassList)
				{
					// ������ ������ ���������� -> ��������, ���� ���������� ����������� ��������
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

		#region ���������

		private static void Draw(clsModel model)
		{
			try
			{
				// ��������� ��������� ������ � �����������
				//if (model.WinDraw.frmBuild == null)
				//	return;

				// ���������� �������� ���� ����
				NalichnikRestriction.draw(model);

				// ������� ������� / ����������� / �������
				if (Settings.idpeople == 255)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						float x = (float) leaf.BaseRegion.MinX(0);
						float y = (float) leaf.BaseRegion.MinY(0);

						clsUserParam up1 = leaf.UserParameters["������� ������������"];
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

				// ���� �/�
				try
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.���)
						{
							clsUserParam up = leaf.UserParameters.GetParam("��������� �����");
							if (up != null && up.StringValue2 != up.DefaultValue.StrValue2)
							{
								double x = (leaf.MinX + leaf.MaxX) / 2;
								double y = (leaf.MinY + leaf.MaxY) / 2;
								model.Canvas.DrawString("�/�: " + up.StringValue2, myFont, myBrush, new RectangleF((float) x - 60, (float) y - 110, 200, 100), null);
							}
						}
					}
				}
				catch
				{
					// ignore
				}

				// ���� ����� �������                 // ���� ������
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
					/// ��� ��� ������� �������� �� ���� ������ ��� ������� ����� ������������ ������� ������� ������������ ������, 
					/// � ��� ����� ������� - ������ ��������
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

					clsUserParam upNP = model.UserParameters.GetParam("�������� �� �����");
					if (model.ConstructionType.Name != _outdoor && upNP != null /*&& (model.FurnitureSystem.Name.ToLower().Contains("roto") || model.FurnitureSystem.Name.ToLower().Contains("siegenia"))*/)
					{
						switch (upNP.StringValue)
						{
							case "����������":
								leaf.HingeColor = Color.SaddleBrown;
								break;
							case "�������":
								leaf.HingeColor = Color.LightGray;
								break;
							case "������ �������":
							case "������ ���������":
								leaf.HingeColor = Color.Yellow;
								break;
							case "������ �������":
								leaf.HingeColor = Color.Orange;
								break;
							case "��� ��������":
								leaf.HingeColor = Color.DimGray;
								break;
							case "�����":
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

		// ������ ���������� ������ �� ������ �������
		private void recalcHingeCount()
		{
			foreach (clsLeaf leaf in Model.Leafs)
			{
				// ��� ���������� ������ �������� � ��������� �������
				if (leaf.OpenType == OpenType.��������� || leaf.OpenType == OpenType.��������)
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

		/// ��������� ������ ������ �����������, � ����� �������� �������� ����� �����������!, 
		/// ��� �� ����������� ��� ������� ������� ��� �����������
		private static void validationProcessor(clsModel model)
		{
			// ������ ����� ������ ���������
			List<WinDrawMessage> tempList = new List<WinDrawMessage>(model.WinDraw.Messages);

			// ������� ������ �����
			List<WinDrawMessage> shadow = (List<WinDrawMessage>) model.WinDraw.Messages;
			shadow.Clear();

			// ��������� ��� ���� FixedInvariantWinDrawMessage, 
			// ��� �� �� ����� �� ��������� ������, � ���� ��������� �� ����������������
			foreach (WinDrawMessage message in tempList)
				if (message is FixedInvariantWinDrawMessage)
					shadow.Add(message);

			bool isChange = false;
			// ����������
			foreach (Invalidance invalidance in Validator.validateAndFix(model))
			{
				Invariant invariant = invalidance as Invariant;

				// ���� ����� ��������� �� �� ��������� ���������
				if (invariant is SilentInvariant)
					continue;

				if (invariant != null && invariant.@fixed)
				{
					if (!string.IsNullOrEmpty(invariant.message()))
					{
						FixedInvariantWinDrawMessage message = new FixedInvariantWinDrawMessage();
						message.ID = invariant.uniq();
						message.Message = string.Format("          {0} // ���������� �������������", invariant.message()); // todo �� ��-�� ��-�� ��� ������ ����� ������ ��������
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

			// ������ �������
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

		/// ������������ ���������
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
			/// ���������� ������������ ����������� � ������
			public static IEnumerable<Invalidance> validateAndFix(clsModel model)
			{
				// ���������� ����������� ������� �� ����� ��������� ���������
				List<Invalidance> invariants = new List<Invalidance>();

				// ������ ����������
				List<Invalidance> invalidances = null;

				int i = 0;
				const int max_i = 12;
				// ���� ���������
				bool fix = true;
				while (fix)
				{
					// ����� ������
					fix = false;

					// ���������, ���� �� ����� ����������� �� �� ����� � ���� �������
					invalidances = Validator.validate(model);

					// ������� ��������� ��������� ����������, ��� ����� �������� ������ �� ������������ � �� ����������� ��������� �������������
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
						invalidances.Add(new FixInvalidance(model, "��������� ������ ���������� ������� ��������� ������"));
						break;
					}
				}

				invalidances.AddRange(invariants);

				return invalidances;
			}


			private static List<Invalidance> validate(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>();
				// ��������
				list.Add(GabaritRestriction.test(model));
				// ����� �� ���� ��������
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

				/// ����������� ���� ��, AHTUNG !! AHTUNG !! , 
				/// �������� ������ ���������� model.userParam["������ ���������"] 
				/// � ����� �� ����������� ����������� ROTO OK => 1 // ������ ���������:��������� ROTO NT
				/// � ������ ����� ���������� ������ ����� ��� ������������ ����� ������� ��������� => 0 ������ ���������:String.Empty
				list.AddRange(RotoOkRestriction.test(model));
				list.AddRange(RotoNTRestriction.test(model));
				list.AddRange(GiesseRestriction.test(model));
				list.AddRange(SiegeniaClassicRestriction.test(model));
				list.AddRange(TitanRestriction.test(model));
				list.AddRange(SiegeniaAxxentRestriction.test(model));
				list.AddRange(AxorRestriction.test(model));

				// �������
				foreach (clsLeaf leaf in model.Leafs)
				{
					list.Add(TZInvalidance.test(leaf));
					list.Add(DoorPassiveDependanceInvariant.test(leaf));

					HingeInvariant.test(leaf);
				}

				// ����������
				foreach (clsRegion region in model.VisibleRegions)
				{
					list.Add(Shpros16Restriction.test(region));
					list.AddRange(FillRestriction.test(region));
				}

				list.AddRange(SpreadingRestriction.test(model));

				// ����� �� EVO
				if (model.ProfileSystem.Name == EVO)
				{
					// ����� ��������� ������
					// list.Add(EvoColor.test(model));
					list.Add(GeneoShtapikInvariant.test(model));
					list.Add(EvoRotoNT.test(model));
					list.AddRange(EvoProfileRestriction.test(model));

					// �������
					foreach (clsLeaf leaf in model.Leafs)
					{
						list.Add(EvoImpostInZ64.test(leaf));
						list.AddRange(EvoLeafRestriction.test(leaf));
					}

					// ����������
					foreach (clsRegion region in model.VisibleRegions)
					{
						list.Add(EvoFill44.test(region));
					}

					// ����� �� ���� � ��� ����� �����
				}


				///// ALUTECH
				if (model.ProfileSystem.Name == ALUTECH62 || model.ProfileSystem.Name == ALUTECH_48)
				{
					// ����������, ����� ���� ����� ���� ����� ������
					list.AddRange(AlutechPorogInvariant.test(model));
					list.AddRange(AlutechHeterogenLeafInvariant.test(model));
					list.AddRange(AlutechLeafFrameConjunctionInvariant.test(model));

					// ���������� �����
					foreach (clsLeaf leaf in model.Leafs)
					{
						list.AddRange(AlutechSocleInvariant.test(leaf));
					}

					/// todo �� ��� ������� ��, �������� fix() �������� ������ ��� ��� ��� ��� �� ����� ���������� �� ������������ ��������� ��� ��� ����� , �� ��������
					/// ���� ������� ��������� �� ����������� ������� �� ����� ��������� �� ������������


					// �����������
					list.AddRange(AlutechProfileRestriction.test(model));
				}

				// m/c
				list.Add(MoskitInvalidance.test(model));

				// �����
				list.Add(ColorRestriction.test(model));

				// common PVH Profile restriction
				list.AddRange(RehauProfileRestriction.test(model));

				// Portal
				list.AddRange(PortalRestriction.test(model));

				// ��������
				list.AddRange(SlidingRestriction.test(model));

				// Accesories Color Automat
				list.AddRange(AccessoriesColorAutomat.test(model));

				// ���������
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
			// ������� ��������, ������ �� Stvorka (clsLeaf) �� ����� ������ ������ �������� � clsFrame, ���� ��� ��� ����� clsModel
			protected object subject;

			// ������� �������� � ������ - ������
			protected Invalidance(object subj)
			{
				subject = subj;
			}

			/// todo ������������ ����� ���������� �������� ����������� ��������  Map { Invalidance.typeName ; String Error }
			/// � ����� �� �������� ������, �� ��� ���� ����� ������� �������� � ����� 
			/// � ������ ������� ����� %1 {0} ������������, �����  ����� ���-�� �����
			public abstract string message();

			/// ������� .hash() ����������
			/// ����������� ������������ 
			/// ��������� �������� ��� ���������� ������� �������� ������ ���� �������������� � �����������
			/// �� ������� ���������� �������������� ���������
			public virtual string uniq()
			{
				return GetType().Name;
			}

			// ���� ������������ ������� // todo refactor EVO ��� ��� _����_ �� ����.
			protected static bool isZ64(clsLeaf leaf)
			{
				return leaf.Model.ProfileSystem.Name == EVO && leaf.HingBeem.Profile.A == 64;
			}

			protected static bool getRestriction(clsModel model)
			{
				try
				{
					OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
					DataRow[] drdocsign = order.ds.docsign.Select("sign_name = '�����' and signvalue_str = '������ �����������' and deleted is NULL");
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

		// �� ������ � �� ��������� - ������ �������������� ������������
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

		// ��������� � ���������
		private abstract class Invariant : Invalidance
		{
			/// todo ��� ������������� � �������������� ����� �� ��������� ���������, ��������� �����
			public bool @fixed = true;

			protected Invariant(object subj) : base(subj)
			{
			}

			public abstract void fix();
		}

		// ���������� ��������
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

		// ������ �������� � ������� EVO.Z64 
		private class EvoImpostInZ64 : Invalidance
		{
			protected EvoImpostInZ64(clsLeaf leaf)
				: base(leaf)
			{
			}

			public override string message()
			{
				return "������ � ������� L";
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

		// ������ ����� ����������
		private class GeneoShtapikInvariant : Invariant
		{
			protected const string value = "�����";

			// ������� ����� ��������� ��� ������
			public GeneoShtapikInvariant(object subj)
				: base(subj)
			{
			}

			public override string message()
			{
				return "EVO: ������ ����� ����������";
			}

			public static Invalidance test(clsModel model)
			{
				if (model.ProfileSystem.Name == EVO && model.UserParameters["���� ����������"].StringValue != value)
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
					model.UserParameters["���� ����������"].StringValue = value;
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
				return "������� EVO ���������� ������ 44 ��";
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
					region.Fill = region.Model.ProfileSystem.Fills["4x16x4x16x�4"];
				}
			}
		}

		private class EvoRotoNT : Invalidance
		{
			private static readonly List<string> furnSystems = new List<string>(new string[] {SiegeniaTitan, SiegeniaAxxent});

			public static Invalidance test(clsModel model)
			{
				if (!furnSystems.Contains(model.FurnitureSystem.Name) && model.FurnitureSystem.Name != FurnSystBEZ)
					return new SimpleInvariant(model, string.Format("{0} ������ ��������� {1}", model.ProfileSystem.Name, string.Join(",", furnSystems.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaTitan); });

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

				// ����
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0)
						list.Add(new EvoProfileRestriction(beem, string.Format("{0} �� ������, ���� �����������", beem.Name)));

					if (beem.PositionBeem == ItemSide.Other)
						if (getRestriction(model))
							list.Add(new EvoProfileRestriction(beem, string.Format("{0} ����� ���� �����������", beem.Name)));
				}

				// �������
				foreach (clsLeaf leaf in model.Leafs)
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0)
							list.Add(new EvoProfileRestriction(beem, string.Format("����� ������� {1} {0} �� ������, ���� �����������", beem.Name, leaf.Name)));

						if (beem.PositionBeem == ItemSide.Other)
							list.Add(new EvoProfileRestriction(beem, string.Format("����� ������� {1} {0} ����� ���� �����������", beem.Name, leaf.Name)));
					}

				// �������
				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0)
						list.Add(new EvoProfileRestriction(beem, string.Format("����� ������� {0} �� ������, ���� �����������", beem.Name)));

					if (beem.PositionBeem == ItemSide.Other)
						list.Add(new EvoProfileRestriction(beem, string.Format("����� ������� {0} ����� ���� �����������", beem.Name)));
				}

				// ������  ��������� ������
				const string ��������� = "���������";
				clsUserParam up = model.UserParameters.GetParam("������");
				if (up != null && up.StringValue != ���������)
				{
					list.Add(new SimpleInvariant(model, string.Format("������ {0} ������", ���������), delegate { up.StringValue = ���������; }));
				}

				// �� ������� ����� �� ������� Z64
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.SpreadingV2 != null && region.SpreadingV2.Type == SpreadingType.Falsh && region._Leaf != null && isZ64(region._Leaf))
					{
						list.Add(new EvoProfileRestriction(region._Leaf, string.Format("����� �� �������� �� ������� {0}", region._Leaf[0].Profile.Marking)));
					}
				}

				return list;
			}
		}

		// todo ����� �������� ��� ������ ������ � ����� �������
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
				// ���� ������ ������ ������������� �����������
				foreach (clsBeem beem in leaf)
				{
					if (beem.R1 > 0 || beem.PositionBeem == ItemSide.Other)
						list.Add(new EvoLeafRestriction(leaf, "��������� ������ ������������� �������"));
				}

				/// todo ��� ������ ������ ������������ ��������
				/// ��� � � ����������� �� ����������� 
				/// � ��� ��� ���� ����� ���� �� ������� ������ ������, ���� ����� � �������

				/// ��� ����� ��� ���������� ����� � ������ �� ���, ����� ������ �����
				/// int wf = (int)(leaf.Width - 2 * leaf.HingBeem.Profile.RefE); // ������ - 2 ������
				/// int hf = (int)(leaf.Height - 2 * leaf.HingBeem.Profile.RefE); // ������ - 2 ������

				int minh;
				int maxh;
				int minw;
				int maxw;

				switch (leaf.OpenType)
				{
					case OpenType.��������:
					case OpenType.���������:
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
					case OpenType.����������:
					case OpenType.���������_��������:
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

				// ������
				if ((leaf.Width < minw || leaf.Width > maxw))
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("������ ������� {0:#} �� ��������� ����������� �� {1} �� {2}", leaf.Width, minw, maxw)));

				// ������
				if ((leaf.Height < minh || leaf.Height > maxh))
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("������ ������� {0:#} �� ��������� ����������� �� {1} �� {2}", leaf.Height, minh, maxh)));

				/// ������ ���������� �����, 
				/// ������� �� ������������ ��������� ���� DM8, 
				/// ����� ����� ���� ������� � ������������ �� ����, 
				/// �� ����� ��� �.�. ���� DM8 ������������ ������ � EVO
				const int minHandleF = 800;
				const int maxHandleF = 2000;
				int lenHandleFalce = (int) leaf.HandleBeem.LineE.Length;
				if (isZ64(leaf) && (lenHandleFalce < minHandleF || lenHandleFalce > maxHandleF))
					if (getRestriction(leaf.Model)) /// ������ ���!!!!!
						list.Add(new EvoLeafRestriction(leaf, string.Format("������ ����� c ������ �� ������ {0:#} �� ��������� ����������� �� {1} �� {2}", lenHandleFalce, minHandleF, maxHandleF)));

				/// Evo c L64 �������� - ������ ����������� �����, ��� ��� ����������� ������� DM8 // bug ��� ��� ? ��� �� ������ ������� � ��������� � �� EVO � DM=8
				if (isZ64(leaf) && leaf.HandlePositionType != HandlePositionType.�����������)
					list.Add(new EvoLeafRestriction(leaf, string.Format("��� ������� {0} �������� ������ ����������� ��������� �����", leaf.HandleBeem.Profile.Marking)));

				if (isZ64(leaf) && leaf.ShtulpOpenType != ShtulpOpenType.NoShtulp && leaf.Model.FurnitureSystem.Name != RotoNT && leaf.Model.FurnitureSystem.Name != RotoNTDesigno)
					if (getRestriction(leaf.Model))
						list.Add(new EvoLeafRestriction(leaf, string.Format("������� {0} �� �������� � ���������� ����������, ����������� ������� Z57", leaf.HandleBeem.Profile.Marking)));

				return list;
			}
		}

		// �������� Z | T  <=> ������� ����������
		private class TZInvalidance : Invalidance
		{
			public TZInvalidance(object subj) : base(subj)
			{
			}

			public override string message()
			{
				clsLeaf leaf = (clsLeaf) subject;
				return string.Format("������� {0} ������� {1} {2} �� ������������� ���������� {3}", leaf.Name, leaf.HingBeem.Profile.Marking, leaf.HingBeem.Profile.Comment, leaf.OpenView);
			}

			public static TZInvalidance test(clsLeaf leaf)
			{
				// 2015-04-21 ������� �� ��� ������ ��� ������� // TODO something
				if (!getRestriction(leaf.Model))
					return null;

				switch (leaf.OpenType)
				{
					case OpenType.���������_��������:
					case OpenType.����������:
					case OpenType.��������:
					case OpenType.���������:
						if (leaf.HingBeem.Profile.Comment != null && ((leaf.HingBeem.Profile.Comment.ToUpper().Contains(Z) && leaf.OpenView != OpenView.������) || (leaf.HingBeem.Profile.Comment.ToUpper().Contains(T) && leaf.OpenView != OpenView.������)))
							return new TZInvalidance(leaf);

						break;
				}

				return null;
			}
		}

		// ����������� �� �������� � ����� ��
		private class OknaDaSystemRestriction : Invalidance
		{
			static readonly List<string> forbiden = new List<string>(new string[] {Solar, Vario, Thermo70, EVO, RehauEuro70, ThermoLock, Classic});

			public OknaDaSystemRestriction(object subj) : base(subj)
			{
			}

			public override string message()
			{
				return string.Format("���������� ������� {0} ���������� � ������ ���� ������", ((clsModel) subject).ProfileSystem.Name);
			}

			public static OknaDaSystemRestriction test(clsModel model)
			{
				try
				{
					// ������-�� ��� ��������� � ������ ��� ���� �� ���������� ���� ������ �����
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
		/// ����������� ���������� � ����������� � �����������
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

				#region ��� ���� ����������� ������ ������� ������ ���_������_�_�������

				switch (model.ConstructionType.Name)
				{
					case _facade:
					case _manual:
					case _gate:
					case _wicket:
					case _moskit:
						clsFill fill = model.ProfileSystem.Fills["���_������_�_�������"];
						if (region.Fill != fill)
							list.Add(new SimpleInvariant(model, string.Format("������ ���������� {0}", fill), delegate { region.Fill = fill; }));
						break;
				}

				#endregion

				#region ����������� ���������� �� �������� todo ���-�� ����� �������� � �������

				if (restriction)
				{
					if (model.ProfileSystem.Name == Thermo70 || model.ProfileSystem.Name == Thermo76 || model.ProfileSystem.Name == Estet)
					{
						const int thikness = 32;
						if (region.Fill.Thikness != thikness && region.Fill.Thikness != 0)
							list.Add(new FillRestriction(region, string.Format("� {0} ����� ������������ ������ ���������� {1}��", model.ProfileSystem.Name, thikness)));

						if (region.Fill.FillType == FillType.GlassPack)
						{
							string marking = region.Fill.Marking.ToLower();
							if (!marking.Contains("�") && !marking.Contains("cos") && !marking.Contains("stneo") && !marking.Contains("4(sg rb)") && !marking.Contains("4(sg s)") && !marking.Contains("4�10�4�10�4") && !marking.Contains("mf") && !marking.Contains("4ultra") && !marking.Contains("cgs"))
							{
								list.Add(new FillRestriction(region, string.Format("� {0} ����� ������������ ������ ���������� c ����������������� ������� (�4, MF, ULTRA, etc.)", model.ProfileSystem.Name)));
							}
						}
					}
					else if (model.ProfileSystem.Name == NEO_80)
					{
						const int thikness = 42;
						const string defaultGlass = "4x16x4x14x�4"; // todo seek through glasses by thikness and order by numpos
						if (region.Fill.Thikness != thikness && region.Fill.Thikness != 0)
							list.Add(new SimpleInvariant(model, string.Format("� {0} ����� ������������ ������ ���������� {1}��", model.ProfileSystem.Name, thikness), delegate { region.Fill = model.ProfileSystem.Fills[defaultGlass]; }));

						const int camernost = 2;
						if (region.Fill.FillType == FillType.GlassPack && region.Fill.Camernost < camernost)
							list.Add(new SimpleInvariant(model, string.Format("� {0} ����� ������������ ������ ������������ ����������", model.ProfileSystem.Name, camernost), delegate { region.Fill = model.ProfileSystem.Fills[defaultGlass]; }));
					}
				}

				#endregion

				#region ������ ������ �����������

				// �������� ���� �� ����� 2550 x 1605
				// ��������, ����, ������ ���� �� ����� 2100 x 1600 // @ 27-06-2016 ���������� 
				// *������ *������
				if (region.Fill.Marking.ToLower().Contains("��������") || region.Fill.Marking.ToLower().Contains("������") || region.Fill.Marking.ToLower().Contains("di") || region.Fill.Marking.ToLower().Contains("������"))
				{
					double h, w;
					// ��������� �����
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
							list.Add(new FillRestriction(region, string.Format("���������� {0} ���� ������ {1} x {2}", region.Fill.Marking, maxh, maxw)));
					}
				}

					// ������������ � ���������� ������������ ������ ����� 2250 � 3210
				else if (region.Fill.Marking.ToLower().Contains("+"))
				{
					double h, w;
					// ��������� �����
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
						list.Add(new FillRestriction(region, string.Format("���������� {0} ���� ������ {1} x {2}", region.Fill.Marking, maxh, maxw)));
					}
				}

				if (region.Fill.FillType == FillType.Sandwich)
				{
					// ���
					if (region.RegionHeight > 3000 || region.RegionWidth > 3000 || region.RegionHeight > 1500 && region.RegionWidth > 1500)
					{
						list.Add(new FillRestriction(region, "������� �������-������ ��������� ����������� ���������� 3000x1500"));
					}

					// ���������
					if ((ColorRestriction.isLam(model.ColorInside) && isWidthBig(region, model.ColorInside)) || ColorRestriction.isLam(model.ColorOutside) && isWidthBig(region, model.ColorOutside))
					{
						if (restriction)
							list.Add(new FillRestriction(region, "������ ��������������� �������-������ ��������� ����������� ���������� �������� ��������� �� �����: 1320/660 ��� Hornschuch, 1150/650 ��� Renolit"));
					}
				}

				if (region.Fill.Marking.Contains("3+3(StNeo)") /*|| region.Fill.Marking.ToLower().Contains("����")*/)
				{
					if (restriction)
						list.Add(new FillRestriction(region, string.Format("���������� {0} �� ��������", region.Fill.Marking)));
				}

				if (Settings.isDealer && model.ConstructionType.Name != _moskit && model.ConstructionType.Name != _nalichnik)
				{
					if (region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown)
						list.Add(new FillRestriction(region, string.Format("��������� {0} ����������", region.Fill.Name)));
				}

				// �������� ���� _������_ ���� ��������
				if (region.Fill.FillType == FillType.GlassPack || region.Fill.FillType == FillType.PuzzleFill)
				{
					colLine lines = region.Lines[0];
					for (int i = 0; i < lines.Count; i++)
					{
						// ������ ���������� ����� ��� ���������� �����������, ������� ��������� �� ������
						int angle = (int) Math.Round(lines[i].AngelBeetwenLines360(lines[(i + 1) % lines.Count]));
						// 05-03-2018 ������������� ������ �����
						if (angle > 180 && restriction)
						{
							list.Add(new FillRestriction(region, string.Format("{0} ({1}) - ���������� �������������, ���������� ���������� ����������� ����� �����!", region.Name, region.Fill.FillType.ToString())));
							break;
						}
					}
				}

				if (restriction && (region.Fill.FillType == FillType.GlassPack || region.Fill.FillType == FillType.PuzzleFill) && (region.RegionHeight / region.RegionWidth > 7 || region.RegionWidth / region.RegionHeight > 7))
				{
					list.Add(new FillRestriction(region, String.Format("{0} {1}x{2} ����������� ������ �� ����� 7:1", region.Part, region.RegionWidth, region.RegionHeight)));
				}

				#endregion

				#region ����������� �� ������� � ������� ������������ ����������

				if (restriction && region.Fill.Camernost > 0)
				{
					const string claim = ", ���������� � ���. ����� ��� �������� ������� � ����������� ������������";

					List<int> spanList = new List<int>();

					Qualifier[] qualifiers = Qualifier.of(region.Fill, ref spanList);

					if (spanList.Count > 1 && spanList[0] < spanList[1])
					{
						list.Add(new FillRestriction(region, string.Format("{0} ������ (�� ������� �����) ���-���������� ������ ������ ���� ������, ��� ��������� ������", region.Fill.Marking)));
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
								list.Add(new FillRestriction(region, string.Format("{1} �� ������� �������� ������� ��� ������������� {2}", region.Part, region.Fill.Marking, qualifier)));
							}

							if (m < max || max == 0)
							{
								max = m;
								q = qualifier;
							}
						}

						// ��2 - > m2
						decimal sqr = (decimal) (region.Lines[4].Sqr / 1000000);

						if (sqr > max)
							list.Add(new FillRestriction(region, string.Format("{1}, {0} ������� {2}�2 , (����������� {3}�2 ({5}) ��� ������ ������� ������������ ) {4}", region.Part, region.Fill.Marking, sqr, max, string.Join(", ", qualifierss), q != null ? q.ToString() : "#")));
					}
					else
					{
						list.Add(new FillRestriction(region, string.Format("�� {0} �� ���� ���� �������������� {1}", region.Fill.Name, claim)));
					}
				}

				#endregion

				// ����������� ����������� ��������� ������� ������ �� ����������� ��� ������ �� ������� ����������� �� ���������� ��� ����� ���������� � ���� ������ - ��������...
				if (restriction && isTriplex(region.Fill.Marking) && isArc(region))
					list.Add(new FillRestriction(region, string.Format("{0} ������������� ��������� ({1}) � ������� ���������� ���������", region.Part, region.Fill.Marking)));

				// ������ ����� ������ ������������� // 16-06-2014 �� ������������ TH � TP � �����
				if (restriction && isWarmFrame(region.Fill.Marking) && !region.IsSquare())
					list.Add(new FillRestriction(region, string.Format("{0} ������������� ������ ����� ({1}) � �� ������������� ���������� ���������", region.Part, region.Fill.Marking)));

				// http://yt:8000/issue/dev-246
				// ������ ����������� ������ � �� ����������� � �������� ������ � ������ "Chromatech" ����� ����� 24CHR
				string troubleMakerSpan;
				if (restriction && isChromatech(region.Fill.Marking, out troubleMakerSpan))
					list.Add(new FillRestriction(region, string.Format("{0} ������������� ����� {1} ���������, �������� ������ 24CHR", region.Part, troubleMakerSpan)));

				return list;
			}

			// ������� - ������ ������� �� ������ ���
			private static bool isChromatech(string marking, out string troubleMakerSpan)
			{
				// �������� �� �� ���
				const char ha = '�';
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

			// todo ��������� � ����� ����� ���� ���������
			private static bool isArc(clsRegion region)
			{
				foreach (clsLine line in region.FillConture)
					if (line.R1 > 0 || line.R2 > 0)
						return true;

				return false;
			}

			private static bool isTriplex(String marking)
			{
				// ������� ��������, ��������� - ���������� ������ � ��������� ������ ��������, ����� �������� ����� �����  ������ ������������
				return marking.Contains("+") || marking.Contains(".4") || marking.Contains(".3");
			}

			private static bool isWarmFrame(String marking)
			{
				// ������� ��������, ��������� - ���������� ������ � ��������� ������ ��������, ����� �������� ����� �����  ������ ������������
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
				// 17	����������� �����
				const int idcolorroupStandart = 17;
				return color.IDColorGroup == idcolorroupStandart;
			}

			// ����� ������� ����� 1150 ��, ������� ��  650��
			private static readonly List<string> colors1150 = new List<string>(new string[] {"2178001", "2065021", "2052089", "3202001", "3149008"});

			/// <summary>
			/// ������������ ������ ������ + ����� ��� �� => ���� �������
			/// </summary>
			public class Qualifier
			{
				// ����� �����, � ������ �������� ������� ����� ������� ������ � ���� ����������
				public enum Glass
				{
					_4,
					_3_3,
					_4�����,
					_5,
					_5�����,
					_6,
					_4_4,
					_6�����,
					_8,
					_10
				}

				public readonly Glass glass;
				public readonly int span;

				/// <summary>
				/// ������� (���������) ���������� �� ����� �������������, �� ����� ������ ������ ������� ������� ������������ ������������� ��� 
				/// ������� ��������� ������� �� ����� ������ ������������� ������ �����������, �������� ��������� ������������� �������� ������
				/// ������� ����������� ����� ���� ����������� �� ������� ������������ ������� ���������� ������� ������� ����� ��� � �������� ��� �� ����� �� ���������� �� �������� ���������
				/// </summary>
				/// <param name="fill">���������</param>
				/// <param name="spanList">������ ������� ����� �����, ������������ �������</param>
				public static Qualifier[] of(clsFill fill, ref List<int> spanList)
				{
					string marking = fill.Marking;
					// �������� �� �� ���
					const char ha = '�';
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
							// (�� ����������) 
							// �������� ������ � ������ ������������� ��������� (enum) ����� ��� 3+3 ��� � �� 4 � �� 5 � �� 6 � ������ 3+3.
							// ��� ������������ �������� 4

							// nullity
							if (string.IsNullOrEmpty(mark))
							{
								glasses.Add(Glass._4);
							}
								// ������� �������� ��������� ������� _������_ �������� �� �� �������, 
								// ����� ����� �� ������� ��������� ���������� ������������� ���������� �� ���� ����� + � ����� �����!,
								// ����� ������ �� ������� ��������� ���������� �������������, ������ int.parse ��� ��� int �� � ���� �� ���������� � enum
							else if (digits.Contains("3+3"))
								glasses.Add(Glass._3_3);
							else if (digits.Contains("4+4"))
								glasses.Add(Glass._4_4);
							else if (mark.Contains("�����"))
							{
								switch (digits)
								{
									case "4":
										glasses.Add(Glass._4�����);
										break;
									case "5":
										glasses.Add(Glass._5�����);
										break;
									case "6":
										glasses.Add(Glass._6�����);
										break;
									case "8":
										glasses.Add(Glass._8); // �� ������������� �������� ��� 8 �����
										break;
									case "10":
										glasses.Add(Glass._10);
										break;
									default:
										glasses.Add(Glass._4�����);
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

								// �������� ����� � ���� ��������: 6, 8, 12, ��� �� ����� �������� � �������������� ������������� � �������
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

					// ����������� �������� ��������� ��������
					if (glasses.Count < 2 || glasses.Count > 3 || spans.Count < 1 || spans.Count > 2 || glasses.Count - 1 != spans.Count)
						throw new ArgumentException("����������� ������� ��: " + fill.Marking);

					// �������� ���������, �� ����� �� ������� �� ��� ����������� ��� � ��� �� ����� �������� ��������� - ��� ������ ������� ������
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

				// ������� ������������ ��������, 
				public static readonly Dictionary<Qualifier, decimal> max = new Dictionary<Qualifier, decimal>();

				// ������� �� ������� ��� ����� ����� �� ��� ������ ������� ��� � ����� ����, �� �� ������������ ����� � ��������� �������� � ���� ������� �������� ���� ��� �� �������� - �� �����
				static Qualifier()
				{
					max.Add(new Qualifier(Glass._4, 06), 1.7m);
					max.Add(new Qualifier(Glass._4, 08), 2.5m);
					max.Add(new Qualifier(Glass._4, 12), 3.17m);

					max.Add(new Qualifier(Glass._3_3, 06), 1.7m);
					max.Add(new Qualifier(Glass._3_3, 08), 2.7m);
					max.Add(new Qualifier(Glass._3_3, 12), 3.5m);

					max.Add(new Qualifier(Glass._4�����, 06), 2.1m);
					max.Add(new Qualifier(Glass._4�����, 08), 2.9m);
					max.Add(new Qualifier(Glass._4�����, 12), 3.9m);

					max.Add(new Qualifier(Glass._5, 08), 3.8m);
					max.Add(new Qualifier(Glass._5, 12), 5.0m);

					max.Add(new Qualifier(Glass._5�����, 08), 4.0m);
					max.Add(new Qualifier(Glass._5�����, 12), 5.5m);

					max.Add(new Qualifier(Glass._6, 08), 4.6m);
					max.Add(new Qualifier(Glass._6, 12), 6.2m);

					max.Add(new Qualifier(Glass._4_4, 08), 4.6m);
					max.Add(new Qualifier(Glass._4_4, 12), 6.0m);

					max.Add(new Qualifier(Glass._6�����, 08), 5.2m);
					max.Add(new Qualifier(Glass._6�����, 12), 7.0m);

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
			// ��������� �������
			static readonly List<String> approvedProfSysList = new List<string>(new string[] {Rehau70, Rehau60, DECOR, RehauDeluxe});

			// ������ ��������� / ������ ��� � ������������ � �� ���������
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

					if (up != null && up.StringValue != "���")
					{
						// ������� �������
						if (!approvedProfSysList.Contains(model.ProfileSystem.Name))
							return new WoodLineRestriction(model, string.Format("����� WoodLine �������� ������ � �������� {0}", string.Join(",", approvedProfSysList.ToArray())));

						// ������� ��������� �������
						if (model.ColorOutside.IDColorGroup != approvedColorGroup && model.ColorOutside.IDParentColorGroup != approvedColorGroup)
						{
							return new WoodLineRestriction(model, string.Format("����� WoodLine �������� ������ ��� �������������� ������� �����������"));
						}

						// ������� ���� 90 �� �����
						foreach (clsBeem beem in model.Frame)
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
								return new WoodLineRestriction(model, string.Format("����� WoodLine �������� ������ ������������� �����������"));

						// � ��������, ������� �����
						foreach (clsLeaf leaf in model.Leafs)
							foreach (clsBeem beem in leaf)
								if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0)
									return new WoodLineRestriction(model, string.Format("����� WoodLine �������� ������ ������������� �����������"));

						// ������ �������� ������������ >= 1050 && ������� ������� >= 650
						if (!test1050(model.Frame.Width, model.Frame.Height))
							if (getRestriction(model))
								return new WoodLineRestriction(model, string.Format("���� {0}x{1} �� �������� ����������� WoodLine: �������� ��������� >1050, ������� ������� >650, ������� >400 ", model.Frame.Width, model.Frame.Height));

						foreach (clsLeaf leaf in model.Leafs)
							if (!test1050(leaf.Width, leaf.Height))
								if (getRestriction(model))
									return new WoodLineRestriction(model, string.Format("{2}  {0}x{1} �� �������� ����������� WoodLine: �������� ��������� >1050, ������� ������� >650, ������� >400", leaf.Width, leaf.Height, leaf.Name));
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

		// ����������� Roto OK
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

			private const string _changeParameterName = "������ ���������";

			// 20-02-2016  ++��������
			private static readonly List<int> lyapins = new List<int>(new int[] {291, 304, 319, 325, 360, 370, 371, 372, 498});

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���������� ������������� ������
				bool change = false;

				// �����������
				bool restriction = RunCalc.getRestriction(model);

				// ������� ����� ����� ��� �������� ����� ��� ��� �� ������ ��������
				// if (upChange != null)
				//	upChange.StringValue = string.Empty;

				// ������ ����������� �������
				if (model.FurnitureSystem.Name == RotoOK)
				{
					// ������ ���������� ��������
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new RotoOkRestriction(model, string.Format("{0} �� �������� � ���������� ������� {1}", RotoOK, model.ProfileSystem.Name)));

					if (model.ProfileSystem.Name == RehauEuro70)
					{
						// Euro70 ���������� ����� ������, ���� �� ������, ������� ������������ �� ���������
						OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
						if (order != null && order.DocRow != null && !order.DocRow.IsidsellerNull() && lyapins.Contains(order.DocRow.idseller))
						{
						}
						else
						{
							if (Settings.idpeople != 255)
								list.Add(new RotoOkRestriction(model, string.Format("{0} �� �������� � ���������� ������� {1}", RotoOK, model.ProfileSystem.Name)));
						}
					}

					// ������ �� ��������
					foreach (clsLeaf leaf in model.Leafs)
					{
						// ���� ������� = roto OK, ���� �� ������ ����� ������ ������� �� roto NT, �� � ������ �������� ��� ��� Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								// list.Add(new RotoOkRestriction(leaf, string.Format("{0} ������ ������������� �������", FurnSystRotoOK)));
								// ���� �� ����������� �� ��������� �� ������ ���� ������� ���������, ����� ������ ������ ������ �� ������� Roto NT ( �������
								change = true;
								break;
							}
						}

						// ������ ��������� �� T ������� ���������� ������� DM15
						if (leaf.OpenView == OpenView.������)
							list.Add(new RotoOkRestriction(model, string.Format("{0} �� ��������� �� ������� T", model.FurnitureSystem.Name)));

						/// todo ��� ��� �� ����� �� ����� ���� ����� ������� �� ������� [leafChange] ����� ����� �� ��������� �� ������������ Roto OK
						/// leafChange ������ ! �������� ���� change �� ����� � �����
						/// �� � ����� ������ �� ����� � ���� �� ������������� - ����� �������� � �� ���� ��������� ��� ���� �� Roto OK

						// ������� � ����
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						switch (leaf.OpenType)
						{
							case OpenType.����������:
								switch (leaf.ShtulpOpenType)
								{
									// �� ����������
									case ShtulpOpenType.NoShtulp:
										if (w_F > 800)
										{
											if (w_F < 300 || w_F > 1300 || h_F < 533 || h_F > 2400)
												if (restriction)
													list.Add(new RotoOkRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
														w_F, h_F, 300, 1300, 533, 2400)));
										}
										else
										{
											if (w_F < 300 || w_F > 1300 || h_F < 300 || h_F > 2400)
												if (restriction)
													list.Add(new RotoOkRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
														w_F, h_F, 300, 1300, 300, 2400)));
										}

										break;

									// ���������� ��������
									case ShtulpOpenType.NoShtulpOnLeaf:
										list.Add(new RotoOkRestriction(leaf, "�������� ���������� ������� ������ ���� ���������-��������"));
										break;

									// ���������� ���������
									case ShtulpOpenType.ShtulpOnLeaf:
										if (w_F < 300 || w_F > 800 || h_F < 360 || h_F > 2400)
											list.Add(new RotoOkRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
												w_F, h_F, 300, 800, 360, 2400)));

										break;
								}

								break;

							case OpenType.���������_��������:
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
									list.Add(new RotoOkRestriction(leaf, "��������� ���������� ������� ������ ���� ����������"));

								if (w_F < 410 || w_F > 1300 || h_F < 360 || h_F > 2400)
									list.Add(new RotoOkRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������-�������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
										w_F, h_F, 410, 1300, 360, 2400)));

								break;

							case OpenType.��������:
								if (w_F < 300 || w_F > 1300 || h_F < 300 || h_F > 800)
									if (restriction)
										list.Add(new RotoOkRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� �������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
											w_F, h_F, 410, 1300, 360, 2400)));


								break;
							default:
								list.Add(new RotoOkRestriction(leaf, string.Format("{0} ��� ���������� {1} �� ������������", RotoOK, leaf.OpenType)));
								break;
						}

						/// ��������� ������ �������

						// ����� ����������� ��������� �����, ����������� �� ������ - ����� ������� �������� �������
						if (leaf.HandlePositionType != HandlePositionType.�����������)
							list.Add(new RotoOkRestriction(leaf, "�������� ������ ����������� ��������� �����"));

						// ����� ������� ����� � ������ � � userParam // todo check after DOORS here
						clsUserParam ht = leaf.UserParameters.GetParam("��� �����");
						if (leaf.HandleType != HandleType.������� && leaf.HandleType != HandleType.���_�����)
							list.Add(new RotoOkRestriction(leaf, "�������� ������ ������� �����"));

						if (ht != null && ht.StringValue != "������� �����")
							list.Add(new RotoOkRestriction(leaf, "�������� ������ ������� �����"));

						// ����� 2 ������ ������ �� �������� � ����������
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.��������)
							list.Add(new RotoOkRestriction(leaf, "����� 2 ������ ����� ���������� ������ �� ���������� � �������� �������"));
					}

					// ������� ������ ������

					// --WK1
					clsUserParam wk1 = model.UserParameters.GetParam("������������ ���������");
					if (wk1 != null && wk1.StringValue != "��������")
						list.Add(new RotoOkRestriction(model, string.Format("{0}, �������� ������ �������� ������������ ���������", RotoOK)));

					// ������ ����� ���� ��������
					clsUserParam dekas = model.UserParameters.GetParam("�������� �� �����");
					if (dekas != null && dekas.StringValue != "�����" && dekas.StringValue != "��� ��������")
						list.Add(new RotoOkRestriction(model, string.Format("{0}, �������� ������ ����� �������� �� �����", RotoOK)));
				}

				/// TODO ������ ���������� � BMC- ����������� 
				/// � ����������� �������� �������� �� ������� ������, ������ ��������� � clsModel ������,
				/// ����� �� ����� ��� �������� � ����������� � ������ ���������  � AMC � �� � BMC, �� ����� ������ �� ��� ���� ���� �������, ����� ������ ���� � Construction ������ - 
				/// �� ����, �� ��� ��� �������� ����������� ���� ��������� �� ����� � clsModel, ��� ��������� ����� �� ������� ���
				/// � AMC ������ � clsModel ��� �������� (�� ��������� ��������� ��� ������ � �����������), 
				/// ������ ���� 1) ���� ��� ������ ���� �������� ����� AMC ������� ������ � Construction ���� ���� �� ����, �� ��� ����������� ������ ���� ��������� ������ � AMC
				/// 
				/// ����� �������� ����� ��������� ������� ������ ��� ����� �������� ����. �������
				try
				{
					model.UserParameters[_changeParameterName].StringValue = change ? RotoNT : "���";
					// todo too strange
					if (model.Construction != null)
					{
						UserParam upChangeInConstruction = model.Construction.GetUserParam(_changeParameterName);
						if (upChangeInConstruction != null)
							upChangeInConstruction.StrValue = change ? RotoNT : "���";
					}
				}
				catch (Exception ex)
				{
					list.Add(new RotoOkRestriction(model, string.Format("����������� �������� {0} {1} {2}", _changeParameterName, ex.Message, ex.StackTrace)));
				}

				return list;
			}
		}

		private class OneImpostOneHinge : Invalidance
		{
			// ��� �������� ������ ������� �� ������� ���������� �����������, �� �������� ��������� �� ��� �����, �� ���� ���, ���������� �������� ��� ������ � �������
			private const int minA = 59;

			private OneImpostOneHinge(clsImpost impost) : base(impost)
			{
			}

			public override string message()
			{
				return string.Format("{0} ���� ����� ����� ������� �������������� ��� ����������, ��������� �������� ����� ������� �� ������ ������ ��� �� ������ ������ ������� ��� ����������� ����� ������� ������� �������", ((clsImpost) subject).Name);
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

		// ����������� �� �������� � �/� � ������ > 16�� � ������������
		//    program, 24.11.2017 11:35:58:
		//    ������ ����� ��� KPG = 12 , � ���  SPGF  =16  ??
		//
		//    �������� ��������, 11:44:20:
		//    �� 12
		//
		//    �������� ��������, 11:44:35:
		//    ��� ������
		private class Shpros16Restriction : Invalidance
		{
			const char ha = '�';
			const char x = 'x';
			private readonly string _message;
			private static readonly Regex _regex = new Regex(@"\D+");

			private const string TP = "TP";
			private const string TR = "��";

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
				return string.Format("����������� {0} {1}. {2}", ((clsRegion) subject).Part, ((clsRegion) subject).Fill, _message);
			}

			public static Invalidance test(clsRegion region)
			{
				try
				{
					// ������ �� ��������������
					if (!getRestriction(region.Model))
						return null;

					// ������ �� ������������ ��� �����
					if (region.SpreadingV2 != null && region.SpreadingV2.Type == SpreadingType.Shpross && region.Fill.Camernost > 0)
					{
						string[] parts = region.Fill.Marking.Replace(ha, x).Split(x);

						if (parts.Length >= 3)
						{
							// ��� ���������� ���������� ����� �������
							// List<int> ramkaList = new List<int>();

							bool f = false;
							for (int i = 1; i < parts.Length; i++, i++)
							{
								// ����� ����� �� ����� 16
								string r = _regex.Replace(parts[i], string.Empty);
								if (int.Parse(r) >= min)
									f = true;

								// � ������ ����� ���� �� ������ TP ��� �� ��������
								if (parts[i].Contains(TP) || parts[i].Contains(TR))
									return new Shpros16Restriction(region, parts[i]);
							}

							if (!f)
								return new Shpros16Restriction(region, string.Format("���������� ���������� ����� �� ��������, ����������� �� � ������ �� ����� {0}��", min));
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
				// ������� ������ ������� ��� ������
				if (model.ProfileSystem.Name == Rehau60)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf[0].Profile.Marking == __AntikMarking)
						{
							model.UserParameters["���� ����������"].StringValue = "�����";

							int t = leaf.BaseRegion.Fill.Thikness;
							if (t >= 22 && t <= 25)
								model.UserParameters["������"].StringValue = "�����������";
							if (t >= 31 && t <= 33)
								model.UserParameters["������"].StringValue = "���������";

							return;
						}
					}
				}
				else if (model.ProfileSystem.Name == ThermoLock || model.ProfileSystem.Name == Classic)
				{
					bool antik = isAntik(model);

					/// ���� �� ������� ������� � ������� � ���� �����������, 
					/// � ��� ����� �������, �� �������� ��� ���� � ������� 
					/// ����� �� ������� ����, ������ �� ��� ����
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

					// ��� ���������
					model.UserParameters["���� ����������"].StringValue = "�����";

					// fix
					if (antik)
					{
						/// �� 24 ����������� ��������� ������� 550090-615, �� ����� � �������� 560607-715
						/// �� 32 ����������� ������ ��������� 560580-615
						/// BUG ������������ �� 0 ���������� ! � ��� ����� ���� ����� ������ ���������� ?
						int t = model.VisibleRegions[0].Fill.Thikness;
						if (t >= 22 && t <= 25 && model.UserParameters["������"].StringValue != "�����������" && model.UserParameters["������"].StringValue != "��������")
							model.UserParameters["������"].StringValue = "�����������";
						if (t >= 31 && t <= 33)
							model.UserParameters["������"].StringValue = "���������";
					}
					else
					{
						/// �� 24 ����������� ��������� �������� 560607-715
						/// �� 32 ����������� ������ ��������� 560580-615
						// BUG ������������ �� 0 ���������� ! � ��� ����� ���� ����� ������ ���������� ?
						int t = model.VisibleRegions[0].Thickness;
						if (t >= 22 && t <= 25)
							model.UserParameters["������"].StringValue = "��������";
						if (t >= 31 && t <= 33)
							model.UserParameters["������"].StringValue = "���������";
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
				return up != null ? string.Format("�������� {1} ������������ ������ {2}", DECOR, up.Name, _value) : this.ToString(); // todo more compact
			}

			public override void fix()
			{
				// ������ 
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
				return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���� �������
				if (model.ProfileSystem.Name != DECOR && model.ProfileSystem.Name != RehauDeluxe)
					return list;

				// ��������� === RotoNT | �������
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon)
					{
						if (model.FurnitureSystem.Name != RotoNT && model.FurnitureSystem.Name != SiegeniaClassic && model.FurnitureSystem.Name != SiegeniaTitan && model.FurnitureSystem.Name != SiegeniaTitanWK1 && model.FurnitureSystem.Name != SiegeniaAxxent)
							list.Add(new DecorRestriction(model, string.Format("� ����������� {0} ������������ ������ ��������� {1}", model.ProfileSystem.Name, SiegeniaClassic)));
					}
					else if (model.ConstructionType.Name == _pskportal)
					{
					}
					else
					{
						if (model.FurnitureSystem.Name != FurnSystDver)
							list.Add(new DecorRestriction(model, string.Format("��� ������ ������������ ������ {0}", FurnSystDver)));
					}
				}

				List<int> thicknesses = new List<int>();
				/// + �������� �������
				/// ������� ���������� === 24 | 32
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
					{
						list.Add(new DecorRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� 24�� ��� 32��", model.ProfileSystem.Name)));
					}
					else
					{
						if (!thicknesses.Contains(region.Fill.Thikness))
							thicknesses.Add(region.Fill.Thikness);
					}
				}

				// ������ �� ��� ������ �� ������� ������� ��������, ������� ��� ����� ������ ������� �� ����������� ����������
				if (thicknesses.Count > 0)
				{
					if (thicknesses.Count > 1)
					{
						// todo ��� �� ��������� ��� ������ ������� ����������
						WinDrawMessage message = new WinDrawMessage();
						message.CanClose = true;
						message.ID = typeof(DecorRestriction).Name;
						message.Message = string.Format("            � ������� ������������ ���������� ������ ������, ��������� ����� ����������� �� ���������� {0}", thicknesses[0]);
						message.Model = model;
						model.WinDraw.AddMessage(message);
					}

					clsUserParam upShtap = model.UserParameters.GetParam("������");
					clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");
					// !! �� ������� �����������, ��������� ���
					switch (thicknesses[0])
					{
						case 24:
							if (upShtap != null && upShtap.StringValue != "�����������")
								list.Add(new DecorInvariant(upShtap, "�����������"));

							if (upRubberColor != null && upRubberColor.StringValue != "������")
								list.Add(new DecorInvariant(upRubberColor, "������"));

							break;
						case 32:
							if (upShtap != null && upShtap.StringValue != "���������")
								list.Add(new DecorInvariant(upShtap, "���������"));

							if (upRubberColor != null && upRubberColor.StringValue != "������")
								list.Add(new DecorInvariant(upRubberColor, "������"));

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
				return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���� �������
				if (model.ProfileSystem.Name != Classic)
					return list;

				bool restriction = getRestriction(model);
				// ��������� === RotoNT | �������
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new ClassicRestriction(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				List<int> thicknesses = new List<int>();

				/// + �������� �������
				/// ������� ���������� === 24 | 32
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
					{
						list.Add(new ClassicRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� 24�� ��� 32��", model.ProfileSystem.Name)));
					}
					else
					{
						if (!thicknesses.Contains(region.Fill.Thikness))
							thicknesses.Add(region.Fill.Thikness);
					}
				}

				// ������ �� ��� ������ �� ������� ������� ��������, ������� ��� ����� ������ ������� �� ����������� ����������
				if (thicknesses.Count > 0)
				{
					if (thicknesses.Count > 1)
					{
						// todo ��� �� ��������� ��� ������ ������� ����������
						WinDrawMessage message = new WinDrawMessage();
						message.CanClose = true;
						message.ID = typeof(DecorRestriction).Name;
						message.Message = string.Format("            � ������� ������������ ���������� ������ ������, ��������� ����� ����������� �� ���������� {0}", thicknesses[0]);
						message.Model = model;
						model.WinDraw.AddMessage(message);
					}

					clsUserParam upShtap = model.UserParameters.GetParam("������");
					clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");
					// !! �� ������� �����������, ��������� ���
					switch (thicknesses[0])
					{
						case 24:
							if (upShtap != null && upShtap.StringValue != "�����������")
								list.Add(new DecorInvariant(upShtap, "�����������"));

							if (upRubberColor != null && upRubberColor.StringValue != "�����")
								list.Add(new DecorInvariant(upRubberColor, "�����"));

							break;
						case 32:
							if (upShtap != null && upShtap.StringValue != "���������")
								list.Add(new DecorInvariant(upShtap, "���������"));

							if (upRubberColor != null && upRubberColor.StringValue != "�����")
								list.Add(new DecorInvariant(upRubberColor, "�����"));

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
			private const string ����� = "�����";
			private const string �������� = "��������";
			private const string �������������� = "�������� ������";

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���� �������
				if (model.ProfileSystem.Name != Thermo76 && model.ProfileSystem.Name != Estet)
					return list;

				bool restriction = getRestriction(model);

				// ��������� 
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new Thermo76Restriction(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				// ������� ���������� === 32 ����������� � FillResstriction

				// ���������� === �����, ������ === ��������
				clsUserParam upShtap = model.UserParameters.GetParam("������");
				clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");

				if (upRubberColor != null && upRubberColor.StringValue != �����)
				{
					list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1} ���� ����������", model.ProfileSystem.Name, �����), delegate { upRubberColor.StringValue = �����; }));
				}

				string rightShtapic = model.ProfileSystem.Name == Estet ? �������������� : ��������;
				if (upShtap != null && upShtap.StringValue != rightShtapic)
				{
					list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1} ������", model.ProfileSystem.Name, rightShtapic), delegate { upShtap.StringValue = rightShtapic; }));
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
			private const string ����� = "�����";
			private const string �������� = "��������";

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���� �������
				if (model.ProfileSystem.Name != NEO_80)
					return list;

				bool restriction = getRestriction(model);

				// ��������� 
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
						list.Add(new Neo80Restriction(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
				}

				// ������� ���������� === 32 ����������� � FillResstriction

				// ���������� === �����, ������ === ��������
				clsUserParam upShtap = model.UserParameters.GetParam("������");
				clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");

				if (upRubberColor != null && upRubberColor.StringValue != �����)
				{
					list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1} ���� ����������", model.ProfileSystem.Name, �����), delegate { upRubberColor.StringValue = �����; }));
				}

				if (upShtap != null && upShtap.StringValue != ��������)
				{
					list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1} ������", model.ProfileSystem.Name, ��������), delegate { upShtap.StringValue = ��������; }));
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

				// ������� �� ����������� �� �����������
				if (model.ConstructionType.Name == _swingdoor)
				{
					switch (model.Leafs.Count)
					{
						case 1:
							if (model.Frame.Width < 500 || model.Frame.Width > 970)
							{
								if (restriction)
									list.Add(new AlutechProfileRestriction(model, string.Format("������ �������������� ����������� ����� {0}, ��� ������� �� ���������� ������� �� {1} �� {2}",
										model.Frame.Width, 500, 970)));
							}

							break;

						case 2:
							if (model.Frame.Width < 1100 || model.Frame.Width > 1800)
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("������ ������������� ����������� ����� {0}, ��� ������� �� ���������� ������� �� {1} �� {2}",
									model.Frame.Width, 1100, 1800)));
							}

							break;

						default:
							list.Add(new AlutechProfileRestriction(model, "����������� ����� ����� ����� ������ 1 ��� 2 �������"));
							return list;
					}

					if (model.Frame.Height < 1600 || model.Frame.Height > 2350)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("������ ����������� ����� {0}, ��� ������� �� ���������� ������� �� {1} �� {2}",
							model.Frame.Height, 1600, 2350)));
					}
				}


				/// ~ todo ��� �� ������ ������������� �����������, ������ �������� �������� � ��� �� �����, ������ ������� �� ��������
				/// ����������� �� ����������� ����� // 30-150 ��������

				// ����
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("������� {0} �� �����", beem.Profile.Marking)));
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
								case ConnectType.������:
									a *= 2;
									if (a < 32 || a > 150)
										list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2:F2}�, ��� ������� �� ���������� ������� 32� - 150�", beem.Name, beem.Beem1.Name, a)));
									break;

								case ConnectType.�������:
								case ConnectType.��������:
									if (a < 40 || a > 140)
										list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2:F2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem1.Name, a)));

									break;

								default:
									list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect1)));
									break;
							}

							switch (beem.Connect2)
							{
								case ConnectType.������:
									b *= 2;
									if (b < 32 || b > 150)
										list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2:F2}�, ��� ������� �� ���������� ������� 32� - 150�", beem.Name, beem.Beem2.Name, b)));
									break;

								case ConnectType.�������:
								case ConnectType.��������:
									if (b < 40 || b > 140)
										list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2:F2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem2.Name, b)));

									break;

								default:
									list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect2)));
									break;
							}
						}
						else
						{
							list.Add(new AlutechProfileRestriction(model, string.Format("����� ���� �� ������� {0} ����� �������������� ������ ��� ������ �����, ��� ��������� ����� ����������� ������� ������� �����  xx.xx01, xx.xx02", beem.Profile.Marking)));
						}
					}
				}

				// �������
				foreach (clsLeaf leaf in model.Leafs)
				{
					foreach (clsBeem beem in leaf)
					{
						if (beem.R1 > 0 || beem.R2 > 0)
						{
							list.Add(new AlutechProfileRestriction(model, string.Format("������� {0} �� �����", beem.Profile.Marking)));
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
									case ConnectType.������:
										a *= 2;
										if (a < 30 || a > 150)
											list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 30� - 150�", beem.Name, beem.Beem1.Name, a)));
										break;

									case ConnectType.�������:
									case ConnectType.��������:
										if (a < 40 || a > 140)
											list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem1.Name, a)));

										break;

									default:
										list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect1)));
										break;
								}

								switch (beem.Connect2)
								{
									case ConnectType.������:
										b *= 2;
										if (b < 30 || b > 150)
											list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 30� - 150�", beem.Name, beem.Beem2.Name, b)));
										break;

									case ConnectType.�������:
									case ConnectType.��������:
										if (b < 40 || b > 140)
											list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem2.Name, b)));

										break;

									default:
										list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect2)));
										break;
								}
							}
							else
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("����� ������� �� ������� {0} ����� �������������� ������ ��� ������ �����, ��� ��������� ����� ����������� ������� ������� �����  xx.xx01, xx.xx02", beem.Profile.Marking)));
							}
						}
					}

					/// �������� ����������� DoorRestriction = ��� :( ������� ���� ���
					/// W62 � �48 ����� ������ ����� ������������� KFV0370.12 == ������������� �������� ��������
					/// 1) ���  �������������  - ����������� ������ ������� = 1978��; 
					/// 2) ��� ��������������  - ����������� ������ ������� = 1900�� 
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
										list.Add(new AlutechProfileRestriction(leaf, string.Format("����������� ������ ������� ��� ��������� {0} = {1}",
											up.StringValue, minHeight)));
								}

								break;

							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.Height < minHeight) // 2030
								{
									if (restriction)
										list.Add(new AlutechProfileRestriction(leaf, string.Format("����������� ������ ���������� ������� ��� ��������� {0} = {1}",
											up.StringValue, minHeight))); // 2030
								}

								break;
						}
					}
				}

				// ������
				foreach (clsBeem beem in model.Imposts)
				{
					if (beem.R1 > 0 || beem.R2 > 0)
					{
						list.Add(new AlutechProfileRestriction(model, string.Format("������� {0} �� �����", beem.Profile.Marking)));
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
									list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem1.Name, a)));
							}
							else
							{
								list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect1)));
							}

							if (beem.Connect2 == ConnectType.ImpostShort)
							{
								if (b < 40 || b > 140)
									list.Add(new AlutechProfileRestriction(beem, string.Format("����� ������� {0} � {1} ���� {2}�, ��� ������� �� ���������� ������� 40� - 140�", beem.Name, beem.Beem2.Name, b)));
							}
							else
							{
								list.Add(new AlutechProfileRestriction(beem, string.Format("���������� {0} �� ��������������", beem.Connect2)));
							}
						}
						else
						{
							list.Add(new AlutechProfileRestriction(model, "������ ����� ���������� ������ ��� ������ �����"));
						}
					}
				}


				// ����� ���� �����
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.bType == ComponentType.Porog && (beem.PositionBeem != ItemSide.Bottom))
					{
						list.Add(new AlutechProfileRestriction(model, "����� ������ �����"));
						break;
					}
				}

				// TODO ���� ���-�� ���������� ������� �����������
				// ����������� �� ������ �����
				// if(model.Frame.Height > 3000 || model.Frame.Width > 6000)

				const int max = 4000;

				int min = isImpostLess(model) ? 200 : 250;

				foreach (clsBeem beem in model.Frame)
				{
					int m = beem.PositionBeem == ItemSide.Other || beem.Beem1.PositionBeem == ItemSide.Other || beem.Beem2.PositionBeem == ItemSide.Other ? 250 : min;

					if (beem.Lenght < m || beem.Lenght > max)
						if (restriction)
							list.Add(new AlutechProfileRestriction(model, string.Format("����� ����� ���� {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, m, max)));
				}

				foreach (clsLeaf leaf in model.Leafs)
				{
					min = isImpostLess(leaf) ? 200 : 250;

					foreach (clsBeem beem in leaf)
					{
						int m = beem.PositionBeem == ItemSide.Other || beem.Beem1.PositionBeem == ItemSide.Other || beem.Beem2.PositionBeem == ItemSide.Other ? 250 : min;

						if (beem.Lenght < m || beem.Lenght > max)
							list.Add(new AlutechProfileRestriction(model, string.Format("����� ����� ������� {3} {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, m, max, leaf)));
					}
				}

				// .Lenght - ����� ������� �� ����, .Inside_Lenght - ������� ����� ������ ����, beem.LineC1.Length - �� �����
				foreach (clsBeem beem in model.Imposts)
					if (beem.LineC1.Length < 200 || beem.LineC2.Length < 200 || beem.Lenght > max)
						if (restriction)
							list.Add(new AlutechProfileRestriction(model, string.Format("����� ������� {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, 200, max)));

				////


				// ���������
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
								list.Add(new AlutechProfileRestriction(model, string.Format("���� � ��������� ������ {0} ���������� ������ {1}", model.ProfileSystem.Name, GEISSE)));
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
								list.Add(new AlutechProfileRestriction(model, string.Format("������ {0} ���������� ������ {1}", model.ProfileSystem.Name, FurnSystDver)));
						}

						break;
				}


				/// ������������ ������� �������, ����������� ���������� ���. ��������� ����� - ������� ���� - 200 / 250 ��, � ������������� ���������
				/// ���� ������ ������� ���� 1950, ����� 2400
				/// ���� ������ ������� ���� 1000, ����� 1100��, 
				/// ������� 1000 x 1000
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
								case OpenType.����������:
								case OpenType.���������_��������:
									if (w > 1000 || h > 1950)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("������� {0} x {1} ��, �������� ���������� ������� �������: ������ �� ����� {2}, ������ �� ����� {3} ��",
												w, h, 1000, 1950)));
									break;

								case OpenType.��������:
								case OpenType.���������:

									if (w > 1500 || h > 1000)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("������� {0} x {1} ��, �������� ���������� ������� �������: ������ �� ����� {2}, ������ �� ����� {3} ��",
												w, h, 1500, 1000)));
									break;

								default:
									list.Add(new AlutechProfileRestriction(leaf, string.Format("{0} ��� ���������� {1} �� ������������", model.ProfileSystem.Name, leaf.OpenType)));
									break;
							}

							break;

						case _indoor:
						case _outdoor:
						case _swingdoor:
							switch (leaf.OpenType)
							{
								case OpenType.����������:
								case OpenType.�����������:
									if (w > 1100 || h > 2400)
										if (restriction)
											list.Add(new AlutechProfileRestriction(leaf, string.Format("������� {0} x {1} ��, �������� ���������� ������� �������: ������ �� ����� {2}, ������ �� ����� {3} ��",
												w, h, 1100, 2400)));
									break;
								default:
									list.Add(new AlutechProfileRestriction(leaf, string.Format("{0} ��� ���������� {1} �� ������������ ��� ����������� ���� {2}", model.ProfileSystem.Name, leaf.OpenType, model.ConstructionType.Name)));
									break;
							}

							break;
						default:
							list.Add(new AlutechProfileRestriction(model, string.Format("��� ����������� {0} �� ������������ � ���������� ������� {1}", model.ConstructionType.Name, model.ProfileSystem.Name)));
							break;
					}
				}

				// ������ ���������� ���������� ����� � �������� ��������
				if (model.Leafs.Count > 0 && model.Leafs[0].OpenView == OpenView.������ && (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor))
				{
					foreach (clsBeem beem in model.Imposts)
					{
						clsImpost impost = beem as clsImpost;
						if (impost != null && impost.BalkaType == ModelPart.Shtulp && !impost.Profile.Marking.ToLower().Contains("���"))
							list.Add(new AlutechProfileRestriction(model, string.Format("{0} ���������� ���������� ����� �������� ������ �� �������� `��� �������`", ALUTECH62)));
					}
				}

				// � ������� ���� 0104 / 0105 ���������� ��������� ������, ����� � 0103 + 0805 ,�� ��� �����
				foreach (clsBeem beem in model.Frame)
				{
					if (beem.BalkaType == ModelPart.Porog || unImpostableProfileList.Contains(beem.Profile.Marking))
					{
						foreach (clsBeem impost in beem.ConnectedImposts)
						{
							if (impost.BalkaType == ModelPart.Impost) // ������� �����
								list.Add(new AlutechProfileRestriction(model, string.Format("{0} ���������� ����������� �� ������� '{1} {2}'", impost.Name, beem.Profile.Marking, beem.Profile.Comment)));
						}
					}
				}

				// ���������� ���������� ���������� �� ����� � ������� ������� 0104 / 0105 (����� ��� ������ � ��� �������)
				foreach (clsRegion region in model.VisibleRegions)
				{
					if (region.Fill.Thikness != 0 && region._Frame != null)
					{
						foreach (clsLine line in region.FillConture)
						{
							clsBeem beem = line.Beem;

							if (beem != null && (beem.BalkaType == ModelPart.Porog || unFillableProfileList.Contains(beem.Profile.Marking)))
							{
								list.Add(new AlutechProfileRestriction(model, string.Format("���������� ����������� ���������� {0} �� ������� '{1} {2}' ����������� ������ ��� �����������, ��� ������� ���� ��� ��������� '���_������_�_�������'", region.Part, beem.Profile.Marking, beem.Profile.Comment)));
								break;
							}
						}
					}
				}

				// ������ ������� ������������� �������, �� ��������� � TZ �����������
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
										list.Add(new AlutechProfileRestriction(model, string.Format("{0} ����� '{1} {2}' �� ���������� � ������� '{3} {4}'", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
								else if (adj.bType == ComponentType.Frame)
								{
									if (!leafFrameCompatiblityDictionary[beem.Profile.Marking].Contains(adj.Profile.Marking))
									{
										if (restriction)
											list.Add(new AlutechProfileRestriction(model, string.Format("{0} ����� '{1} {2}' �� ���������� � ����� '{3} {4}', ������������� ������� ���� �������� ���������� �������", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
							}
						}
					}

					// ��������� �������� ��� ������� ������� ������ ��������� ���� .AdjacentBeem(...) ������� ������ �������� ��������� ��� ������ 0 ������� // ������ ����������� !!!
					if (leaf.IsFrameLeaf)
					{
						foreach (clsBeem adj in model.Frame)
						{
							if (adj.bType == ComponentType.Porog)
							{
								clsLeaf beems = leaf.ConnectedBeem(adj); // ��� �����-�� ��������� �����
								foreach (clsBeem beem in beems)
								{
									if (leafPorogCompatiblityDictionary.ContainsKey(beem.Profile.Marking) && !leafPorogCompatiblityDictionary[beem.Profile.Marking].Contains(adj.Profile.Marking))
									{
										if (restriction)
											list.Add(new AlutechProfileRestriction(model, string.Format("{0} ����� '{1} {2}' �� ���������� � ������ ���� '{3} {4}'", leaf.Name, beem.Profile.Marking, beem.Profile.Comment, adj.Profile.Marking, adj.Profile.Comment)));
										break;
									}
								}
							}
						}
					}

					// leaf.ConnectedBeem(model.Frame[0])
				}

				// ������������� ������ ������ � 48 � 62 // 09/07/2018  --������������� ������: 62 ������ , 48 ����, ���� �� ���������� - ����� ������
				clsUserParam upv = model.UserParameters.GetParam("�������������");
				if (upv != null && upv.StringValue != "������")
				{
					upv.StringValue = "������";
				}

				// ������ ������� ��������� ����� �� �����
				if (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.���)
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
				// c# 2.0 ��� �������������� ��������
				// 62
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0303 (������� ��� ������)", new List<string>(new string[] {"��� ������"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0106 (������� ��� ������)", new List<string>(new string[] {"��� ������"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0106 (�������)", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0103 (�������)", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0204", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				leafPorogCompatiblityDictionary.Add("AYPC.W62.0205", new List<string>(new string[] {"AYPC.W62.0801", "AYPC.W62.0802"}));
				// 62 ������� ������� -> ����
				leafFrameCompatiblityDictionary.Add("AYPC.W62.0204", new List<string>(new string[] {"AYPC.W62.0104", "AYPC.W62.0205 (����)"}));
				leafFrameCompatiblityDictionary.Add("AYPC.W62.0205", new List<string>(new string[] {"AYPC.W62.0105", "AYPC.W62.0204 (����)"}));

				// 48
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0104 (������� ������ ��� ������)", new List<string>(new string[] {"��� ������"}));
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0104 (������� ������)", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806"}));

				// 48 ������� T ������ ��� ������������ ��������� � ����� AYPC.C48.0202 (����), �� �� ���� �������� �� ������� ������ ���� ���� �������� �����������
				leafPorogCompatiblityDictionary.Add("AYPC.C48.0202", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806", "��� ������"}));
				leafFrameCompatiblityDictionary.Add("AYPC.C48.0202", new List<string>(new string[] {"AYPC.C48.0105", "AYPC.C48.0203 (����)", "��� ����"}));

				leafPorogCompatiblityDictionary.Add("AYPC.C48.0203", new List<string>(new string[] {"AYPC.C48.0805", "AYPC.C48.0806", "��� ������"}));
				leafFrameCompatiblityDictionary.Add("AYPC.C48.0203", new List<string>(new string[] {"AYPC.C48.0105", "AYPC.C48.0202 (����)", "��� ����"}));
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

				// �������������� ������ ��� ���� ����� ������� �.�. ����� .ChangeLeafSize ��������� ������ .ChangeProfile 
				if (model.LastAction == wdAction.ChangeProfile || model.LastAction == wdAction.AddPerimetr || model.LastAction == wdAction.ChangeProfileSystem || model.LastAction == wdAction.ChangeConstructionType)
				{
					// ���������� ���
					ComponentType type = model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor ? ComponentType.Porog : ComponentType.Frame;

					// �������� ����� ��������
					clsUserParam up = model.UserParameters.GetParam("������ ���������� �������");

					// ������, ��������� ���� ���������������� �� ����� ���������� ������� ��������� ������������
					if (model.LastAction == wdAction.ChangeProfile && model.SelectedBeems != null && model.SelectedBeems.Count > 0 && isOnlyBottomFrame(model.SelectedBeems))
					{
						if (model.SelectedBeems[0].bType != type)
						{
							if (up != null)
								up.StringValue = "��";
						}
					}

					// ������ �������� ���� �� ��� ��� �����������
					if (up != null && up.DefaultValue.StrValue != up.StringValue)
					{
						if (model.ConstructionType.Name != _indoor && model.ConstructionType.Name != _outdoor && model.ConstructionType.Name != _swingdoor)
							up.StringValue = up.DefaultValue.StrValue;
					}

					// ���� ����� ������ ���������� ������� �� ������� ��������
					if (up != null && up.DefaultValue.StrValue != up.StringValue)
						return list;

					// todo ����������� ��� ������ �����, �� �� � ���� ������ �����, ���� ������, �� �� ��� �������� � ���� ������� ���� �������� ��� Frame
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

			// todo �������
			private static bool getRestrictionOnPorog(clsModel model)
			{
				try
				{
					OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
					DataRow[] drdocsign = order.ds.docsign.Select("sign_name = '�����' and signvalue_str = '������ �����������' and deleted is NULL");
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
                        beem.ConnectType = ConnectType.��������;
                    }
                }*/
				clsBeem beem = (clsBeem) subject;
				switch (_componentType)
				{
					case ComponentType.Porog:
						// ��� ���� ��������� � ������ �� ������� ������������ � �������� �� ����� ���� ����� 1 �� �� �� ���� ����
						foreach (clsProfile profile in beem.Model.ConstructionType.ProfForType(_componentType)) // ������ ������ �������� numpos, �� ���� � ����� ������
						{
							if (profile.ProfType == _componentType)
							{
								beem.Profile = profile;
								beem.ConnectType = ConnectType.��������;
								break;
							}
						}

						break;

					case ComponentType.Frame:
						// todo ����������� ��� ������ ����� �� �� ��� ���� ������ � ����� ���� ����������� - ���� �� ������� ���
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
						if (beem.PositionBeem == ItemSide.Bottom && beem.ConnectType != ConnectType.������)
							list.Add(new AlutechSocleInvariant(beem, ConnectType.������));
					}
					else
					{
						if (beem.PositionBeem == ItemSide.Bottom && beem.ConnectType != ConnectType.��������)
							list.Add(new AlutechSocleInvariant(beem, ConnectType.��������));

						if (beem.PositionBeem != ItemSide.Bottom)
							list.Add(new AlutechProfileRestriction(leaf, "��������� ������� ������ ��� ������ �����"));
					}
				}

				return list;
			}

			protected static bool isLeafOnlyProfile(clsBeem beem)
			{
				string mark = beem.Profile.Marking.Split(' ')[0];

				// ����������� � �����
				if (beem.Model.ProfileSystem.GetFrameProfileByMarking(mark, false) != null)
					return false;

				// ����������� � ��������
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
				return string.Format("�� ����� {0} ��������� ����� �� �����������������", ((clsLeaf) subject).Model.ProfileSystem.Name);
			}

			public override void fix()
			{
				((clsLeaf) subject).IsMoskit = IsMoskit.���;
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

			// id �������� �����
			public const int __witeColorGroup = 10;
			public const int __laminatedColorGroup = 1;
			public const int __aluminiumColorGroup = 12;
			public const int __RALColorGroup = 26;

			// ����� ���
			public static bool isWhite(clsConstructionColor color)
			{
				return color.IDColorGroup == __witeColorGroup;
			}

			// ���������
			public static bool isLam(clsConstructionColor color)
			{
				return color.IDColorGroup == __laminatedColorGroup || color.IDParentColorGroup == __laminatedColorGroup;
			}


			// ��������� ��������
			public static bool isFabcolor(clsConstructionColor color)
			{
				return color.IDColorGroup == __aluminiumColorGroup;
			}

			public static bool isPaint(clsConstructionColor color)
			{
				return color.IDColorGroup == __RALColorGroup || color.IDParentColorGroup == __RALColorGroup;
			}


			// ����� ��������� ?
			public static bool isPaintable(clsConstructionColor color)
			{
				return isFabcolor(color) && (color.ColorName == "����� RAL9016" || color.ColorName == "�� ����������"); /// TODO use Dictionary
			}

			// ������ ����� ���� ��� ��� ���� ��� ���������� ��� ������, ��� ������� ������ ����������� �����
			public static bool isAlutechColor(clsConstructionColor color)
			{
				return isFabcolor(color) || isLam(color) || isPaint(color);
			}

			// ������� � ��� �� �������� ���� other ������� ��� ��������� �� color ����� ���� ���������, � ���� ��� ��������� � �� != color �� ������
			public static bool isAcceptFabDependsOther(clsConstructionColor color, clsConstructionColor other)
			{
				return !(alutechColorMap.ContainsValue(other.IDColor) && alutechColorMap.ContainsKey(color.IDColor) && alutechColorMap[color.IDColor] != other.IDColor);
			}

			// ������� ���������� ������ � ��������� ������
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
					return new ColorRestriction(model, string.Format("���� {0} �������� ������ �� ������������ � �������������", string.Join(",", restrictedColorNames.ToArray())));

				// ������ ���� = ��� ������ ������� ������� �� ��������
				int idseller = getIdseller(model);

				// ������ ������������ ��� ������� ������� windraw = 0
				// ��� ����� �������� �� ����������� �� ������ ������� ������� � ��������������
				/* http://yt:8000/issue/wd-34
				if (!model.ColorInside.IsVisible)
				if (restriction)
				return new ColorRestriction(model, string.Format("���� {0} �� ������������ ��� �����������", model.ColorInside.ColorName));

				if (!model.ColorOutside.IsVisible)
				if (restriction)
				return new ColorRestriction(model, string.Format("���� {0} �� ������������ ��� �����������", model.ColorOutside.ColorName));
				*/

				/// ��� ����� ����� � �������������� idcolor � id-������ ������������ � 0 ��� ������ ����������� ���������������� ����� ������
				if (model.ColorOutside.IDColor == 0 || model.ColorOutside.IDColorGroup == 0)
				{
					return new ColorRestriction(model, string.Format("���� {0} �� ����������������, ���������� ����������� ����������� ��� ���, ���� ��� �� ������� �� ���������� ������� ���� ����������� � ������ ���������", model.ColorOutside.ColorName));
				}

				if (model.ColorInside.IDColor == 0 || model.ColorInside.IDColorGroup == 0)
				{
					return new ColorRestriction(model, string.Format("���� {0} �� ����������������, ���������� ����������� ����������� ��� ���, ���� ��� �� ������� �� ���������� ������� ���� ����������� � ������ ���������", model.ColorInside.ColorName));
				}

				/// ������ ������ ���������� ��� ���������� �������� �����, +����� � +��_���������� ����� ���������!, � ���������� ��� ����� ��� ������, 
				/// ���� ������ ������� �� ���� ��� ������� ���� ���� �� ��� �����
				/// ������ ���������� ������ � ���������
				/// ���� ����� �� up ������ ����� RAL 9016
				if (model.ProfileSystem.Name == ALUTECH62 || model.ProfileSystem.Name == ALUTECH_48)
				{
					// ������ ����� ��������� # 12 �������� #1 ��������� ��� ������������ ������ = ����� ��� �������� RAL
					//                    if ((model.ColorInside.IDColorGroup != __aluminiumColorGroup && model.ColorInside.IDColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __RALColorGroup)
					//                        || (model.ColorOutside.IDColorGroup != __aluminiumColorGroup && model.ColorOutside.IDColorGroup != __laminatedColorGroup && model.ColorOutside.IDParentColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __RALColorGroup))
					//					{
					//						return new ColorRestriction(model, string.Format("����������� �� {0} �� ������ ��� �����, ����������� ����� �������� ��� ���������, ��� RAL", model.ProfileSystem.Name));
					//					}

					// ��� ������ ���� ����� �����������������
					if (!isAlutechColor(model.ColorInside) || !isAlutechColor(model.ColorOutside))
						return new ColorRestriction(model, string.Format("����������� �� {0} �� ������ ��� �����, ����������� ����� �������� ��� ���������, ��� RAL", model.ProfileSystem.Name));

					// ���� �������� ��������� ������ �� � ����� ������ === ����������
					//					if (model.ColorInside.IDColorGroup == __aluminiumColorGroup && model.ColorOutside.IDColorGroup == __aluminiumColorGroup)
					//					{
					//						if (model.ColorInside.IDColor != model.ColorOutside.IDColor)
					//							return new ColorRestriction(model, string.Format("�������� ���� ������� {0} � ����� ������ ������ ���� ���������� ��� ����������� ���������", model.ProfileSystem.Name));
					//					}

					// ���� �������� ��������� ������ �� � ����� ������ === ����������
					if (isFabcolor(model.ColorInside) && isFabcolor(model.ColorOutside) && model.ColorInside.IDColor != model.ColorOutside.IDColor)
						return new ColorRestriction(model, string.Format("��������� ���� ������� {0} � ����� ������ ������ ���� ���������� ��� ����������� ��������� ��� �������� � ����� �� ������� RAL", model.ProfileSystem.Name));

					// ���� ������� �� RAL == ���������� �� ������������ � ��������� ��� ���� � ����
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

					// ���� ���������� � ������ ����� �� ������ ��������� ���������
					// �������� ��������, 20.03.2019 12:36:52:
					// �� �� �� ������ ������� � ������ ����� � ������ ������, ������� ������� �� ����
					// if (isPaint(model.ColorInside) && isPaint(model.ColorOutside) && model.ColorInside.IDColor != model.ColorOutside.IDColor)
					//  return new ColorRestriction(model, "����������� ���������� ����� �������� ����� ������ �������");

					// ���� �������� �� ������ ���� ��� ����� �� ��� �������������
					if (isPaint(model.ColorInside) && !(model.ColorInside.IDColor == model.ColorOutside.IDColor || isPaintable(model.ColorOutside)) ||
						isPaint(model.ColorOutside) && !(model.ColorInside.IDColor == model.ColorOutside.IDColor || isPaintable(model.ColorInside)))
					{
						if (restriction)
							return new ColorRestriction(model, string.Format("��������� �������� ������ ������� ���������� ����� {1} ��� {2}", model.ProfileSystem.Name, "����� RAL 9016", "�� ����������")); // TODO Dictionary colors
					}

					// ���� �������� �� up.������ ALUTECH = ����� RAL 9016 ��� ������������
					if (isPaint(model.ColorInside) || isPaint(model.ColorOutside))
					{
						clsUserParam up = model.UserParameters.GetParam("������ ALUTECH");
						if (up != null && up.StringValue != "����� RAL9016" && up.StringValue != "�� ����������")
						{
							up.StringValue = "����� RAL9016";
						}
					}
					else if (isLam(model.ColorInside) || isLam(model.ColorOutside))
					{
					}
					else
					{
						clsUserParam up = model.UserParameters.GetParam("������ ALUTECH");
						if (up != null && up.StringValue != up.DefaultValue.StrValue)
						{
							up.StringValue = up.DefaultValue.StrValue;
						}
					}
				}
				else if (model.ProfileSystem.Name == SystSlayding60)
				{
					// �������� ����� ���� ��� ����� ��� � ����� ����� RAL ������ �����
					if ((isWhite(model.ColorInside) && isWhite(model.ColorOutside)) || (isPaint(model.ColorInside) && isPaint(model.ColorOutside) && model.ColorInside.ID == model.ColorOutside.ID))
					{
						// ok
					}
					else
					{
						if ((isPaint(model.ColorInside) || isWhite(model.ColorInside)) /*&& isWhite(model.ColorOutside)*/ && model.LastAction == wdAction.ChangeInsColor)
						{
							return new SimpleInvariant(model, string.Format("{0} ������ ���� ������� � ����� ������ ���������", model.ProfileSystem.Name), delegate { model.ColorOutside = model.ColorInside; });
						}
						else if ((isPaint(model.ColorOutside) || isWhite(model.ColorOutside)) /*&& isWhite(model.ColorInside)*/ && model.LastAction == wdAction.ChangeOutColor)
						{
							return new SimpleInvariant(model, string.Format("{0} ������ ���� ������� � ����� ������ ���������", model.ProfileSystem.Name), delegate { model.ColorInside = model.ColorOutside; });
						}
						else
						{
							return new ColorRestriction(model, string.Format("{0} ������������ � ����� ������ ����� ������ RAL ��� �������� �����", model.ProfileSystem.Name));
						}
					}
				}
				else if (model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74 || model.ProfileSystem.Name == ProfSystGlass)
				{
					if (model.ColorInside.IDColorGroup != __witeColorGroup || model.ColorOutside.IDColorGroup != __witeColorGroup)
						return new ColorRestriction(model, string.Format("{0} ������ �����", model.ProfileSystem.Name));
				}
				else if ((model.ProfileSystem.Name == SibDesign && idseller != 798) // ��������� �����
					|| model.ProfileSystem.Name == RehauEuro70
					|| model.ProfileSystem.Name == RehauOptima || model.ProfileSystem.Name == Eco60 || model.ProfileSystem.Name == EnwinMinima
					|| ((model.ProfileSystem.Name == RehauBlitzNew || model.ProfileSystem.Name == RehauGrazio) && !isRbnGrazioGrey(idseller))) // ���� �����, �� ���� ���� �������� ���� ��� ������ ������
				{
					// ��� ������ ������ �����
					if (model.ColorInside.IDColorGroup != __witeColorGroup || model.ColorOutside.IDColorGroup != __witeColorGroup)
						if (getRestriction(model))
							return new ColorRestriction(model, string.Format("���������� ������� {0} �� ������������", model.ProfileSystem.Name));
				}
				else if (model.ProfileSystem.Name == Vario || model.ProfileSystem.Name == DECOR || model.ProfileSystem.Name == "Rehau 60�� � �����" || model.ProfileSystem.Name == "Rehau 70�� � �����" || model.ProfileSystem.Name == BAUTEKmass)
				{
					if (!isLam(model.ColorInside) || !isLam(model.ColorOutside))
					{
						if (restriction) // �������� ����� 192210
							return new ColorRestriction(model, string.Format("� ���������� ������� {0} ��������� �����������", model.ProfileSystem.Name));
					}
				}
				else if (model.ProfileSystem.Name == FORWARD && idseller == 798)
				{
					if (isLam(model.ColorInside) || isLam(model.ColorOutside))
					{
						if (getRestriction(model))
							return new ColorRestriction(model, string.Format("���������� ������� {0} �� ������������", model.ProfileSystem.Name));
					}
				}
				else
				{
					// ��� ������ ������������ ����� ��������
					// ������ ����� ��������� # 10 ����� #1 ��������� ��� ������������ ������ = �����
					if ((model.ColorInside.IDColorGroup != __witeColorGroup && model.ColorInside.IDColorGroup != __laminatedColorGroup && model.ColorInside.IDParentColorGroup != __laminatedColorGroup)
						|| (model.ColorOutside.IDColorGroup != __witeColorGroup && model.ColorOutside.IDColorGroup != __laminatedColorGroup && model.ColorOutside.IDParentColorGroup != __laminatedColorGroup))
					{
						return new ColorRestriction(model, string.Format("����������� {0} �� ���������� ����� ��������", model.ProfileSystem.Name));
					}
				}

				// ��������� ��� �� ������� � ����������� �������
				if (model.ProfileSystem.Name == EVO)
				{
					if (model.ColorInside.IDColorGroup == __laminatedColorGroup || model.ColorInside.IDParentColorGroup == __laminatedColorGroup || model.ColorOutside.IDColorGroup == __laminatedColorGroup || model.ColorOutside.IDParentColorGroup == __laminatedColorGroup)
					{
						foreach (clsLeaf leaf in model.Leafs)
						{
							if (isZ64(leaf) && getRestriction(model))
							{
								return new ColorRestriction(model, string.Format("{0} ������� L64 �� ������������", model.ProfileSystem.Name));
							}
						}
					}
				}

				// todo
				// eurodesign �� ������������ � ������� �������
				// ������� �� ���������� ���������

				return null;
			}
		}


		/// much useful
		private const string ���������� = "����������";
		/// ��������� ������� ����������� �������
		private const string __systemDepth = "��������� �������";
		// ���
		private const int __system60 = 60; // ����� | �������
		private const int __system70 = 70; // �����
		private const int __system80 = 80; // ����� INTELIO 80 aka NEO 80
		private const int __system86 = 86; // ����� Geneo
		// ������
		private const int __system48 = 48;
		private const int __system62 = 62;

		// ��������� ������� ���������
		internal const string __mechanizm = "��������";
		internal const string __mechanizmFlag = "������������";
		internal const string __hinge = "�����";

		// todo structure it
		internal const string __office1 = "������� - ������������ ����� ����/����";
		internal const string __office1b = "������� - ������������ ����� ����/�������";
		internal const string __officeM = "������� - ������������� ����/����";
		internal const string __officeMb = "������� - ������������� ����/�������";

		internal const string __keykey = "�������� ����/����";
		internal const string __keybar = "�������� ����/�������";
		internal const string __keykey1 = "�������� ������������ ����/����";
		internal const string __keybar1 = "�������� ������������ ����/�������";
		internal const string __balcon = "��������� ��������";
		internal const string __balconk = "��������� �������� ����/����";
		internal const string __balconb = "��������� �������� ����/�����";
		internal const string __wc = "��������� ��������";
		internal const string __window = "�������";
		internal const string __no = "���";

		internal const string __portalOneSide = "������ �������������";
		internal const string __portalOneSideKey = "������ ������������� � ������";
		internal const string __portalKeyKey = "������ ������������� ����/����";
		internal const string __portalKeyBar = "������ ������������� ����/�������";

		internal const string __white = "�����";
		internal const string __brown = "����������";
		internal const string __whitebrown = "�����/����������";
		internal const string __bronze = "������";
		internal const string __silver = "�������";
		internal const string __darksilver = "������ �������";
		internal const string __gold = "������";

		private static bool isDev(clsModel model)
		{
			clsUserParam up = model.UserParameters.GetParam(����������);
			return up != null && up.StringValue == "��";
		}

		private static bool isNewMech(clsModel model)
		{
			clsUserParam up = model.UserParameters.GetParam(__mechanizmFlag);
			return up != null && up.StringValue == true.ToString();
		}

		private static bool isLegacy(clsModel model)
		{
			return model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74 || model.ProfileSystem.Name == SystSlayding60 || model.ProfileSystem.Name == "���������� �����������";
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
				throw new ArgumentException(string.Format("�������� {0} �� ������", __mechanizmFlag));
			}
		}

		/// ��������� ������ ������ �� ��� ���� ������, ������� ������ Visible = false
		/// ��� �������� � ������������� ��� � �������� ������, �� � ��� ��� ���� � ���������
		/// ������� �� ������ ������� �� �������� ������ �� ������ � ���� ���� ����� ������ ��� ��������� �����
		/// 
		private void hideFlag(clsModel model)
		{
			//            clsUserParam up = model.UserParameters.GetParam(__mechanizmFlag);
			//            if (up != null)
			//                up.Visible = false;
		}

		//		private static readonly string[] toDisableStrings = new string[] { "����������", "������� ������", "�������� �������", "������� �����", "��������� ���������� �������", "��� �����", "�������� ���������", "������� �����", "������������� �����", "������� �����", "��� �������" };
		private static readonly string[] oldParameterNames = new string[] {"������� �����", "������� ������", "�������� �������", "��������� ���������� �������", "�������� ���������", "������� �����", "������� �����", "��� �������", "��� �����"};
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

					// ��� ������������� �����������
					case wdAction.ShowModel:

						/// ��������  - ���� � ������
						/// 1) ���� ������ = ����������� �� ��������� ShowModel
						/// 2) ���� ��������� ������� � ����������� �� ���� ����� Draw �� ���������� :( ������� :((
						/// 3) ����� ������ ���������� � model.CalcVariables ����� ������� "������" � ���������������� ��������������� � ����������� // ������ �����
						/// ������ ��� ���������� ������ ������� Save(...) �� ������� ����������� ������
						/// ��� ���� ������������ ������� "������" �������� ����� SelectModel - ���� ���� ������������-�������� ..... !
					case wdAction.SelectModel:

						// ������� �������� ������ ������ ��� ����� �� ����������� � ����� �������
					case wdAction.BeforeModelCalc:
						recognizeDoorParameters(model);
						break;
				}
			}
		}

		private class TransformData
		{
			// �� ����
			public clsConstructionType constructionType;
			public clsLeaf leaf;

			// ������ ���������
			public HandleType oldHandleType;

			// ��� ����
			public string �������_�����; // ���� ������� �����

			// ��� ��������� ������
			public string ���_�����; // ��� ����� ��� ��������� ������, �������� ��.....
			public string ���_�����_����; // ���� ��������� ���������, �� �� ����� �� ������� !, ����� ��� ��� ....
			public string ���������_�����; // ��������

			// ��� ������
			public string �������_������_����;
			public string ��������_�������;
			public string �������_�����;
			public string �������_�����_����������;
			public string ��������_���������; // ���� ��������� ���������
			public string �������_�����; // ���� ������� �����
			public string ���_�������; // ������� | ���������
			public string �������������_�����; // �� ����� | �� ����� = todo �� ������� �� ��� ������

			// ����� ���������
			public HandleType newHandleType;
			public string ��������;
			public string ��������_����;
		}

		/// ������������� ������ ������������� :
		/// 1) �������� 1 - ���, ��� ������ � ������� �������� model.up ��� ���� �� ��� ����� ��� ���� ��� ����� �� ����������, �������������� �� ������� ����������� ������������ :\ - ������ ����
		/// 2) ������������ ��� �� ��� �� ������ � ����� ��������������� 
		/// 4) ������ ��������� � <calcVariables>Doors</calcVariables> 
		/// 5) ��� ������ ������� ������� up ������������
		private void recognizeDoorParameters(clsModel model)
		{
			//            return;

			/// ������ ������ ������ ������, ������� ������ ������� (������ ����� ���������� � ������ ����)
			/// ��� ������ = ��� ���������, �� � CalcVariables
			/// ���� ����� ������ �� ���������� ������ � ������� ��� � ��������
			/// ���� �� ����� - �� �� ��� - �����
			/// ������ ������� ������ �� ��������� clsUserParam, ��� ������� �������� � classNative, �� � class ��������� ������ �������
			/// clsUserParam.Visible - ������������ � classNative ������� ���������� ��������� ���� ���, � ������ �� ����������� ��� ������� ��� ���������� ������� ��� ���� ��������� ������
			/// ����� ���������� ��������� � modelparam.isactive ����� ��������� classNative ��� �� ����� ����� ����� �������� ��� ������� ����������� ���-���
			/// ����� classNative ��������� ���� ��������, ����� ����������� ��������! ����������� � �������� �������/������
			/// �������� ���� [��� �����] ������� ��������� ��� ���������, �� ��������� useunvisible � class �� ������� ������� ����� � ������ ���������� �������, ���� ���� ������� ����� ���������� =>
			/// �����-�� ���������� ������� �� ������ ����� ����� ��� ��������� ����� ��� �������� �����, ������� ���� � ����� ������� �������� �������� - ��� ��������� ���� �����
			if (isNewMech(model))
				return;

			/// ��� ���� ��������� ���� �������� ������ �� ���������� �����-�� ��������� ������ YES | NO | Cancel
			/// �� � ���� YES ����� ����� ����� ��������� ����� ����� ���� �� �������, 
			/// ��������� ��� ������ ����� ���� ����������� � ������� � ������� ������ ���������, ����� ��� ��� ��������� ( � ����� � ������ ?)
			/// �� ������� �� �� ������ ������ ����������� ������������ ������ ���������, �� ���� ������ ���� ��� � .Enable ??
			/// ����� !!! ���� ������ enabled � �����-�� ��������� DataTable, � ������� "���" ������� �������� ����� ����� //
			/// Tools.ChangeSetting("Visibility", "��� ������", true);


			/// ���� ���c�� 
			List<TransformData> transformDataList = new List<TransformData>();

			foreach (clsLeaf leaf in model.Leafs)
			{
				clsUserParam up = leaf.UserParameters[__mechanizm];
				if (up == null)
					throw new Exception(string.Format("��������� ��������, ����������� �������� {0}", __mechanizm));

				// todo List<struct>.Add() �������� �� ��������, � List<class>.Add() �� ������ ����� ��� �������� !!!!?????
				TransformData transformData = new TransformData();
				transformDataList.Add(transformData);

				// ����������
				transformData.constructionType = leaf.Model.ConstructionType;
				transformData.leaf = leaf;
				transformData.oldHandleType = leaf.HandleType;

				// ~todo ��� - ������ ���, ����� �� �����������
				if (leaf.HandleType == HandleType.���_�����)
				{
					transformData.newHandleType = HandleType.���_�����;
					transformData.�������� = __no;
					if (leaf.ShtulpLeaf != null)
					{
						/// ����������� ���� ��������� �� �������� �������
						/// ����� ��� ��� �� ��������� ����� � ������� ����� 
						/// ����� ��� ����� �� ������ �������
					}
					else
					{
						transformData.��������_���� = up.DefaultValue.StrValue2;
					}
				}
					// ~todo ���� �����, ���� - ����� ����� ����, ����� ������ �������
				else if (leaf.Model.ConstructionType.Name == _window)
				{
					transformData.newHandleType = HandleType.�������;
					transformData.�������� = __window;

					// ���� ������� �����
					clsUserParam upWindowHandleColor = leaf.UserParameters.GetParam("������� �����");
					if (upWindowHandleColor != null && upWindowHandleColor.Visible)
					{
						transformData.�������_����� = upWindowHandleColor.StringValue;
						switch (transformData.�������_�����)
						{
							case "�����":
								transformData.��������_���� = __white;
								break;
							case "����������":
								transformData.��������_���� = __brown;
								break;
							default:
								transformData.��������_���� = up.DefaultValue.StrValue2;
								break;
						}
					}
					else
					{
						transformData.��������_���� = up.DefaultValue.StrValue2;
					}
				}
					/// ~todo ���� �����, ��������� �����, �� ��� ����� leaf.HandleType ������� �� ���� ����������,
					/// �������� ���_����� ����� ������ ������, �� � ���� ���� ����� ������� ����� ������ == false
				else if (model.ConstructionType.Name == _balcon)
				{
					clsUserParam upBalconHandle = leaf.UserParameters.GetParam("��� �����");
					if (upBalconHandle != null && upBalconHandle.Visible && upBalconHandle.StringValue == "��������� ���������")
					{
						transformData.���_����� = "��������� ���������";
						transformData.newHandleType = HandleType.��������_��������;
						transformData.�������� = __balcon;

						transformData.���_�����_���� = upBalconHandle.StringValue2;
						switch (transformData.���_�����_����)
						{
							case "�����":
							transformData.��������_���� = __white;
							break;
							case "����������":
							transformData.��������_���� = __brown;
							break;
							case "�����/����������":
							transformData.��������_���� = __whitebrown;
							break;
							default:
							transformData.��������_���� = up.DefaultValue.StrValue2;
							break;
						}

						// ��������� ��� �� ������ ��� ��, �� ��� ��� ������ ����/���� == �������, ����/����� == � ������
						clsUserParam upCilinder = leaf.UserParameters.GetParam("�������� �������");
						if (upCilinder != null && upCilinder.Visible && upCilinder.StringValue == "� ������")
						{
							transformData.�������� = __balconb;
						}
						else if (upCilinder != null && upCilinder.Visible && upCilinder.StringValue == "�������")
						{
							transformData.�������� = __balconk;
						}
					}
					else
					{
						transformData.���_����� = "������� �����";
						transformData.newHandleType = HandleType.�������;
						transformData.�������� = __window;
						// ���� ������� ����� ����������, �� ��� ����� ���� ��������, � "��� �����", � � ������ ��������� ������
						clsUserParam upWindowHandleColor = leaf.UserParameters.GetParam("������� �����");
						if (upWindowHandleColor != null && upWindowHandleColor.Visible)
						{
							transformData.�������_����� = upWindowHandleColor.StringValue;
							switch (transformData.�������_�����)
							{
								case "�����":
									transformData.��������_���� = __white;
									break;
								case "����������":
									transformData.��������_���� = __brown;
									break;
								default:
									transformData.��������_���� = up.DefaultValue.StrValue2;
									break;
							}
						}
						else
						{
							transformData.��������_���� = up.DefaultValue.StrValue2;
						}
					}
				}
					// ����� ������������ � �������
				else if (model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor)
				{
					//                // ��� ������
					//                public string �������_������_����;
					//                public string ��������_�������;
					//                public string �������_�����;
					//                public string �������_�����_����������;
					//                public string ��������_���������; // ���� ��������� ���������
					//                public string �������_�����; // ���� ������� �����
					//                public string ���_�������; // ������� | ���������
					//                public string �������������_�����; // �� ����� | �� ����� = todo �� ������� �� ��� ������

					// ��� ���������� ����� ������� ������ ���� ��� �������� ������� ����� - ��� (��� ��������), �.�. / �������
					transformData.newHandleType = leaf.HandleType;
					if (leaf.HandleType == HandleType.��������_�������� || leaf.HandleType == HandleType.�������)
					{
						clsUserParam upDoorLock = leaf.UserParameters.GetParam("������� �����");
						if (upDoorLock != null && upDoorLock.Visible)
						{
							transformData.�������_����� = upDoorLock.StringValue;
							transformData.�������_�����_���������� = upDoorLock.StringValue2;
						}

						clsUserParam upCilinder = leaf.UserParameters.GetParam("�������� �������");
						if (upCilinder != null && upCilinder.Visible)
						{
							transformData.��������_������� = upCilinder.StringValue;
						}

						/// �� �� �� �� ������ ������ �������
						clsUserParam upLatch = leaf.UserParameters.GetParam("��� �������");
						if (upLatch != null && upLatch.Visible)
						{
							transformData.���_������� = upLatch.StringValue;
						}

						clsUserParam upMLL = leaf.UserParameters.GetParam("�������������_�����");
						if (upMLL != null && upMLL.Visible)
						{
							transformData.�������������_����� = upMLL.StringValue;
						}
						///

						/// ~todo~ //////////////////////////////////////////////////
						if (leaf.HandleType == HandleType.��������_��������)
						{
							// ��������
							if (transformData.�������_����� == "������������� �� �����" || transformData.�������_����� == "������������� �� �����" || transformData.�������_����� == "������������")
							{
								if (transformData.��������_������� == "�������")
									transformData.�������� = __keykey;
								else if (transformData.��������_������� == "� ������")
									transformData.�������� = __keybar;
								else
									transformData.�������� = __keykey;
							}
							else if (transformData.�������_����� == "�������")
							{
								transformData.�������� = __wc;
							}
							else
							{
								transformData.�������� = __keykey;
							}

							// ����
							clsUserParam upDoorHandle = leaf.UserParameters.GetParam("�������� ���������");
							if (upDoorHandle != null && upDoorHandle.Visible)
							{
								transformData.��������_��������� = upDoorHandle.StringValue;
								switch (upDoorHandle.StringValue)
								{
									case "�����":
										transformData.��������_���� = __white;
										break;
									case "����������":
										transformData.��������_���� = __brown;
										break;
									case "�����/����������":
										transformData.��������_���� = __whitebrown;
										break;
									default:
										/// todo ��� ��� ������ ����������� � ����� = ��� �����
										MessageBox.Show(string.Format("���� �������� ��������� '{0}' �� ���������� � ����� ���������� ������", upDoorHandle.StringValue), "��������!");
										transformData.��������_���� = up.DefaultValue.StrValue2;
										break;
								}
							}
							else
							{
								transformData.��������_���� = up.DefaultValue.StrValue2;
							}
						}

						/// ~todo~ //////////////////////////////////////////////////
						if (leaf.HandleType == HandleType.�������)
						{
							// �������� 
							if (transformData.�������_����� == "������������")
							{
								if (transformData.��������_������� == "�������")
									transformData.�������� = __office1;
								else if (transformData.��������_������� == "� ������")
									transformData.�������� = __office1b;
								else
									transformData.�������� = __office1;
							}
							else if (transformData.�������_����� == "������������� �� �����" || transformData.�������_����� == "������������� �� �����")
							{
								if (transformData.��������_������� == "�������")
									transformData.�������� = __office1; // TODO ������ !!! ������ !!
								else if (transformData.��������_������� == "� ������")
									transformData.�������� = __office1b; // TODO ������ !!! ������ !!
								else
									transformData.�������� = __office1; // TODO ������ !!! ������ !!
							}
							else
							{
								transformData.�������� = __office1;
							}

							// ����
							clsUserParam upOfficeHandle = leaf.UserParameters.GetParam("������� �����");
							if (upOfficeHandle != null && upOfficeHandle.Visible)
							{
								transformData.�������_����� = upOfficeHandle.StringValue;
								switch (upOfficeHandle.StringValue)
								{
									case "�����":
										transformData.��������_���� = __white;
										break;
									case "����������":
										transformData.��������_���� = __brown;
										break;
									case "�����/����������":
										transformData.��������_���� = __whitebrown;
										break;
									default:
										/// todo ???
										MessageBox.Show(string.Format("���� ������� ����� '{0}' �� ���������� � ����� ���������� ������", upOfficeHandle.StringValue), "��������!");
										transformData.��������_���� = up.DefaultValue.StrValue2;
										break;
								}
							}
							else
							{
								transformData.��������_���� = up.DefaultValue.StrValue2;
							}
						}

						/// ���� ������ = �����
						clsUserParam upHingeColor = leaf.UserParameters.GetParam("������� ������");
						if (upHingeColor != null && upHingeColor.Visible)
						{
							transformData.�������_������_���� = upHingeColor.StringValue2;

							// todo �������� ���� ��������� ������������ � ������ ���������
						}
					}
					else
					{
						transformData.newHandleType = HandleType.���_�����;
						transformData.�������� = __no;
						transformData.��������_���� = up.DefaultValue.StrValue2;
					}
				}
				else if (model.ConstructionType.Name == _moskit)
				{
					transformData.newHandleType = HandleType.���_�����;
					transformData.�������� = __no;
					transformData.��������_���� = up.DefaultValue.StrValue2;
				}
				else if (model.ConstructionType.Name == _pskportal)
				{
					transformData.newHandleType = leaf.HandleType;
					transformData.��������_���� = up.DefaultValue.StrValue2;
					switch (leaf.HandleType)
					{
						case HandleType.��������_��������:
							transformData.�������� = __keykey;
							break;
						case HandleType.�������:
							transformData.�������� = __office1;
							break;
						default:
							transformData.�������� = __no;
							break;
					}
				}
					// ������
				else
				{
					throw new NotImplementedException(string.Format("���������� ���������� ������� � ������ �������. ��� ����������� {0} �� ����������", leaf.Model.ConstructionType.Name));
				}
			}

			// ������������ ���� �� ��������� �������� �� ����� �������� �������
			foreach (TransformData data in transformDataList)
			{
				if (data.leaf.HandleType == HandleType.���_����� && data.leaf.ShtulpLeaf != null)
					foreach (TransformData data2 in transformDataList)
						if (data2.leaf == data.leaf.ShtulpLeaf)
							data.��������_���� = data2.��������_����;
			}

			///
			/// ������� ��� ��� � �������� � �������� ������������
			///
			StringBuilder sb = new StringBuilder();
			foreach (TransformData t in transformDataList)
			{
				sb.Append(string.Format("\n{0} {1}\n======================================\n", t.constructionType.Name, t.leaf.Name));
				sb.Append("������ ���������:\n");
				sb.Append(string.Format("��� �����:{0}\n", t.oldHandleType));

				if (t.constructionType.Name == _window || t.constructionType.Name == _balcon)
				{
					sb.Append(string.Format("�������_�����: {0}\n", t.�������_�����));
					sb.Append(string.Format("���_�����: {0}, ����: {1}\n", t.���_�����, t.���_�����_����));
					sb.Append(string.Format("���������_�����: {0}\n", t.���������_�����));
				}
				else
				{
					sb.Append(string.Format("�������_������_����: {0}\n", t.�������_������_����));
					sb.Append(string.Format("��������_�������: {0}\n", t.��������_�������));
					sb.Append(string.Format("�������_�����: {0}, �������_�����_����������: {1}\n", t.�������_�����, t.�������_�����_����������));
					sb.Append(string.Format("��������_���������: {0}\n", t.��������_���������));
					sb.Append(string.Format("�������_�����: {0}\n", t.�������_�����));
					sb.Append(string.Format("���_�������: {0}\n", t.���_�������));
					sb.Append(string.Format("�������������_�����: {0}\n", t.�������������_�����));
				}

				sb.Append("\n����� ���������:\n");
				sb.Append(string.Format("��� �����:{0}\n", t.newHandleType));
				sb.Append(string.Format("��������:{0}\n", t.��������));
				sb.Append(string.Format("����:{0}\n", t.��������_����));

				if (t.constructionType.Name == _indoor || t.constructionType.Name == _outdoor)
				{
					sb.Append(string.Format("������� �����:{0}\n", t.��������_����));
				}
			}

			// ���� � transformDataList ������ ������ � ������� �� ����� � ���������
			bool ask = false;
			foreach (TransformData transformData in transformDataList)
			{
				if (transformData.constructionType.Name != _moskit)
					ask = true;
			}

			DialogResult result = ask ? MessageBox.Show(sb.ToString(), "���������� ���������� ������� � ������ �������", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) : DialogResult.OK;

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
						up.StringValue = transformData.��������;
						up.StringValue2 = transformData.��������_����;
					}
				}

				// fix flag
				setNewMech(model, true);
			}
		}

		/// ������ ��� ������������� up.strValue <= �������.���_�����
		private void upht(clsModel model)
		{
			// TODO
			if (!isNewMech(model))
				return;

			// xxx foreach (clsLeaf leaf in model.Leafs.SelectedLeafs) // �� ������ ��� ������� �������
			foreach (clsLeaf leaf in model.Leafs) // � ��� �� ������� ��� ������� ������ - ���� �� ���
			{
				clsUserParam up = leaf.UserParameters[__mechanizm];
				if (up != null)
				{
					switch (leaf.HandleType)
					{
						case HandleType.��������_��������:
							if (up.StringValue != __keykey1 && up.StringValue != __keybar1 && up.StringValue != __keykey && up.StringValue != __keybar && up.StringValue != __wc && up.StringValue != __balcon && up.StringValue != __balconk && up.StringValue != __balconb && up.StringValue != __portalOneSide && up.StringValue != __portalOneSideKey && up.StringValue != __portalKeyKey && up.StringValue != __portalKeyBar)
								up.StringValue = model.ConstructionType.Name == _pskportal ? __portalKeyKey : __keykey;
							break;
						case HandleType.�������:
							if (up.StringValue != __office1 && up.StringValue != __office1b && up.StringValue != __officeM && up.StringValue != __officeMb)
								up.StringValue = __office1;
							break;
						case HandleType.�������:
							up.StringValue = __window;
							break;
						case HandleType.���_�����:
							up.StringValue = __no;
							break;
					}
				}
			}
		}

		/// �������� ��� ������������� �������.���_����� <= up.strValue
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
						leaf.HandleType = HandleType.�������;
						break;
					case __keykey:
					case __keybar:
					case __keykey1:
					case __keybar1:
					case __wc:
						leaf.HandleType = HandleType.��������_��������;
						break;

					case __balcon:
					case __balconk:
					case __balconb:
						leaf.HandleType = HandleType.��������_��������;
						break;

					case __window:
						leaf.HandleType = HandleType.�������;
						break;
					case __no:
						leaf.HandleType = HandleType.���_�����;
						break;

					case __portalOneSide:
					case __portalOneSideKey:
					case __portalKeyKey:
					case __portalKeyBar:
						leaf.HandleType = HandleType.��������_��������;
						break;

					default:
						MessageBox.Show("����������� ��������"); // �� ������ ������ ���� �� �����
						break;
				}

				model.WinDraw.Refresh();
			}
		}


		/// ������ ���������� ��� ����� �� BMC � ����� �����������, ���
		/// 1) ���� ���������� ������� (��� ������ �����������) ������ �� ��� ���� ��������� clssNative �� ���� ��� ��� �������� .Calc(), � AMC ��� ������
		/// 2) ���� ������������, � � ��� � ����������� ����� ������ ������������ ��� ��� ���� �� ���������, � ��������� ������� ������ �����, ��������� �� �� ��� ���� � classNative
		private void setInvariants(clsModel model)
		{
			// �� �������  // �����������������
			if (model.ProfileSystem.Name == Vario && (model.FurnitureSystem.Name == RotoNT || model.FurnitureSystem.Name == RotoNTDesigno))
				model.UserParameters["������������ ���������"].StringValue = "���������������";

			// ��������� �������
			foreach (clsLeaf leaf in model.Leafs)
			{
				setInvariants(leaf);
			}

			// ��������� ������� �����(int) ��� ������ ������
			clsUserParam up = model.UserParameters[__systemDepth];
			if (up != null)
			{
				int? depth = getSystemDepth(model.ProfileSystem.ID);				
				up.StringValue = depth != null ? depth.ToString() : string.Empty;
			}

			// Z60 Antik // �� ������ ��� ��� ����� ������� ��������
			if (restriction)
				RehauZ60AntikInvariant.fix(model);
		}

		/// ������������� �� ���� ��������� �����������: ���������; ���, ��������, ������, etc. :: /// deWrapper
		private static int? getSystemDepth(int idprofsys)
		{
			// lazy proxy inside
			return CalcProcessor.Modules["getSystemDepth"].Invoke(new object[] {idprofsys})[0] as int?;
		}

		// ���������� ������� �������
		private static void setInvariants(clsLeaf leaf)
		{
			/// ���������� ������ ������ � userparam �.�. �����-�� ��� ������� ������� � �������� // todo ������ ������������� ����_���������
			leaf.UserParameters["������� ������"].NumericValue = leaf.HingeCount;

			/// todo ����� ����
			/// �������� ������ ����� ����� // ������� ����� ���������� � ����� ������ ������: ����� �������, �������, ���� ����������, ���� �����������
			/// ����������� ����� ���� �����������
			/// Dornmas  = (c-e)/2
			/// ����� ���������� �� ���������� ���� 0,8, 15, 25, 35
			/// ��� ����� ���� ��� ������ ����������� ���� �� �������� �� ������� �������
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

		// todo �������� ������������� ������������ () � ������� �������� ��� ��������� ���� ������� , ����� ����� ������������ �������  - ����� ������ / ������������
		// ���� ����� � ��� ��� ���������� ������������� ��� � ������� ������ ��� ���-�� ������ - ��������
		private class DoorInvalidance : Invalidance
		{
			private readonly string _message;

			//    SELECT DISTINCT --constructiontype.name, 
			//    system.name 
			//    FROM constructiontypedetail 
			//    INNER JOIN systemdetail on constructiontypedetail.idsystemdetail = systemdetail.idsystemdetail AND constructiontypedetail.deleted IS NULL AND systemdetail.deleted IS NULL AND constructiontypedetail.isactive = 1 AND systemdetail.isactive = 1
			//    INNER JOIN system ON systemdetail.idsystem = system.idsystem AND system.deleted IS NULL AND (system.isactive =1 OR system.idsystem = 50)
			//    INNER JOIN constructiontype ON constructiontypedetail.idconstructiontype = constructiontype.idconstructiontype AND constructiontype.deleted IS NULL AND constructiontype.isactive = 1
			//    WHERE constructiontype.name = '����� ������������' OR constructiontype.name = '����� �������'
			//    --ORDER BY constructiontype.name, system.name 
			//            ALUTECH ALT W62
			//            Pimapen
			//            Rehau 60��
			//            Rehau 70
			//            Rehau 70��
			//            Vario
			//            ������ ��45 ?
			//            ������ ���74 ?

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

				// ������� �����
				if (model.ConstructionType.Name == _pskportal)
					return list;

				// ��(�) �����
				if (model.ProfileSystem.Name == KP45 || model.ProfileSystem.Name == KPT74)
					return list;

				if (!isNewMech(model))
					return list;

				// ��� ����������� ~ ���������
				if ((model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor || model.ConstructionType.Name == _swingdoor || model.ConstructionType.Name == _wcdoor) && model.FurnitureSystem.Name != FurnSystDver && model.FurnitureSystem.Name != FurnSystBEZ)
				{
					list.Add(new DoorInvalidance(model, string.Format("{0} ���������� ������ {1}", model.ConstructionType.Name, FurnSystDver)));
				}

				//���������� ������� ������� ������� ��  ������������ �������  ������ ��� ���������� ������

				int systemDepth;
				try
				{
					clsUserParam upSystemDepth = model.UserParameters.GetParam(__systemDepth);
					systemDepth = int.Parse(upSystemDepth.StringValue);
				}
				catch
				{
					list.Add(new DoorInvalidance(model, string.Format("�������� {0} �� ������, ���� ������ ����������, ��������� ���������", __systemDepth)));
					return list;
				}
				// ���� ���������  // todo �� ���� ����
				if (model.FurnitureSystem.Name == FurnSystDver)
				{
					// ������� ������� - ���������� ���� - ������ 252/03/20
//					int systemDepth;
//					try
//					{
//						clsUserParam upSystemDepth = model.UserParameters.GetParam(__systemDepth);
//						systemDepth = int.Parse(upSystemDepth.StringValue);
//					}
//					catch
//					{
//						list.Add(new DoorInvalidance(model, string.Format("�������� {0} �� ������, ���� ������ ����������, ��������� ���������", __systemDepth)));
//						return list;
//					}

					foreach (clsLeaf leaf in model.Leafs)
					{
						// 20-12-2017
						if (model.ConstructionType.Name == _wcdoor || model.ConstructionType.Name == _indoor || model.ConstructionType.Name == _outdoor)
						{
							if (leaf.OpenType != OpenType.����������)
								leaf.OpenType = OpenType.����������;
						}

						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up == null)
						{
							list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ������, ��������� ���������", __mechanizm)));
							continue;
						}

						string mechanizm = up.StringValue;

						/// todo ������ ����������� ������ ������� --������
						/// ��� ���� ������� � ������� (��������), ������ ������ ������������ � ������ �������� ��� �� ����� ������ '��������' � �������� �� �����
						/// ��������������� �� ���� �� �������, ��� ��-�� ����������� ������������� ������� ��� ���� _�������_ �������
						/// ������� ���� ����� ������� ���� ����� ������� �������� �� ����� ����� ������������, �������� ������ � ���

						// ���� �� ���� ������� todo � ��� ��� ��� ��� �������� ��� / �������, ���� ��������� ??? �����, �� �� ��� ��� ���!!! �������� �� .... 
						// todo ��� ������� ����� !!! , ���� ����������� ����� ����������� �������
						switch (model.ProfileSystem.Name)
						{
							// ���
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

								// ���� �� ���� ����������� 
								// ����� �� ���������, ��� � ������ �������
								switch (model.ConstructionType.Name)
								{
									case _outdoor:
										switch (mechanizm)
										{
											case __office1:
											case __office1b:
												// ������� >= 35 
												if (leaf.Dornmas < 35)
													if (RunCalc.getRestriction(model)) // �������� ��������, 11.02.2016 10:47:39: �361384 - ����� �����������
														list.Add(new DoorInvalidance(leaf, string.Format("{0} - ��������� ������� �� ����� �������� ������� Z98 / T118", mechanizm)));

												// todo ������ ��������

												break;
											case __keykey:
											case __keybar:
											case __no:

												// todo ������ ��������

												break;

											default:
												list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ���������� � {1}, �����������: {2}, {3}, {4}, {5}",
													mechanizm, model.ConstructionType.Name, __office1, __office1b, __keykey, __keybar)));
												break;
										}

										break;

									case _indoor:

										// �������� ������ � 60 �������� � ������ � Dornmas = 25 , ����.... 
										// �������� ������������ setInvariant ������� ������
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

													// todo ������ ��������

													break;

												default:
													list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ���������� � {4}, �����������: {1}, {2}, {3}",
														mechanizm, __keykey, __keybar, __wc, _indoor)));
													break;
											}
										}
										else
										{
											OrderClass order = (OrderClass) model.WinDraw.DocClass ?? (OrderClass) model.dr_model.Table.DataSet.ExtendedProperties["DocClass"];
											bool reclamacia = order != null && order.DocRow != null && !order.DocRow.IsiddocoperNull() && order.DocRow.iddocoper == 59;
											//if (!reclamacia && getRestriction(model)) 
											list.Add(new DoorInvalidance(leaf, string.Format("{0} �������� ��������� ������ � ������� {1}�� �� ������� Z74 / T94", _indoor, __system60)));
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
												// todo ������ ��������

												break;

											default:
												if (getRestriction(model))
													list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ���������� � {4}, �����������: {1}, {2}, {3}",
														mechanizm, __wc, __keybar, __keykey, model.ConstructionType.Name)));
												break;
										}

										break;

									default:
										list.Add(new DoorInvalidance(model, string.Format("{0} ������������ ������ � ������������: {1} � {2}", model.FurnitureSystem.Name, _outdoor, _indoor)));
										break;
								}

								break;

							// �������
							case ALUTECH62:
							case ALUTECH_48:

								// todo �� ������� ��� ����������� ?? ������� ��� ���� ��� ����� ����������� � ������ �����
								switch (model.ConstructionType.Name)
								{
									case _indoor:
									case _outdoor:

										// ������� ��������
										switch (mechanizm)
										{
											case __keykey:
											case __keybar:
											case __office1:
											case __office1b:
											case __keykey1:
											case __keybar1:
											case __no:
												// todo ������ ��������
												break;
											default:
												list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ���������� � {6} / {7}, �����������: {1}, {2}, {3}, {4}, {5}",
													mechanizm, __office1, __keykey1, __keybar1, __keykey, __keybar, model.ProfileSystem.Name, model.FurnitureSystem.Name)));
												break;
										}

										break;

									case _swingdoor:
										// ������� ��������
										switch (mechanizm)
										{
											case __office1:
											case __office1b:
											case __keykey1:
											case __keybar1:
											case __no:
												// todo ������ ��������
												break;
											default:
												list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ���������� � {4} / {5}, �����������: {1}, {2}, {3}",
													mechanizm, __office1, __keykey1, __keybar1, model.ProfileSystem.Name, model.FurnitureSystem.Name)));
												break;
										}

										break;

									default:
										list.Add(new DoorInvalidance(model, string.Format("{0} ������������ ������ � ������������: {1}, {2} � {3}", model.FurnitureSystem.Name, _outdoor, _indoor, _swingdoor)));
										break;
								}

								break;

							default:
								list.Add(new DoorInvalidance(model, string.Format("{0} �� ������������ � ���������� ������� {1}", model.FurnitureSystem.Name, model.ProfileSystem.Name)));
								return list;
						}
					}
				}

					// todo ���� ��� ���������� � ����� - ������ ������� ��������� ���������� �� �������
				else if (model.ConstructionType.Name == _window || model.ConstructionType.Name == _balconGlazing)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							switch (leaf.HandleType)
							{
								case HandleType.�������:
									// ���� ���� ����� ��� ����������, ������ ���� ����������
									//if(up.StringValue2 == __whitebrown)
									//	list.Add(new DoorInvalidance(leaf, string.Format("{0} ����� ������ ������ {1} ��� {2}", HandleType.�������, __white, __brown)));
									break;
								case HandleType.���_�����:
									break;
								default:
									list.Add(new DoorInvalidance(leaf, string.Format("������ {0} ����� ��� {1}", HandleType.�������, HandleType.���_�����)));
									break;
							}
						}
						else
						{
							list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ������, ��������� ���������", __mechanizm)));
							continue;
						}
					}
				}
					// ~todo � ��������� ������ - ������� ��� ��������� ��������� ��� DM=25
				else if (model.ConstructionType.Name == _balcon)
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							switch (leaf.HandleType)
							{
								case HandleType.�������:
									// ���� ���� ����� ��� ����������, ������ ���� ����������
									//if (up.StringValue2 == __whitebrown)
									//	list.Add(new DoorInvalidance(leaf, string.Format("{0} ����� ������ ������ {1} ��� {2}", HandleType.�������, __white, __brown)));

									break;
								case HandleType.���_�����:
									// ok
									break;
								case HandleType.��������_��������:
									if (up.StringValue == __balcon || up.StringValue == __balconk || up.StringValue == __balconb)
									{
										if (leaf.Dornmas >= 25 && (model.FurnitureSystem.Name == RotoNT || model.FurnitureSystem.Name == RotoNTDesigno || model.FurnitureSystem.Name == SiegeniaTitan || model.FurnitureSystem.Name == SiegeniaAxxent))
										{
											//ok
										}
										else
										{
											if (getRestriction(model))
												list.Add(new DoorInvalidance(leaf, string.Format("{0} ����� ���������� ������ �� ������� Z74,T94 ��� ������������� ����������� ������ {1},{2},{3}", __balcon, RotoNT, RotoNTDesigno, SiegeniaTitan)));
										}
									}
									else
									{
										list.Add(new DoorInvalidance(leaf, string.Format("{0} ����� ������������� ������ {1}, {2} ��� {3}", _balcon, __balcon, HandleType.�������, HandleType.���_�����)));
									}

									break;
								default:
									list.Add(new DoorInvalidance(leaf, string.Format("{0} ����� �� �������� �� {1}", leaf.HandleType, _balcon)));
									break;
							}
						}
						else
						{
							list.Add(new DoorInvalidance(leaf, string.Format("�������� {0} �� ������, ��������� ���������", __mechanizm)));
							continue;
						}
					}
				}


				/// ���� �����
				/// ht == ������� && linea ��� ����� �/�
				/// ht == ������� && !linea ������ �,�
				/// ������ �, �, �/�
				foreach (clsLeaf leaf in model.Leafs)
				{
					if (leaf.HandleType == HandleType.���_�����)
					{
					
					}
					else if (leaf.HandleType == HandleType.�������)
					{
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							// ��� ��� �� ��� �����������
							if (up.StringValue2 == __whitebrown)
							{
								list.Add(new DoorInvalidance(leaf, string.Format("���� ������� ����� �� ����� ���� {0}", up.StringValue2)));
								continue;
							}

							if (isPvh(model))
							{
								/// TODO refactor: ������������� ���� ��� ���������� ������ � ���������� ����� ���������� �������� ������� ������������ ��� ��������� �����������
								/// ����� ��� ����������
								// �����������
								// � ������
								// Rehau LINEA Design � ������

								/// �������, ������, ������, ����������, ����� ++ ��������� ����� ���� ��������� ���� �� ������������
								// Rehau LINEA Design
								// Hoppe Austin
								// Hoppe SecuForte Hamburg
								// Hoppe Stuttgart

								/// �������
								// Hoppe Athinai Secustik

								/// ������
								// Hoppe Singapore
								clsUserParam up2 = leaf.UserParameters.GetParam("���������� ������� �����");
								if (up2 != null)
								{
									switch (up2.StringValue)
									{
										case "�����������":
										case "� ������":
										case "Rehau LINEA Design � ������":
											if (up.StringValue2 != __white && up.StringValue2 != __brown)
												list.Add(new DoorInvalidance(leaf, string.Format("���� ������� ����� ���������� {0} �� ����� ���� {1} ����������� {2} ��� {3} ��� ���� ����������", up2.StringValue, up.StringValue2, __white, __brown)));
											break;

										case "Hoppe Athinai Secustik":
											if (up.StringValue2 != __silver)
												list.Add(new SimpleInvariant(model, string.Format("���� ������� ����� ���������� {0} ������ {1}", up2.StringValue, __silver), delegate { up.StringValue2 = __silver; }));
											break;

										case "Hoppe Singapore":
											if (up.StringValue2 != __bronze)
												list.Add(new SimpleInvariant(model, string.Format("���� ������� ����� ���������� {0} ������ {1}", up2.StringValue, __bronze), delegate { up.StringValue2 = __bronze; }));
											break;
										// ����� ��������� � ����� ���������� � ���� ���� ���� �� ����� ���� ������ ��������...
										case "Rehau LINEA Design":
										case "Hoppe Austin":
										case "Hoppe SecuForte Hamburg":
										case "Hoppe Stuttgart":
											if (up.StringValue2 == __darksilver)
												list.Add(new DoorInvalidance(leaf, string.Format("���� ������� ����� ���������� {0} �� ����� ���� {1} ����������� ���� ����������", up2.StringValue, up.StringValue2)));
											break;
										//DECEUNINCK ������ 23/3/2020 ��� ����� ��������� ������ ��� ������� �����...
										case "DECEUNICK":
											if (getiddocoper(model) != iddocoperLerua)
											 list.Add(new SimpleInvariant(model, string.Format("����� {0} ���������� ��� ����� ���� �������", up2.StringValue), delegate { up2.StringValue = "�����������"; }));
											else if (up.StringValue2 == __bronze || up.StringValue2 == __gold)
												list.Add(new DoorInvalidance(leaf, string.Format("���� ������� ����� ���������� {0} �� ����� ���� {1} ����������� ������ ����", up2.StringValue, up.StringValue2)));
											break;										
										default:
											// �� ��������� = ��� ��������
											break;
									}
								}
							}
							else
							{
								if (up.StringValue2 != __white && up.StringValue2 != __brown && up.StringValue2 != __silver)
								{
									list.Add(new DoorInvalidance(leaf, string.Format("���� ������� ����� �� ����� ���� {0}", up.StringValue2)));
								}
							}
						}
					}
					else
					{
						const string ���������������������� = "���������� ������� �����";
						clsUserParam up = leaf.UserParameters.GetParam(__mechanizm);
						if (up != null)
						{
							if (up.StringValue2 != __white && up.StringValue2 != __brown && up.StringValue2 != __whitebrown)
							{
								// list.Add(new DoorInvalidance(leaf, string.Format("���� ����� ����� ���� ������: {0}, {1} ��� {2}", __white, __brown, __whitebrown)));
								if (model.ProfileSystem.Name.StartsWith("ALUTECH") && (up.StringValue == "�������� ������������ ����/����" || up.StringValue == "�������� ������������ ����/�������") && up.StringValue2 == "�������")
								{
									// �����������
								}
									//��� ���� � ������� �������� ������ ������� ������ � "������� ��������"
								else if ( model.ConstructionType.Name == _outdoor & leaf.Dornmas >= 35)
								{
									//����� ���������� ��� ����� - ��������� ����� �� ������ 
									if (leaf.UserParameters.GetParam(����������������������) != null & leaf.UserParameters.GetParam(����������������������).StringValue != leaf.UserParameters.GetParam(����������������������).DefaultValue.StrValue)
									{
										if (up.StringValue2 == __silver || up.StringValue2 == __gold || up.StringValue2 == __bronze)
										{
											// ����������� ��� ����� ������� ������ ������.. 
											
										}
										else //��� �������� ����������� ������� ��� ���� - ����� ����� ������������� ��� program
										{
											list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
										}
									}
									else //���� �� ��� ����� - ����� ����� ��� � ��������� �������, ��� 70 �� �������� �� ���� �����
									{
										if (systemDepth == __system70)
										{
											if (up.StringValue2 == __silver || up.StringValue2 == __bronze)
											{
												// done
											}
											else
											{
												list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
											}
										}
										else if (systemDepth == __system60 & up.StringValue2 == __silver || up.StringValue2 == __bronze || up.StringValue2 == __gold)
										{
											//done
										}
										else //��� ������ �������� �� ��������...
										{

											list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
										}
                                                                               
										
										//list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
									}
                                                                         

								}
								//���������� ��� ������������ ������ � ���������� ����� 35 (����� �������
								else if (model.ConstructionType.Name == _indoor & leaf.Dornmas < 35)
								{
//									if (Settings.idpeople == 255)
//										MessageBox.Show (leaf.Dornmas.ToString());	
									//�������� ��� ����� ����� �� ���� ������ ����������, �� ������� ����� � �������, ������� �� �� ������� � �� �������������. �� ������� ����������� �� �����������, ��� ��� ��� ���������� ��������� ������ � ������������ ����� : �������, ������ ������
									if (up.StringValue2 == __silver || up.StringValue2 == __bronze || up.StringValue2 == __gold)
									{
										//done
									}
									else
									{
										list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
									}
								}
								else
								{
									list.Add(new DoorInvalidance(leaf, string.Format("���� ����� {0}, �� ��������", up.StringValue2)));
								}
							}
						}
					}

				}

				/// �������� �� ����� ��� �� _outdoor
				if (model.ConstructionType.Name == _indoor)
				{
					clsUserParam up = model.UserParameters.GetParam("�������� �� �����");
					if (up != null)
					{
						switch (up.StringValue)
						{
							case "����������":
							case "�������":
							case "������ �������":
							case "��� ��������":
							case "�����":
								break;
							case "������ �������":
							case "������ ���������":
							default:
								list.Add(new DoorInvalidance(model, "���� ���. �������� ����� ���� ���� ��: �����, ����������, �������, ������, ��� ��������"));
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
						if (openView != leaf.OpenView) // �� ��������� ��� !!!
							if (getRestriction(model))
								return new OppositeOpenView(model, "������� ����������� � ������ ����������� ������ / ������");
					}
					else
					{
						openView = leaf.OpenView;
					}
				}

				return null;
			}
		}

		// todo ��� �������� ���� �������� ����� ������� � �������� �����
		private class AlutechHeterogenLeafInvariant : SimpleInvariant
		{
			public AlutechHeterogenLeafInvariant(clsModel model, string message, FixDelegate fixDelegate) : base(model, message, fixDelegate)
			{
			}

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>();

				// ���� ������� ����������� � ������ ������� �� �� ��� ��� ������ - ������ ��� �����, �������������� �� �� ������
				OppositeOpenView oppositeOpenView = OppositeOpenView.test(model);
				if (oppositeOpenView != null)
				{
					list.Add(oppositeOpenView);
				}
				else
				{
					// ����� ����������� ����������� �������
					if (model.ConstructionType.Name != _swingdoor)
					{
						foreach (clsImpost impost in model.Imposts)
						{
							// ��� �������������� ����� � ���� �� ������� �������� �� ���. �������, ����� �� ����� ���� �������
							// if (string.IsNullOrEmpty(impost.Profile.Marking)) 
							//    continue;

							foreach (clsLeafBeem b in impost.ConnectedLeafBeem)
							{
								clsLeaf leaf = b.Leaf;
								// ����������� �� �� ����������
								if (leaf == null)
									continue;

								// ��������� ������� �������� ������ �� ��������� ������� ��� ������������� ������� ��� ������� � �������� .A == 0 (NULL)
								bool invert = leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && impost.Profile != null && impost.Profile.A == 0;

								// ������������� �� �������� ����� , ���� �� ��� �� ������� ����������
								if (leaf.HingBeem == null || leaf.HingBeem.Profile == null || leaf.HingBeem.Profile.A == 0)
									throw new ArgumentException("�� ������� �������� ����� ��� �� ������ � ������� ��� �� '��� �������'");

								clsProfile rightProfile = invert ? getConjugateProfile(model, leaf.HingBeem.Profile) : leaf.HingBeem.Profile;

								if (b.Profile != rightProfile)
								{
									clsLeafBeem beemToModify = b;
									list.Add(new SimpleInvariant(model, string.Format("C������ {0} ������ ������� ����� ����� � {1} �� {2}", leaf.Name, beemToModify.Profile, rightProfile), delegate { beemToModify.Profile = rightProfile; }));
								}
							}
						}
					}
				}

				return list;
			}
		}

		// �������� 0205 ���� 0105 / 0204 -> 0104
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

				// ���� ������� ����������� � ������ ������� �� �� ��� ��� ������ - ������ ��� �����, �������������� �� �� ������
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
							// ���� ����� ���� ?
							if (leafBeem.Profile.Comment != null && (leafBeem.Profile.Comment.Contains(T) || leafBeem.Profile.Comment.Contains(Z)))
							{
								clsBeem adjacentBeem = leaf.AdjacentBeem(leafBeem);

								// ������ �� T | Z ���� :) � ������� ���� ���� � ����
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
				// ��� �� �������
				string TZ;
				if (leafBeem.Profile.Comment.Contains(T))
					TZ = T;
				else if (leafBeem.Profile.Comment.Contains(Z))
					TZ = Z;
				else
					return null;

				foreach (clsProfile p in frameBeem.Profile.ProfileSystem.colProfileFrame)
				{
					// ������� .A
					if (frameBeem.Profile.RefA != p.RefA)
						continue;

					// ������� � ���� ����������� ����� � �� ������ � ������ ��������� ��� ����� ���� ����������� => ����� !
					if (p.Comment.Contains(TZ) && frameBeem.Model.ConstructionType._Frame.Contains(p))
						return p;
				}

				// ���
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

					// ������ ����� �� �������� ������� ����� ��� ��������� ������
					if (leaf.HandleType != HandleType.���_����� && leaf.OpenType != OpenType.����������� && leaf.OpenType != OpenType.����������)
					{
						leaf.HandleType = HandleType.���_�����;
						if (upPassive != null)
							upPassive.StringValue = __no;
					}
						// ��� ����������� ������ ���� �� �������
					else if (leaf.HandleType != HandleType.���_����� && leaf.OpenType == OpenType.����������� && leaf.HandleType != HandleType.�������)
					{
						leaf.HandleType = HandleType.���_�����;
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
					// �����������
					bool restriction = RunCalc.getRestriction(model);

					// ������ ���������� ��������
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new GiesseRestriction(model, string.Format("{0} �� �������� � ���������� ������� {1}", name, model.ProfileSystem.Name)));

					// ������ �� ��������
					foreach (clsLeaf leaf in model.Leafs)
					{
						// ���� ������� = roto OK, ���� �� ������ ����� ������ ������� �� roto NT, �� � ������ �������� ��� ��� Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								if (restriction)
								{
									list.Add(new GiesseRestriction(leaf, string.Format("{0} ������ ������������� �������", name)));
									break;
								}
							}
						}

						// ������� � ����
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						switch (leaf.OpenType)
						{
							case OpenType.����������:
								switch (leaf.ShtulpOpenType)
								{
									// �� ����������
									case ShtulpOpenType.NoShtulp:
										if (w_F < 390 || w_F > 1700 || h_F < 600 || h_F > 2500)
											if (restriction)
												list.Add(new GiesseRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
													w_F, h_F, 390, 1700, 600, 2500)));
										break;

									// ���������� ��������
									case ShtulpOpenType.NoShtulpOnLeaf:
										list.Add(new GiesseRestriction(leaf, "�������� ���������� ������� ������ ���� ���������-��������"));
										break;

									// ���������� ���������
									case ShtulpOpenType.ShtulpOnLeaf:
										if (w_F < 300 || w_F > 1200 || h_F < 600 || h_F > 2500)
											if (restriction)
												list.Add(new GiesseRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
													w_F, h_F, 300, 1200, 600, 2500)));

										break;
								}

								break;

							case OpenType.���������_��������:
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf)
									list.Add(new GiesseRestriction(leaf, "��������� ���������� ������� ������ ���� ����������"));

								if (w_F < 390 || w_F > 1700 || h_F < 600 || h_F > 2500)
									if (restriction)
										list.Add(new GiesseRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� ���������-�������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
											w_F, h_F, 390, 1700, 600, 2500)));

								break;

							case OpenType.��������:
							case OpenType.���������:
								if (w_F < 600 || w_F > 2500 || h_F < 390 || h_F > 1700)
									if (restriction)
										list.Add(new GiesseRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� �������� �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
											w_F, h_F, 600, 2500, 390, 1700)));


								break;
							default:
								list.Add(new GiesseRestriction(leaf, string.Format("{0} ��� ���������� {1} �� ������������", name, leaf.OpenType)));
								break;
						}

						/// ��������� ������ �������

						// ����� ����������� ��������� �����, ����������� �� ������ - ����� ������� �������� �������
						if (leaf.HandlePositionType != HandlePositionType.�����������)
							list.Add(new GiesseRestriction(leaf, "�������� ������ ����������� ��������� �����"));

						// ������ ������� ����� � ������ � � userParam // todo check after DOORS here
						if (leaf.HandleType != HandleType.������� && leaf.HandleType != HandleType.���_�����)
							list.Add(new GiesseRestriction(leaf, "�������� ������ ������� �����"));

						/// ������� �� ������, ������ ��� �������� �� ���������,
						/// ������� ����������, �������� ��������� ��� ����� ������� �� �� ������� �� ���� �� ������
						/// ���� ������ ��������� ������� ��������� � ���� ��� ��� ����������� - ��� ����� �� �������
						if (leaf.OpenType == OpenType.����������)
						{
							leaf.HingeCount = h_F > 1200 ? 3 : 2;
						}
						else if ((leaf.OpenType == OpenType.�������� || leaf.OpenType == OpenType.���������))
						{
							leaf.HingeCount = w_F > 1200 ? 3 : 2;
						}
					}

					// ������� ������ ������

					// --WK1
					clsUserParam wk1 = model.UserParameters.GetParam("������������ ���������");
					if (wk1 != null && wk1.StringValue != "��������")
						list.Add(new GiesseRestriction(model, string.Format("{0}, �������� ������ �������� ������������ ���������", RotoOK)));
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
					// �� �������� ����� ���� 4 �����, ������� �� ����� 3
					if (leaf.OpenType != OpenType.��������� && leaf.OpenType != OpenType.�������� && leaf.HingeCount > 3)
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
				return "��������� ����� ������ �������������";
			}

			public static Invalidance test(clsModel model)
			{
				if (getRestriction(model))
				{
					foreach (clsLeaf leaf in model.Leafs)
					{
						if (leaf.IsMoskit != IsMoskit.���)
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

				/// todo ���� ��� ������ ��� ���� ��������� = ��� ��� ������� ����� �� ������� ��� ������� ����������� � 130 ������ ������
				/// ������ �� �����
				if (rehauList.Contains(model.ProfileSystem.Name))
				{
					// �������� ������ �������� �� ������ ���� �������
					if (!isConsistent(model.Frame))
						if (restriction)
							list.Add(new RehauProfileRestriction(model, "���� ������� �� �������� ������� �������"));

					foreach (clsLeaf leaf in model.Leafs)
					{
						if (!isConsistent(leaf))
							if (restriction)
								list.Add(new RehauProfileRestriction(model, string.Format("������� {0} ������� �� �������� ������� �������", leaf.Name)));
					}

					// ��������� ������
					foreach (clsFrame frame in model.Frame)
					{
						if (frame.bType == ComponentType.Porog && frame.ConnectType != ConnectType.��������)
						{
							// C# 2.0 behaviour
							clsFrame frame1 = frame;
							list.Add(new SimpleInvariant(model, null, delegate { frame1.ConnectType = ConnectType.��������; }));
						}
					}

					// ������ 55000 ������ ����� ��������
					if (restriction)
					{
						foreach (clsFrame frame in model.Frame)
						{
							const string marking = "550000";
							if (frame.Profile.Marking == marking)
							{
								list.Add(new RehauProfileRestriction(model, string.Format("������� {0} �������� ������ �� ������������ � �������������", marking)));
							}
						}
					}

					// todo � ������� ������ ��������� ����� � ������������ �������
					List<string> markings = getDistinctMarkings(model);
					if (markings != null && isMultiMassColor(markings))
						list.Add(new RehauProfileRestriction(model, string.Format("������� ������� �� �������� ������ ������ {0}", string.Join(",", markings.ToArray()))));

					// 14-01-2019 ������������� � ������������� ����� ���� // ����, ���� ....
					// 26-09-2018 ������������� � ������������� ��� ��� ���������� 
					// 24-07-2018 ��� �� 550711 ����� SIB-DeSIGN
					//					foreach (clsBeem beem in model.Frame)
					//					{
					//						if (beem.Profile.Marking == "550711")
					//						{
					//							list.Add(new RehauProfileRestriction(model, string.Format("������� {0} ����������", beem.Profile.Marking)));
					//							break;
					//						}
					//					}

					// �� ������ �� �������� ������� ������
					foreach (clsBeem beem in model.Frame)
					{
						if (beem.BalkaType == ModelPart.Porog)
						{
							foreach (clsBeem impost in beem.ConnectedImposts)
							{
								if (impost.BalkaType == ModelPart.Impost)
								{
									list.Add(new RehauProfileRestriction(model, "�� �������� ���������� ������ �� �����"));
									break;
								}
							}
						}
					}

					int? iddocoper = getiddocoper(model);

					if (restriction)
					{
						/// �������: ������� �/� _�������_ � 24��, 
						/// ��� �������� ������� � ���� ���������� ��������� �� ������ � ������ ������ ArmingInvarinat, ������� ���� ������ � ������ ��� ����������  (��� �� �������)
						foreach (clsLeaf leaf in model.Leafs)
						{
							clsUserParam upGlue = leaf.UserParameters["������� ������������"];
							clsUserParam upArm = leaf.UserParameters["_arm"];


							/// ������� ������ �� 0 �����, �������� ������ ������� ������� �� ������ ������� 
							/// � ��������� ������������, ���� ������� ��� ����������� - �� �����
							if (leaf[0].Profile.C <= 60 && leaf.CommonColSteel().Count > 0 && !string.IsNullOrEmpty(leaf.CommonColSteel()[0]))
							{
								/// ����� ��� �������
								string marking;
								bool glue;

								if (getRightArming(leaf, out marking, out glue))
								{
									// �������� �� Vario � _����� ������� ����� ������
									if (leaf.Model.ProfileSystem.Name == Vario || leaf.Model.ProfileSystem.Name == EVO || !leaf.Region.IsSquare())
									{
										glue = true;
									}

									// ������ ���� ���, �����
									// ������� ����������������� �����������
									// ���������� � �������� ���������� �����������, ���������������� ����������� � ������ ����������� �������� ord = 0
									if (getOrder(marking, glue) > getOrder(upArm.StringValue, upGlue.StringValue != upGlue.DefaultValue.StrValue) || !isSteelValidHere(upArm.StringValue, iddocoper))
									{
										// todo delegate ���������� �� ������� ���� ������������� ������ �����������
										if (isDiffer(marking, upArm.StringValue))
										{
											foreach (clsBeem beem in leaf)
											{
												beem.MarkingSteel = marking;
											}
										}

										// ����
										upGlue.StringValue2 = glue ? _yes : _no;
									}
									else
									{
										// ���������� �� ���� ��� ������ ����, ��� ��������� ������
										foreach (clsBeem beem in leaf)
										{
											beem.MarkingSteel = upArm.StringValue;
										}

										// ����
										upGlue.StringValue2 = upGlue.StringValue; // glue ? "��" : "���"; //;
									}
								}
								else
								{
									if (restriction)
										list.Add(new RehauProfileRestriction(leaf, string.Format("�� ������� ����������� ����������� ��� ������� {0}x{1} ��", leaf.Width, leaf.Height)));
								}
							}
								// ����� .C > 60 // ��� ������ ������� ������� ��� �����������  ( L )
							else if (leaf[0].Profile.C > 60 && leaf.Model.ConstructionType.Name != _window && leaf.Model.ConstructionType.Name != _balcon)
							{
								upGlue.StringValue2 = _yes;
							}
								// EVO , Vario ������
							else if (leaf.Model.ProfileSystem.Name == EVO || leaf.Model.ProfileSystem.Name == Vario)
							{
								upGlue.StringValue2 = _yes;
							}
								// ������
							else
							{
								upGlue.StringValue2 = upGlue.DefaultValue.StrValue;
								upArm.StringValue = string.Empty;
							}
						}
					}

					/// ������������� ������ �� EURO-DESIGN �� ��������� ������ ����������� ����� ������ � ������ �����������, � ������ ������ ��������� - �����������
					/// ���� ���� ������ � ��� ����������� � � ED ��� �������� �� �������� �� ���� ���������� ��� ������ �� ���������� ���� ��� �� �������� ����� ����������� ��� (1.1)
					/// ���� ������� �� ������ �����������, ����� ���. � ������!
					{
						foreach (clsBeem beem in beemEnumerable(model))
						{
							if (!isSteelValidHere(beem, iddocoper)) //(isThinArm(beem))
							{
								clsBeem beem1 = beem;

								string steel = getNextSteel(beem);

								if (string.IsNullOrEmpty(steel))
								{
									list.Add(new RehauProfileRestriction(model, string.Format("����������� {0} �� �������� ��� ������� ���� ������", beem.MarkingSteel)));
								}
								else
								{
									list.Add(new SimpleInvariant(model, string.Format("����������� {0} -> {1} ", beem.MarkingSteel, steel), delegate { beem1.MarkingSteel = steel; }));
								}
							}
						}
					}

					// �������� ��������, 27.06.2017 15:03:20: ����� ������ ������� ����������� ������� ���������� ���� � ��������� ����� � ����������� �������
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
											list.Add(new RehauProfileRestriction(model, string.Format("���������� ������������� ������� ��������� � ������� {0}", beem.Profile.Marking)));
									}
								}
							}
						}
					}


					// Errata �� ����������� � ������������
					foreach (clsBeem connector in model.Connectors)
					{
						if (!connector.Profile.Steels.Contains(connector.MarkingSteel) && connector.Profile.Steels.Count > 0)
						{
							string steel = connector.Profile.Steels[0];
							clsBeem connector1 = connector;
							// WdLog.Add(connector.Profile.Marking, connector.MarkingSteel, steel);
							list.Add(new SimpleInvariant(model, string.Format("{0} ����������� {1} -> {2} ", connector.Name, connector.MarkingSteel, steel), delegate { connector1.MarkingSteel = steel; }));
						}
					}


					// ����� ��� ������, ��� ������
					// �������� ��������, 17.10.2018 17:26:09:
					//    � �496688 - -����� ���� �� ������� ����� � ��������������� ���������, 
					//    � ����� ��� �� ���������, ��� ������� ������� �� ����� ���� �� 
					//    � ������������� � ������ ���� ��
					// ������������� ������� ��� �� ���� ������ __�� �������__ ���������� � ������������� ������ ���� � ���� �����������
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
									list.Add(new RehauProfileRestriction(leaf, "���������� � ������������� ����������� ������������ ������������� ������� �������"));
							}
						}
					}

					// ������ ���������, 17.10.2018 17:20:57: �485308 - � ����� ������� ����� ������ �������. ����� 
					// ��������� ����������� � ��������� ����: � ����� ������� ������ ��������� ���������� ������� ��� ����������� �� �������
					if (restriction && model.Leafs.Count > 1)
					{
						clsProfile profile = null;
						foreach (clsLeaf leaf in model.Leafs)
						{
							if (profile == null)
								profile = leaf.HingBeem.Profile;
							else if (profile != leaf.HingBeem.Profile)
								list.Add(new RehauProfileRestriction(leaf, "������� �� ��������� �������� � ����� ����"));
						}
					}

					// todo ����� ������ � �������� ������ min < 30� ���� ���������� ������� � ������������ ������
					//                    foreach (clsLeaf leaf in model.Leafs)
					//                    {
					//                        clsLeafBeem hingBeem = leaf.HingBeem;
					//                        if (hingBeem != null)
					//                        {
					//                        }
					//                    }

					// 20-08-2019 // 541150 ������ ������ �� ����� ���������� �����������, ����� ������ ��� ������� ��� �� ������ �����������
					foreach (clsBeem impost in model.Imposts)
					{
						const string marking = "541150 (������)";
						const string steel = "251886";
						if (impost.Profile.Marking == marking && impost.MarkingSteel != steel)
						{
							clsBeem impost1 = impost;
							list.Add(new SimpleInvariant(model, string.Format("������ {0} ���������� ����������� {1}", marking, steel), delegate { impost1.MarkingSteel = steel; }));
						}
					}
				}

				list.AddRange(InactiveProfileRestriction.test(model));

				return list;
			}

			// ��������� ��� ����������� �� ������� �) ����������� � ����� �������, �) �� ���� �������� �������� 1,1 - �� ��� ����
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
				// ������� ��������
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

			private const string _yes = "��";
			private const string _no = "���";


			private static readonly string[] armOrder;

			private static int getOrder(string marking, bool glue)
			{
				/// ���� ����������� 244506(1.1)
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

			// ����������� ����������� ���������� ���� ��� ��� ������ �������������� �������, ������ ��� ������ �������� ����������� ����� �������� ���������
			// ���� ��� �������� ���-�� - ��� ��� ������ �����������
			static RehauProfileRestriction()
			{
				// todo 
				DataTable table = Settings.GetSetVar("������� �����������").TableCustomValue;
				//DataTable table = Settings.GetSetVar(313).TableCustomValue;
				group = (new DataView(table)).ToTable(true, new string[] {_openType, _color, _marking, _glue, "ord"}); // ord = ������� �������� ���������
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
				string s = Settings.GetSetVar("������� �����������").strvalue;
				if (string.IsNullOrEmpty(s))
					throw new ArgumentException("������ ���������� ����������� �� ������ � ���������� `������� �����������`");

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
					case OpenType.����������:
					case OpenType.���������_��������:
					case OpenType.�����������:
					case OpenType.����������:
						openType = OpenType.���������_��������;
						break;
					case OpenType.������:
						openType = leaf.Height > leaf.Width ? OpenType.���������_�������� : OpenType.��������;
						break;
					case OpenType.��������:
					case OpenType.���������:
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
						// ���������� �������
						double x = leaf.Width;
						double y = leaf.Height;

						// ��������� � ����
						if (v.y0 < y && y <= v.y1)
						{
							// ���� v^b >= 0 �� ok, ����� ������� ����� (slab)
							double p = v.p(x, y);
							if (p >= 0)
							{
								clsBeem beem = leaf[0];
								// �������� ������� ��������� ���������� � ������ ����� ������� && ������������ � ���� ���� �����
								if (beem.Profile != null && beem.Profile.Steels != null && beem.Profile.Steels.Contains(row[_marking].ToString()) && isSteelValidHere(row[_marking].ToString(), iddocoper))
								{
									marking = row[_marking].ToString();
									glue = Boolean.Parse(row[_glue].ToString());

									/// 03-08-2016 Vario && Decor ����������� ������ 244536 ��������.
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

		/// ���������� ����� �����������, ��� ������� � ����������, ��� ��� ���� ��������, 
		/// ��� ����� ������� �����������, ���� � ������ ������� ��� ����������� ������� ������ �� ���, ������� � ��������� ��� ������� �������, ������� �� ���� ������ ���������� ���� ���
		private void ChangeSteel(clsModel model, wdAction action)
		{
			if (model.SelectedBeems.Count > 0)
			{
				List<clsLeaf> leafs = new List<clsLeaf>();

				// ������� ���������� ������� �� ��������� ������, ��� ������ �������� �����
				foreach (clsBeem beem in model.SelectedBeems)
				{
					clsLeafBeem leafBeem = beem as clsLeafBeem;
					if (leafBeem != null)
					{
						if (!leafs.Contains(leafBeem.Leaf))
							leafs.Add(leafBeem.Leaf);
					}
				}

				// ������ ��������� �����������, ����� �� ������, ��� ��� ����� ���� ������ ���� legacy ����� ������ �����������, � ��� ������ � ���� � ��������
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
				// ��� �� ������������
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
						return new GabaritRestriction(model, @"�������� ����������� ��������� ����������� ����������, ���� �� ��������� �� ������ ��������� 2800, ��������� ������ �� �����");

					clsUserParam up = model.UserParameters.GetParam("��������");
					if (up != null && up.StringValue != up.DefaultValue.StrValue)
					{
						const double max1 = 2500; // 13-02-2018 // 2800
						const double max2 = 2800; // 13-02-2018 // infinity+

						//				        int face = up.StringValue.StartsWith("60") ? 60 : up.StringValue.StartsWith("90") ? 90 : 0;
						//				        bool piForm = up.StringValue2 != "�� ���������";
						//				        int dx = (int)up.NumericValue;

						//				        int width = (int)(model.MaxX + 2 * face);
						//				        int height = (int)(model.MaxY + face + dx);

						int width = (int) (model.MaxX);
						int height = (int) (model.MaxY);

						if (width > max1 && height > max1)
							return new GabaritRestriction(model, @"������� ��������� ��������� ����������� ����������, ���� �� ��������� �� ������ ��������� " + max1);

						if (width > max2 || height > max2)
							return new GabaritRestriction(model, @"������� ��������� ��������� ����������� ����������, �� ���� �� ��������� �� ������ ��������� " + max2);
					}
				}

				return null;
			}
		}

		#region ��������

		// ������ ������������ �� Balka, ��� �������� ����������� ��� �� �� Serializable, �������� ��
		// ��������� ����������� � ����� ������������, ��� ��� object ���� � Construction ������ � ���������� ��� ���� ��������, �������� �� �������� 
		// � ����� �������������� �������� ��� calcVariables = new Dictionary<string, string>() �������� �� � ������ ���
		// ��� ��� ���� GlobalParamList : List<GlobalParam> =  { string _Name; object _Value; , ������ �� ���-�� ����� ����������� element.SetAttribute("Value", this._Value.ToString()); 
		// � ��� ��� ����� ����������� ���������
		[Serializable]
		public class B
		{
			// ������ ����� � ������� ��� ������
			public int Lenght;

			// ���� � ������� �� �������� � Balka as decimal
			public decimal Ang1;
			public decimal Ang2;
			public decimal Radius1;
			public decimal Radius2;

			/// ��� ����������
			public SoedType Connect1;
			public SoedType Connect2;

			/// �������
			public ItemSide Side;
		}

		private class NalichnikRestriction : Invalidance
		{
			// ��� ���������
			private const string �������� = "��������";

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
				clsUserParam up = model.UserParameters.GetParam(��������);

				// ������ ���� ����
				if (up != null && up.StringValue != "���")
				{
					// todo ���� �� ��������� ?? ������ �������
					// == ����� > 0
					if (up.NumericValue < 0)
						up.NumericValue = 0;

					// todo ���� � �� ����� ������ 1 ������ �����
					bool piForm = up.StringValue2 != "�� ���������";
					if (piForm)
					{
						clsBeem bottom = null;
						foreach (clsBeem beem in model.Frame)
						{
							if (beem.PositionBeem == ItemSide.Bottom)
							{
								if (bottom != null)
								{
									// ���� ��� ���� �� ���������� ��� ��� ����� ������ ���� 
									bottom = null;
									break;
								}

								bottom = beem;
							}
						}

						if (bottom == null)
							return new NalichnikRestriction(model, "�-�������� ��������: ���������� ������� ������ ����� ������ �������������� �����");

						if (model.Frame.Count > 2)
						{
							if (bottom.Beem2.PositionBeem != ItemSide.Left || bottom.Beem2.R1 > 0.05)
								return new NalichnikRestriction(model, "�-�������� ��������: ����� ����� �� ������������");

							if (bottom.Beem1.PositionBeem != ItemSide.Right || bottom.Beem1.R1 > 0.05)
								return new NalichnikRestriction(model, "�-�������� ��������: ������ ����� �� ������������");
						}
					}

					bool restriction = getRestriction(model);

					foreach (clsBeem beem in model.Frame)
					{
						// todo ���� ��� ������������� ����
						if (beem.R1 > 0 && beem.R2 > 0)
							if (restriction)
								return new NalichnikRestriction(model, "������������� ������� �������� �� ����������, ����������� ������� ���� R2=0");

						if (beem.R1 < 0)
							return new NalichnikRestriction(model, "������� �������� ������� �������� �� ����������");

						if (beem.PositionBeem == ItemSide.Bottom && up.NumericValue > 0 && model.Frame.Count > 2 && (beem.Beem2.PositionBeem != ItemSide.Left || beem.Beem1.PositionBeem != ItemSide.Right))
							return new NalichnikRestriction(model, "�� �� ������������� ��������� ����� �� ����������");
					}

					/// ���� ������ �������� ��� �������� � ��� ����������� ��� �� �������� �� ����� ���������� � ������������ � ��������� ��� ����������� = ��������
					/// ��������
					/// �������� = 0
					/// ���������� 1 �� � ��� ���������� 
					/// ��� ���� .A = 0
					if (model.ConstructionType.Name != _nalichnik && model.Leafs.Count == 0 && model.VisibleRegions.Count == 1 && (model.VisibleRegions[0].Fill.FillType == FillType.Unknown || model.VisibleRegions[0].Fill.FillType == FillType.NotFill) && model.Frame.TrueForAll(delegate(clsBeem beem) { return beem.Profile.A < 1; }))
					{
						return new SimpleInvariant(model, "�� ���� ��������� ��� ��������, ������ ��� ����������� �����������", delegate
							{
								model.ProfileSystem = model.WinDraw.ProfileSystems.FromName(ProfSystBEZ);
								model.ConstructionType = model.ProfileSystem.ConstructionTypes.Find(_nalichnik, model.WinDraw.ProfileSystems.FromName(ProfSystBEZ));
							});
					}

					if (model.ConstructionType.Name == _nalichnik)
					{
						// ���������� ������� � �������� ����������� ���������� ��������
						// ���������� ������� ������ ������
						foreach (clsRegion region in model.VisibleRegions)
						{
							if (region.Fill.FillType != FillType.Unknown && region.Fill.FillType != FillType.NotFill)
							{
								clsRegion region1 = region;
								return new SimpleInvariant(model, "�������� ������ ��� ����������", delegate { region1.Fill = region1.Model.ProfileSystem.GetFillByMarking("���_������_�_�������"); });
							}
						}
					}
				}

				if (model.ConstructionType.Name == _nalichnik && up != null && up.StringValue == up.DefaultValue.StrValue)
				{
					return new NalichnikRestriction(model, "��������� � ���������������� ���������� ������ ��������� �� ������");
				}


				return null;
			}

			private static int getFace(clsUserParam up)
			{
				switch (up.StringValue)
				{
					case "60��":
						return 60;
					case "90��":
						return 90;
					default:
						return 0;
				}
			}

			// ���������� �������� ���� ����
			internal static void draw(clsModel model)
			{
				clsUserParam up = model.UserParameters.GetParam(��������);
				if (up != null && up.StringValue != "���")
				{
					bool piForm = up.StringValue2 != "�� ���������";
					int face = getFace(up);

					double @baseX = model.Frame.MinX;
					double @baseY = model.Frame.MinY;

					List<clsLine> offsets = new List<clsLine>();

					// ��������� �������
					foreach (clsBeem beem in model.Frame)
					{
						double overhang = beem.PositionBeem == ItemSide.Bottom ? up.NumericValue : 0;
						clsLine offset = beem.LGabarit.Clone.CreateOffset(piForm & beem.PositionBeem == ItemSide.Bottom ? overhang : face + overhang);
						offsets.Add(offset);
					}

					// �����������
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


					// ���������
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
							// bug ���� �� �-����� + �����, ������������ ���� �� ���� ��������, � ���� �� ��������� ����� .A ���������, ��� ������ �� ���� = �����
							connect1.Point2 = beem.Point1.Clone;

							offset.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, offset.ToIPrimitive());

							// �� ������������� ���� �� ����� �-����� 
							if (piForm && beem.Beem1.PositionBeem == ItemSide.Bottom)
								continue;
							connect1.Move(@baseX, @baseY);
							model.Canvas.DrawPrimitive(pen, connect1.ToIPrimitive());
						}
					}
				}
			}

			// ���������� clsModel.Nalichnik -> Construction.Nalichnik
			internal static void calc(clsModel model)
			{
				/// ������� ���������� �������� ��� ���� � �������� � Construction.CalcVaraibles
				const string __nalichnik = "��������";
				clsUserParam up = model.UserParameters.GetParam(__nalichnik);

				if (up == null)
					return;

				// �����
				int face = getFace(up);

				// �����
				bool piForm = up.StringValue2 != "�� ���������";

				/// todo calc offset && angles
				List<B> list = new List<B>();
				foreach (clsBeem beem in model.Frame)
				{
					// �� ������ �� ������
					// todo ������� ��� ����� ������ ��� "��� �������" �� � ��� ���� ��� ��� �������
					if (piForm && beem.PositionBeem == ItemSide.Bottom)
						continue;

					B b = new B();

					b.Side = beem.PositionBeem;

					// todo ���� �������� �� 90 � ����� ������� �������� ������ ���� �-�������� ��������
					// ��������� ����� ��� �� ����
					double ang1 = beem.LGabarit.AngelBeetwenLines360(beem.Beem2.LGabarit) / 2;
					double ang2 = beem.Beem1.LGabarit.AngelBeetwenLines360(beem.LGabarit) / 2;

					// bug around here fixing
					// ����� ��� ���� ���������� �����, ���� �-�����
					int dx;

					// ��� ���������� ������ = ������
					b.Connect1 = b.Connect2 = SoedType.Ravnoe;

					// �����, ��� �������
					if (beem.Beem1.PositionBeem == ItemSide.Bottom)
					{
						// ����� 
						dx = (int) up.NumericValue;

						// ���� �-����� �� � �������� � ������ ����� �������
						if (piForm)
						{
							ang2 = 90;
							b.Connect2 = SoedType.Dlinnoe;
						}
					}
						// ������ � ������
					else if (beem.Beem2.PositionBeem == ItemSide.Bottom)
					{
						// �����
						dx = (int) up.NumericValue;

						// ���� �-����� �� � �������� � ������ ����� �������
						if (piForm)
						{
							ang1 = 90;
							b.Connect1 = SoedType.Dlinnoe;
						}
					}
						// ���������
					else if (beem.PositionBeem == ItemSide.Bottom)
					{
						dx = 0; // todo ���� �� ���������� ��� ������ ����� �������� � ������ ��������������� ���������, �� ���� ��������� != ������ �������� �����
					}
						// ������ ����
					else
					{
						dx = 0;
					}

					// todo ������ ��� ����� ����� ��������
					// bug ���� �������� ������� �������� �� ri.LengthDouble = ���������� ������, ��� ���� ������ �������� ����� �� ������� ����� ����������� ri.LGabarit.Length !! @������
					double x1 = (ang1 <= 90 ? face : face + beem.Profile.A) / Math.Tan(ang1 * Math.PI / 180);
					double x2 = (ang2 <= 90 ? face : face + beem.Profile.A) / Math.Tan(ang2 * Math.PI / 180);

					// todo ��������� �� ����� ����������� 
					// todo ����� ������ (��� �����������) �� ������ ����� �� �������� �� 2 ������ x = fa�e * tg a = face / tg b
					int length = (int) Math.Round(beem.LGabarit.Length + x1 + x2 + dx); //  + w

					// ���� �� ���� ������� ������ ������ ��������� ������������ �� �� ����� �
					if (length <= 0)
						continue;

					if (beem.R1 > 0)
					{
						// ������ ������� �����
						double lengthBasisArc = beem.LGabarit.BaseLineArc.Length + 2 * face;
						// ������� ������ ���������
						double r1 = beem.R1 + face;
						length = (int) Math.Round(Math.Asin(lengthBasisArc / (2 * r1)) * 2 * r1);
					}

					b.Lenght = length;
					b.Ang1 = (decimal) ang1;
					b.Ang2 = (decimal) ang2;
					// 01-06-2016 ������ __���������__ �� ������� ����, ��������� ������ ������� ����, ������������ - �����                 // todo ��������� �� �����
					b.Radius1 = (decimal) beem.R1;


					list.Add(b);
				}

				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(B[]));
					MemoryStream mem = new MemoryStream();
					serializer.Serialize(mem, list.ToArray());
					// bug ������ ������ xml ������ xml, ������� �������
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

		#endregion ��������

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

			// ����������� �� ����� ���������
			private static readonly Dictionary<string, SizeRestriction> furnSizeRestrictionDictionary = new Dictionary<string, SizeRestriction>();

			// ������ ���������� ����������
			private static readonly List<string> protalMechanizmList = new List<string>(new string[] {__portalOneSide, __portalOneSideKey, __portalKeyKey, __portalKeyBar});

			static PortalRestriction()
			{
				// ������ ������� �� ������ (���) 670 - 1200,  ������ ������� �� ������ (���) (840) 1000 - 2360 // ��� �� ��� ���� �������� ���� 840 ������� ��� �������
				furnSizeRestrictionDictionary.Add(PSKPORTAL100, new SizeRestriction(670, 840, 1200, 2360));

				// ������ ������� �� ������ (���) 670 - 1600,  ������ ������� �� ������ (���) (840) 1000 - 2360
				furnSizeRestrictionDictionary.Add(PSKPORTAL160, new SizeRestriction(670, 840, 1600, 2360));

				// ������ ������� �� ������ (���) 770 - 2000,  ������ ������� �� ������ (���) 1690* - 2360 // 1690 ��� �� ����������� � ������� ������� ������� �������
				furnSizeRestrictionDictionary.Add(PSKPORTAL200, new SizeRestriction(770, 1690, 2000, 2360));

				// ������ ������� �� ������ (���) 670 - 1600,  ������ ������� �� ������ (���) (840) 1000 - 2360 (2800) // ��� 2800 ����������� ���������� ������
				furnSizeRestrictionDictionary.Add(PSK_160_COMFORT, new SizeRestriction(670, 840, 2000, 2360));

				// ������ ������� �� ������ 
				furnSizeRestrictionDictionary.Add(PSK_Vorne, new SizeRestriction(600, 700, 1600, 2200));
			}

			// todo ������� ���-�� ��������� �� ������-�� ������� ������ �� clsFurnitureSystem
			private static readonly List<string> furnSystemList = new List<string>(new string[] {PSKPORTAL100, PSKPORTAL160, PSKPORTAL200, FurnSystBEZ, PSK_160_COMFORT, PSK_Vorne});

			public static IEnumerable<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// �������� ������� ������������� ���������� ���������: �� ������������ � ��� �� ����
				if (furnSystemList.Contains(model.FurnitureSystem.Name) && model.FurnitureSystem.Name != FurnSystBEZ && model.ConstructionType.Name != _pskportal)
				{
					list.Add(new PortalRestriction(model, string.Format("{0} ����������� ������ �� ���� ����������� {1}", model.FurnitureSystem.Name, _pskportal)));
				}

				// ���� ��� �� ������ �� ��� �����
				if (model.ConstructionType.Name != _pskportal)
					return list;

				// ������ ������� ������������� ���������� ���������: � �������� ������������ ������ ���������� ���������
				if (!furnSystemList.Contains(model.FurnitureSystem.Name))
				{
					// ������ �� ���� �.�. ��� ������� ���� ������� ������������/����������� ����������� �������
					list.Add(new PortalRestriction(model, string.Format("{0} ��������� ������ ��������� PSK", model.ConstructionType.Name)));
					return list;
				}

				// ������ ����������� ��� ���������
				bool restriction = getRestriction(model);

				// ����������� ������� 
				foreach (clsLeaf leaf in model.Leafs)
				{
					// ���� �� ���������� ������ ������ ��� ����������
					if (leaf.OpenType != OpenType.����������)
					{
						clsLeaf leaf1 = leaf;
						list.Add(new SimpleInvariant(model, "������ ���������� ����������", delegate { leaf1.OpenType = OpenType.����������; }));
					}

					// ������ ������������� �������
					if (!isRectangleLeaf(leaf))
						list.Add(new PortalRestriction(leaf, "������ ������������� �������"));

					// �������� ���������� ����������� // ������� Z
					clsProfile profile = leaf[0].Profile;
					if (!profile.Comment.Contains("Z"))
					{
						list.Add(new PortalRestriction(leaf, "�������� ������ Z �������"));
						break;
					}

					// .C ������� - �� ���� ������ ����������� ���������
					int profileC = (int) leaf[0].Profile.C;

					// ��������
					clsUserParam upMechanizm = leaf.UserParameters.GetParam(__mechanizm);

					// ����������� �� .C ������� �������
					if (model.FurnitureSystem.Name == PSKPORTAL200)
					{
						if (98 <= profileC)
						{
							// nop
						}
						else if (74 <= profileC && profileC < 98)
						{
							list.Add(new SimpleInvariant(model, string.Format("��� ������� Z{0} ���������� ��������� {1}, ����������� �� {2}",
								profileC, model.FurnitureSystem.Name, PSKPORTAL160), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(PSKPORTAL160); }));
							/// ���������� �������� ����������� �� ������� ���� ����� ��������� SimpleInvariant
							break;
						}
						else if (profileC < 74)
						{
							list.Add(new SimpleInvariant(model, string.Format("��� ������� Z{0} ���������� ��������� {1}, ����������� �� {2}",
								profileC, model.FurnitureSystem.Name, PSKPORTAL100), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(PSKPORTAL100); }));
							/// ���������� �������� ����������� �� ������� ���� ����� ��������� SimpleInvariant
							break;
						}
						else
						{
							list.Add(new PortalRestriction(leaf, string.Format("�� ���������� ������ ������� .�={0}", profileC)));
						}
					}
					else if (model.FurnitureSystem.Name == PSKPORTAL160 && upMechanizm != null && upMechanizm.StringValue != __portalOneSide)
					{
						if (profileC < 74)
						{
							list.Add(new PortalRestriction(model, string.Format("��� {0} � ������� Z{1} ���������� ����� {2} ����������� ����� ������� ������� Z74, Z98 ��� ������������� �����",
								model.FurnitureSystem.Name, profileC, upMechanizm.StringValue)));
						}
					}

					// ���, ���
					int h_F = leaf.BaseRegion.Lines[0].HeightInt;
					int w_F = leaf.BaseRegion.Lines[0].WidthInt;


					// 
					// ����������� �����������
					if (model.FurnitureSystem.Name != FurnSystBEZ)
					{
						// ���������� �� ������ �������� ����� ��������� ���
						List<string> availableFurniture = new List<string>();
						foreach (KeyValuePair<string, SizeRestriction> pair in furnSizeRestrictionDictionary)
						{
							SizeRestriction sizeRestriction = pair.Value;

							if (sizeRestriction.minWidth <= w_F && w_F <= sizeRestriction.maxWidth && sizeRestriction.minHeight <= h_F && h_F <= sizeRestriction.maxHeight)
							{
								availableFurniture.Add(pair.Key);
							}
						}

						// ���� ������� ��������� ��� ����� ���������� �� �������� ��������
						if (!availableFurniture.Contains(model.FurnitureSystem.Name))
						{
							if (availableFurniture.Count == 1)
							{
								list.Add(new SimpleInvariant(model, string.Format("��� ������ �������� ������� {0}x{1} ���x��� �������� ��������� ���������:{2}", w_F, h_F, string.Join(",", availableFurniture.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(availableFurniture[0]); }));
								/// ���������� �������� ���� ������� ����������� �� ������� ���� ����� ��������� SimpleInvariant
								break;
							}
							else if (availableFurniture.Count > 1)
							{
								list.Add(new PortalRestriction(leaf, string.Format("��� ������ �������� ������� {0}x{1} ���x��� �������� ��������� ���������:{2}", w_F, h_F, string.Join(",", availableFurniture.ToArray()))));
							}
							else
							{
								if (restriction)
									list.Add(new PortalRestriction(leaf, "�� ������� ���������� ��������� ��� ������ �������� ������� � ���� �������"));
							}
						}

						// TODO ���� ����� ���� ���� �� �������� ������� ������� ���-�� ����������� 
						// �������� ������ ����� ��������� ������ ���� �� ���� ��� �� ������� furnitureInvarian, ��� � �������� ������ �� ��������� ���� �����������
						// if (!isFurnitureInvariantReached(list))
						{
							if (upMechanizm != null)
							{
								// ������ ����� �� PSK100 = �� �������� �������������
								if (leaf.Model.FurnitureSystem.Name == PSKPORTAL100)
								{
									if (upMechanizm.StringValue != __portalOneSide)
									{
										upMechanizm.StringValue = __portalOneSide;
									}
								}

								// �� 100/160 ������ �������� ��� = ���������, � ��� 200 ������ ���� ���� �����
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.ShtulpLeaf != null)
								{
									if (leaf.Model.FurnitureSystem.Name == PSKPORTAL100
										|| leaf.Model.FurnitureSystem.Name == PSKPORTAL160
										|| (leaf.Model.FurnitureSystem.Name == PSKPORTAL200 && leaf.HandleType != HandleType.���_�����))
									{
										clsLeaf active = leaf.ShtulpLeaf;
										clsUserParam upActiveMechanizm = active.UserParameters.GetParam(__mechanizm);
										upMechanizm.StringValue = upActiveMechanizm.StringValue;
										upMechanizm.StringValue2 = upActiveMechanizm.StringValue2;
									}
								}

								if (leaf.HandleType != HandleType.���_�����)
								{
									switch (upMechanizm.StringValue)
									{
										case __portalOneSide:
											break;
										case __portalOneSideKey:
											if (model.FurnitureSystem.Name != PSK_160_COMFORT)
											{
												list.Add(new PortalRestriction(leaf, string.Format("{0} ����� ���������� ������ � ��������� {1}", upMechanizm.StringValue, PSK_160_COMFORT)));
											}

											break;

										case __portalKeyKey:
										case __portalKeyBar:
											// changed
											// if (model.FurnitureSystem.Name == PSK_160_COMFORT && Settings.idpeople != 255)
											//    list.Add(new PortalRestriction(leaf, string.Format("{0} ������ ���������� � ��������� {1}", upMechanizm.StringValue, model.FurnitureSystem.Name)));
											// ����� ��������� ������ �� DM=>25 �.�. �� Z74
											if (profileC < 74)
												list.Add(new PortalRestriction(leaf, string.Format("{0} ����� ���������� ������ �� ������� Z74 � Z98", upMechanizm.StringValue)));
											// ����� ������ �� ��� >= 1000
											if (h_F < 1000)
												list.Add(new PortalRestriction(leaf, string.Format("{0} ����� ���������� ������ �� ������� ��� >= 1000��", upMechanizm.StringValue)));
											break;

										default:
											list.Add(new PortalRestriction(leaf, string.Format("{0} ��������� ������ ���������: {1}", model.ConstructionType.Name, string.Join(", ", protalMechanizmList.ToArray()))));
											break;
									}
								}
							}

							// ��������� ������� ��� ����� �������� ������ � ��� PSK 200 c ������������� ������ 200/GH �����
							if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.HandleType == HandleType.���_�����)
							{
								bool ok = model.FurnitureSystem.Name == PSKPORTAL200;

								if (ok)
								{
									clsLeaf activeLeaf = leaf.ShtulpLeaf;
									// todo XXX ������ �� ������ �����, ����� ��� ������ ���
									if (activeLeaf != null)
									{
										clsUserParam upMechanizmActive = activeLeaf.UserParameters.GetParam(__mechanizm);
										ok = upMechanizmActive != null && upMechanizmActive.StringValue == __portalOneSide;
									}
								}

								if (!ok)
								{
									list.Add(new PortalRestriction(leaf, string.Format("��������� ������� ��� ����� �������� ������ � ��� {0} c ������������� ������ PSK200/GH �� �������� �������", PSKPORTAL200)));
								}
							}

							// ���� �����
							if (model.FurnitureSystem.Name == PSK_160_COMFORT)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown && upMechanizm.StringValue2 != __gold)
								{
									list.Add(new PortalRestriction(leaf, string.Format("���� ����� ����� ���� ������: {0}, {1}, {2}, {3} ��� {4}", __white, __brown, __whitebrown, __silver, __gold)));
								}
							}
							else if (model.FurnitureSystem.Name == PSKPORTAL100 || model.FurnitureSystem.Name == PSKPORTAL160)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("���� ����� ����� ���� ������: {0}, {1}, {2} ��� {3}", __white, __brown, __whitebrown, __silver)));
								}
							}
							else if (model.FurnitureSystem.Name == PSKPORTAL200)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __bronze && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("���� ����� ����� ���� ������: {0}, {1}, {2}, {3} ��� {4}", __white, __brown, __whitebrown, __bronze, __silver)));
								}
							}
							else if (model.FurnitureSystem.Name == PSK_Vorne)
							{
								if (upMechanizm.StringValue2 != __white && upMechanizm.StringValue2 != __brown && upMechanizm.StringValue2 != __silver && upMechanizm.StringValue2 != __whitebrown)
								{
									list.Add(new PortalRestriction(leaf, string.Format("���� ����� ����� ���� ������: {0}, {1}, {2} ��� {3}", __white, __brown, __whitebrown, __silver)));
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

		// � ������ �� ��� RunCalc, � ����� �� �������������
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

				// ������ ����������� �������
				if (model.FurnitureSystem.Name == System && model.Leafs.Count > 0)
				{
					// �����������
					bool restriction = RunCalc.getRestriction(model);

					// ������ ���������� ��������
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new SiegeniaClassicRestriction(model, string.Format("�� �������� � ���������� ������� {1}", System, model.ProfileSystem.Name)));

					// ������ �� ��������
					foreach (clsLeaf leaf in model.Leafs)
					{
						// ���� ������� = roto OK, ���� �� ������ ����� ������ ������� �� roto NT, �� � ������ �������� ��� ��� Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (leaf.Count != 4 || beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								list.Add(new SiegeniaClassicRestriction(leaf, "������ ������������� �������"));
								break;
							}
						}

						// TODO ����� �������, �� ������ ������
						// ������ ��������� �� T ������� ���������� ������� DM15 
						if (leaf.OpenView == OpenView.������)
							list.Add(new SiegeniaClassicRestriction(leaf, "�� ��������� �� ������� T"));

						// ���������� ������� ������ ���, ��������� ������ �/�
						switch (leaf.ShtulpOpenType)
						{
							case ShtulpOpenType.ShtulpOnLeaf:
								if (leaf.OpenType != OpenType.����������)
									list.Add(new SiegeniaClassicRestriction(leaf, "��������� ���������� ������� ������ ��������� ����������"));
								break;
							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.OpenType != OpenType.���������_��������)
									list.Add(new SiegeniaClassicRestriction(leaf, "�������� ���������� ������� ������ ���������-�������� ����������"));
								break;
						}

						// ����� ����������� ��������� �����, ����������� �� ������ - ����� ������� �������� �������
						// ������ � ����� ������������ �������� ����, �� ��� �� �����������
						if (leaf.HandlePositionType != HandlePositionType.�����������)
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("�������� ������ {0} ��������� �����", HandlePositionType.�����������)));

						// ������� ���� wd-4
						if (!leaf.HandlePosition.IsAutomat)
						{
							leaf.HandlePosition.IsAutomat = true;
							leaf.HandlePosition.HandlePosition = 0;
						}


						// ����� ������� ����� � ������ � � userParam
						if (leaf.HandleType != HandleType.������� && leaf.HandleType != HandleType.���_�����)
							list.Add(new SiegeniaClassicRestriction(leaf, "�������� ������ ������� �����"));

						clsUserParam ht = leaf.UserParameters.GetParam("��� �����");
						if (ht != null && ht.StringValue != "������� �����")
							list.Add(new SiegeniaClassicRestriction(leaf, "�������� ������ ������� �����"));

						// ����� 2 ������ ������ �� �������� � ����������
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("{0} ������ ����� ���������� ������ �� ���������� � �������� �������", leaf.HingeCount)));

						// --WK1
						clsUserParam wk1 = model.UserParameters.GetParam("������������ ���������");
						if (wk1 != null && wk1.StringValue != "��������")
							list.Add(new SiegeniaClassicRestriction(model, "�������� ������ �������� ������������ ���������"));

						// http://yt:8000/issue/WINDRAW-45
						// ������ ����� ��� ���������� ���� ��������
						// clsUserParam dekas = model.UserParameters.GetParam("�������� �� �����");
						// if (dekas != null && dekas.StringValue != "�����" && dekas.StringValue != "����������" && dekas.StringValue != "��� ��������")
						// 	if (restriction)
						//		list.Add(new SiegeniaClassicRestriction(model, "�������� ������ ����� � ���������� �������� �� �����"));

						// �������
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						if (sizesDictionary.ContainsKey(leaf.OpenType))
						{
							int[] sizes = sizesDictionary[leaf.OpenType];

							if (w_F < sizes[0] || w_F > sizes[1] || h_F < sizes[2] || h_F > sizes[3])
							{
								if (restriction)
									list.Add(new SiegeniaClassicRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� {6} �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
										w_F, h_F, sizes[0], sizes[1], sizes[2], sizes[3], keepRussian(leaf.OpenType))));
							}
							else
							{
								// ���������
								if (leaf.OpenType == OpenType.���������� && w_F > 700 && h_F < 490)
								{
									if (restriction)
										list.Add(new SiegeniaClassicRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, ��� ������ > 700 �� ������ ������ �� ����� ���� ����� 490 �� �� ������", w_F, h_F)));
								}

								if (leaf.OpenType == OpenType.���������� && w_F <= 700 && h_F > 2360)
								{
									if (restriction)
										list.Add(new SiegeniaClassicRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, ��� ������ < 700 �� ������ ������ �� ����� ���� ����� 2360 �� �� ������", w_F, h_F)));
								}
							}
						}
						else
						{
							list.Add(new SiegeniaClassicRestriction(leaf, string.Format("��� ���������� {0} �� ������������", leaf.OpenType)));
						}
					}
				}

				return list;
			}

			private static readonly Dictionary<OpenType, int[]> sizesDictionary = new Dictionary<OpenType, int[]>();

			static SiegeniaClassicRestriction()
			{
				sizesDictionary.Add(OpenType.���������_��������, new int[] {290, 1260, 490, 2400});
				sizesDictionary.Add(OpenType.����������, new int[] {290, 1560, 200, 2400}); // ��� ����������
				sizesDictionary.Add(OpenType.��������, new int[] {200, 2360, 250, 700});
				sizesDictionary.Add(OpenType.���������, new int[] {200, 2360, 250, 700});
			}

			private static string keepRussian(OpenType openType)
			{
				switch (openType)
				{
					case OpenType.����������:
						return "����������";
					case OpenType.���������_��������:
						return "���������-��������";
					case OpenType.��������:
						return "��������";
					case OpenType.���������:
						return "���������";
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

				// ������ ����������� �������
				if (model.FurnitureSystem.Name == System && model.Leafs.Count > 0)
				{
					// �����������
					bool restriction = RunCalc.getRestriction(model);

					// ������ ���������� ��������
					if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
						list.Add(new SiegeniaAxxentRestriction(model, string.Format("�� �������� � ���������� ������� {1}", System, model.ProfileSystem.Name)));

					// ������ �� ��������
					foreach (clsLeaf leaf in model.Leafs)
					{
						// ���� ������� = roto OK, ���� �� ������ ����� ������ ������� �� roto NT, �� � ������ �������� ��� ��� Roto OK
						foreach (clsBeem beem in leaf)
						{
							if (leaf.Count != 4 || beem.PositionBeem == ItemSide.Other || beem.R1 > 0 || beem.R2 > 0)
							{
								list.Add(new SiegeniaAxxentRestriction(leaf, "������ ������������� �������"));
								break;
							}
						}

						// ������ ��������� �� T ������� 
						if (leaf.OpenView == OpenView.������)
							list.Add(new SiegeniaAxxentRestriction(leaf, "�� ��������� �� ������� T"));

						// ���������� ������� ������ ���, �������� ������ �/�
						switch (leaf.ShtulpOpenType)
						{
							case ShtulpOpenType.ShtulpOnLeaf:
								if (leaf.OpenType != OpenType.����������)
									list.Add(new SiegeniaAxxentRestriction(leaf, "��������� ���������� ������� ������ ��������� ����������"));
								break;
							case ShtulpOpenType.NoShtulpOnLeaf:
								if (leaf.OpenType != OpenType.���������_��������)
									list.Add(new SiegeniaAxxentRestriction(leaf, "�������� ���������� ������� ������ ���������-�������� ����������"));
								break;
						}

						// ����� ����������� ��������� �����, ����������� �� ������ - ����� ������� �������� �������
						// ������ � ����� ������������ �������� ����, �� ��� �� �����������
						if (leaf.HandlePositionType != HandlePositionType.�����������)
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("�������� ������ {0} ��������� �����", HandlePositionType.�����������)));

						// ������� ���� wd-4
						if (!leaf.HandlePosition.IsAutomat)
						{
							leaf.HandlePosition.IsAutomat = true;
							leaf.HandlePosition.HandlePosition = 0;
						}

						// ����� ������� ����� � ������ � � userParam
						if (leaf.HandleType != HandleType.������� && leaf.HandleType != HandleType.���_�����)
							list.Add(new SiegeniaAxxentRestriction(leaf, "�������� ������ ������� �����"));

						clsUserParam ht = leaf.UserParameters.GetParam("��� �����");
						if (ht != null && ht.StringValue != "������� �����")
							list.Add(new SiegeniaAxxentRestriction(leaf, "�������� ������ ������� �����"));

						// ����� 2 ������ ������ �� �������� � ����������
						if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("{0} ������ ����� ���������� ������ �� ���������� � �������� �������", leaf.HingeCount)));

						// --WK1
						// clsUserParam wk1 = model.UserParameters.GetParam("������������ ���������");
						// if (wk1 != null && wk1.StringValue != wk1.DefaultValue.StrValue)
						//	list.Add(new SimpleInvariant(model, "�������� ������ �������� ������������ ���������", delegate
						// 		{
						//			wk1.StringValue = wk1.DefaultValue.StrValue;
						//		}));

						// ������ ����� ���� �������� ��� �� ���
						clsUserParam dekas = model.UserParameters.GetParam("�������� �� �����");
						if (dekas != null && dekas.StringValue != "�����" && dekas.StringValue != "����������" && dekas.StringValue != "��� ��������")
							if (restriction)
								list.Add(new SimpleInvariant(model, string.Empty, delegate { dekas.StringValue = dekas.DefaultValue.StrValue; }));

						// �������
						int h_F = leaf.BaseRegion.Lines[0].HeightInt;
						int w_F = leaf.BaseRegion.Lines[0].WidthInt;

						if (sizesDictionary.ContainsKey(leaf.OpenType))
						{
							int[] sizes = sizesDictionary[leaf.OpenType];

							// ��������������
							if (w_F < sizes[0] || w_F > sizes[1] || h_F < sizes[2] || h_F > sizes[3])
							{
								if (restriction)
									list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ���������� ������� {6} �������: ������ {2}-{3}, ������ {4}-{5} �� ������",
										w_F, h_F, sizes[0], sizes[1], sizes[2], sizes[3], keepRussian(leaf.OpenType))));
							}
							else
							{
								// ���������, ���������� �� 1450 �� ������
								const int ws_F = 1450;
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && w_F > ws_F)
								{
									list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("������ ���������� ������� �� ����� {0}", ws_F)));
								}
							}
						}
						else
						{
							list.Add(new SiegeniaAxxentRestriction(leaf, string.Format("��� ���������� {0} �� ������������", leaf.OpenType)));
						}

						// ������� �� ���������� ������
						if (leaf.OpenType == OpenType.�������� || leaf.OpenType == OpenType.���������)
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
				sizesDictionary.Add(OpenType.���������_��������, new int[] {380, 1650, 460, 3000});
				sizesDictionary.Add(OpenType.����������, new int[] {380, 1650, 460, 3000});
				sizesDictionary.Add(OpenType.��������, new int[] {400, 2400, 500, 1600});
				sizesDictionary.Add(OpenType.���������, new int[] {400, 2400, 500, 1600});
			}

			private static string keepRussian(OpenType openType)
			{
				switch (openType)
				{
					case OpenType.����������:
						return "����������";
					case OpenType.���������_��������:
						return "���������-��������";
					case OpenType.��������:
						return "��������";
					case OpenType.���������:
						return "���������";
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


		// ������ ���� ������
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

				// ��� ���� �/�
				// ���� �������
				// if (!profSystemList.Contains(model.ProfileSystem.Name))
				//    return list;

				const int idddocoperKrupn = 3;
				const int idddocoperPred = 33;
				const int idddocoperDA = 67;
				int? iddocoper = getiddocoper(model);

				// TODO ��� ����� ����� ���� ������
				//                if (iddocoper != idddocoperKrupn && iddocoper != idddocoperPred && iddocoper != idddocoperDA && iddocoper != 4)
				//                    list.Add(new OrderTypeRestriction(model, string.Format("���������� ������� {0} ���������� ��� ������� ���� ������", model.ProfileSystem.Name)));

				bool restriction = RunCalc.getRestriction(model);

				if (restriction && iddocoper != idddocoperKrupn && iddocoper != 33 && model.FurnitureSystem.Name == Axor)
					list.Add(new OrderTypeRestriction(model, string.Format("{0} ����������", model.FurnitureSystem.Name)));

				// ��� ����� =  ������ != ���������
				if (iddocoper == idddocoperKrupn)
				{
					if (model.LastAction == wdAction.AddLeaf && model.Leafs.SelectedLeafs != null && isSomeSelectedLeafHaveHandle(model))
					{
						list.Add(new SimpleInvariant(model, string.Format("�� ��������� � {0} ���������� ������� ��� �����, �� ����� �������", model.ProfileSystem.Name),
							delegate
							{
								foreach (clsLeaf leaf in model.Leafs.SelectedLeafs)
								{
									leaf.HandleType = HandleType.���_�����;
									leaf.UserParameters[__mechanizm].StringValue = __no;
									leaf.UserParameters["��������� �����"].StringValue = __no;
								}
							}));
					}
				}
				else
				{
					// �� ������� �������� �� ���������� Austin :( �������� �������������
					if (model.ProfileSystem.Name == EVO || model.ProfileSystem.Name == NEO_80 || model.ProfileSystem.Name == Thermo76 || model.ProfileSystem.Name == Estet)
					{
						foreach (clsLeaf leaf in model.Leafs)
						{
							// �� ��������� �� ����� ������� �����
							clsUserParam upMech = leaf.UserParameters.GetParam("��������");
							if (upMech != null && upMech.StringValue2 == upMech.DefaultValue.StrValue2)
							{
								clsUserParam up = leaf.UserParameters.GetParam("���������� ������� �����");
								if (up != null && up.StringValue == "Hoppe Austin")
								{
									list.Add(new OrderTypeRestriction(model, string.Format("{0} = {1} �� �������� � ������� {2} ��-�� ������������� ����� ������ �����", up.Name, up.StringValue, model.ProfileSystem.Name)));
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
					if (leaf.HandleType != HandleType.���_�����)
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
			// todo ����������� Vorne ����������� � ���� ������

			private readonly string _message;

			// todo DRY violation
			private EuroSibOptimaMaximaRestriction(object subj, string message) : base(subj)
			{
				_message = message;
			}

			public override string message()
			{
				// ������� �-1 �� �-1 ��� �� �������� �������� �-1
				clsRegion region = subject as clsRegion;
				return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
			}

			public static List<Invalidance> test(clsModel model)
			{
				List<Invalidance> list = new List<Invalidance>(0);

				// ���� �������
				if (!profSystemList.Contains(model.ProfileSystem.Name))
					return list;

				bool restriction = RunCalc.getRestriction(model);

				//				if(restriction && model.ProfileSystem.Name == SibDesign)
				//				{
				//					list.Add(new EuroSibOptimaMaximaRestriction(model, string.Format("���������� ������� {0} ����������", model.ProfileSystem.Name)));
				//					return list;
				//				}

				// ���������
				if (model.FurnitureSystem.Name != FurnSystBEZ)
				{
					bool optimaTitan = isOptimaTitan(model);

					List<string> furnSystemList = model.ConstructionType.Name == _window || model.ConstructionType.Name == _balcon ? model.ProfileSystem.Name == SibDesign || model.ProfileSystem.Name == RehauMaxima ? furnSystemList70 : optimaTitan ? furnSystemList60bis : furnSystemList60 : furnSystemListDoor;

					if (restriction)
					{
						if (!furnSystemList.Contains(model.FurnitureSystem.Name))
							list.Add(new SimpleInvariant
								(model,
								string.Format("� ����������� {0} ��� ���� ����������� {1} ������������ {2}", model.ProfileSystem.Name, model.ConstructionType.Name, string.Join(", ", furnSystemList.ToArray())),
								delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
								));
					}
				}

				const string upShtapicName = "������";
				const string upColorName = "���� ����������";
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
									string.Format("� ����������� {0} ������������ ��� ���������� �������� {1}�� �c���������� ������ {2} {3}", model.ProfileSystem.Name, region.Fill.Thikness, getShtapic(profile), getColor(profile)), delegate
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
								// �� ������ ����, �� � ����� ��� ��� ���������, ������ ����� ���� ��������� �� ����������� � �.�.
								clsFill fill = thickness <= 24 ? model.ProfileSystem.Fills["4�16��4"] : model.ProfileSystem.Fills["4�24��4"];

								// make clojurable
								clsRegion region1 = region;
								string s = string.Join(", ", thicknesses.ConvertAll<string>(delegate(int input) { return input.ToString(); }).ToArray());
								list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ����� ������������ ������ ���������� �������� {1}��", model.ProfileSystem.Name, s), delegate { region1.Fill = fill; }));
							}
							else
							{
								list.Add(new EuroSibOptimaMaximaRestriction(region, string.Format("�� ������� ��������� ����������", model.ProfileSystem.Name, region.Fill.Thikness)));
							}
						}
					}
				}

				return list;
			}

			// ������ �������� // ������ ���� = ����������� ������ ������� �� �������� � ����� ��������� ������ ������ = 
			// ���� ��������� ����� ��������� ������ id ���� ������������ ��������� ������������ ��������
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
					// todo refactor [F1,F2] ��� ��������, � ���� ������� (Thickness) ��� �� ����� :((
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


		// ��� ����� �� ID ��� �� �����
		// 45	����������� �� ������� �������
		// 46	������ (������ ������)
		// 47	������� (������ ������)
		//        private static readonly ConstructionType manualConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("����������� �� ������� �������");
		//        private static readonly ConstructionType gateConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("������ (������ ������)");
		//        private static readonly ConstructionType wicketConstructionType = winDraw.SettingsLoad.currentSettings.GetConstructionTypeByName("������� (������ ������)");

		///
		/// *** ����� �����
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

				// ������ ����������� �������
				if (model.FurnitureSystem.Name != TitanDebug && model.FurnitureSystem.Name != SiegeniaTitan && model.FurnitureSystem.Name != SiegeniaTitanWK1)
					return list;

				if (model.Leafs.Count == 0)
					return list;

				// �����������
				bool restriction = getRestriction(model);

				// ������ ���������� ��������
				if (!profSystemsToUse.Contains(model.ProfileSystem.Name))
					list.Add(new TitanRestriction(model, string.Format("�� �������� � ���������� ������� {0}", model.ProfileSystem.Name)));

				// ������ �� ��������
				foreach (clsLeaf leaf in model.Leafs)
					{
					bool wk1 = getWk1(leaf);

					// ����� ����� WK1
					if (model.ProfileSystem.Name == Vario && !wk1)
						list.Add(new SimpleInvariant(model, string.Format("� ���������� ������� {0} ������������ ������ ��������������� ����������", model.ProfileSystem.Name), delegate
							{
								clsUserParam upWk1 = model.UserParameters.GetParam("������������ ���������");
								if (upWk1 != null)
									upWk1.StringValue = "���������������";
							}));

					// ������ ����������� ��� ����������� ��������� �����
					if (leaf.HandlePositionType != HandlePositionType.����������� && leaf.HandlePositionType != HandlePositionType.�������������)
						list.Add(new TitanRestriction(leaf, string.Format("�������� ������ {0} ��� {1} ��������� �����", HandlePositionType.�����������, HandlePositionType.�������������)));

					// DM8 ������ ����������� ��������� �����
					if (leaf.HandleBeem != null && leaf.HandleBeem.Profile.C <= 50 && leaf.HandlePositionType != HandlePositionType.�����������)
						list.Add(new TitanRestriction(leaf, string.Format("��� DM8 �������� ������ {0} ��������� �����", HandlePositionType.�����������)));

					// DM25 ������� �������� ������ ��� ������ �������� ����� �� ������ >1200 // �������.pdf ���. 6.2.30 - 6.2.34
					int TGMK0520 = 1200;
					if (leaf.HandleBeem != null && leaf.HandleBeem.Profile.C > 60 && leaf.HandleBeem.LineE.Length <= TGMK0520 && leaf.UserParameters[__mechanizm].StringValue.ToLower().Contains("����"))
						list.Add(new TitanRestriction(leaf, string.Format("��� ������� DM25 ������� ����� (����) ��������������� ��� ��� > {0}��", TGMK0520)));

					// ���������� ������� ������ ���, �������� ������ �/�
					switch (leaf.ShtulpOpenType)
					{
						case ShtulpOpenType.ShtulpOnLeaf:
							if (leaf.OpenType != OpenType.����������)
								list.Add(new TitanRestriction(leaf, "��������� ���������� ������� ������ ���������� ����������"));
							break;
						case ShtulpOpenType.NoShtulpOnLeaf:
							if (leaf.OpenType != OpenType.���������_�������� && restriction)
								list.Add(new TitanRestriction(leaf, "�������� ���������� ������� ������ ���������-�������� ����������"));
							break;
					}

					// ����� 2 ������ ������ �� �������� � ����������
					if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
						list.Add(new TitanRestriction(leaf, string.Format("{0} ������ ����� ���������� ������ �� ���������� � �������� �������", leaf.HingeCount)));

					//                    // ����� ������� ����� � ������ � � userParam
					//                    if (leaf.HandleType != HandleType.������� && leaf.HandleType != HandleType.���_�����)
					//                        list.Add(new SiegeniaClassicRestriction(leaf, "�������� ������ ������� �����"));
					//
					//                    clsUserParam ht = leaf.UserParameters.GetParam("��� �����");
					//                    if (ht != null && ht.StringValue != "������� �����")
					//                        list.Add(new SiegeniaClassicRestriction(leaf, "�������� ������ ������� �����"));


					// �������
					int hF = leaf.BaseRegion.Lines[0].HeightInt;
					int wF = leaf.BaseRegion.Lines[0].WidthInt;

					// ���/��� <=1.5 ������ �������� ��� ����������� ��� �� ��� �������� ��������� ������� � ���������� ���������� ����������� ����
					if (leaf.OpenType == OpenType.���������_�������� || leaf.OpenType == OpenType.����������)
					{
						if ((double) wF / (double) hF > 1.5)
							if (restriction)
								list.Add(new TitanRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ����������� �� ����������� ������, ������ �� ������ ��������� ������ ����� ��� � 1,5 ����", wF, hF)));
					}

					/// ���������
					/// 
					/// ��������
					if (isStandart(leaf))
					{
						// ���� ����������
						if (leaf.OpenType != OpenType.���������_�������� && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
						{
							list.Add(new TitanRestriction(leaf, string.Format("��� ���������� {0} �� ������������", leaf.OpenType)));
						}
						else
						{
							int[] limits = getLimits(leaf);

							if (limits != null && (wF > limits[2] || hF > limits[3]))
							{
								if (restriction)
									list.Add(new TitanRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ������������ ������� {7} {6} ������� {9}: ������ �� {4}, ������ �� {5} �� ������",
										wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
										)));
							}

							if (limits != null && (wF < limits[0] || hF < limits[1]))
							{
								if (restriction)
									list.Add(new TitanRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ����������� ������� {7} {6} ������� DM{8} {9}: ������ �� {2}, ������ �� {3} �� ������",
										wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
										)));
							}
						}
					}
					else
						{
						/// ����������, ������ ��� 1)����������, 2)���������� ������, 3)���� �������, 
						/// ��������� �� :
						/// 1) todo ���� ����������� ���� ����� // ��� �� �����������
						/// 2) todo ������ �������� ��������������
						switch (leaf.OpenType)
						{
							case OpenType.����������:
								// ���������� ���������� 4-�� �������� ������ �� �/� ��������
								if (leaf.ShtulpOpenType == ShtulpOpenType.ShtulpOnLeaf && leaf.Count == 4)
									{
									goto case OpenType.���������_��������;
									}

								// todo
								if (restriction)
								list.Add(new TitanRestriction(leaf, string.Format("�� ����������� {0} (��������) ���������� �/� ����������", leaf.OpenType)));

								break;

							case OpenType.���������_��������:

							//if (leaf.OpenType == )

							// �������� ���������
							if (!isVertical(leaf.HingBeem))
							{
							list.Add(new TitanRestriction(leaf, "�������� ����� �� �����������"));
							break;
							}

							// ��� ������� ������, ������� ������� ����������� 
							clsBeem bottom = getFirst(leaf, ItemSide.Bottom);
							if (bottom == null)
							{
							list.Add(new TitanRestriction(leaf, "��������� �� �������������"));
							break;
							}

							// ��������� � �������� ������ ���� ��������� ��� ������ �����
							if (getNextBottom(leaf.HingBeem) != bottom)
							{
							list.Add(new TitanRestriction(leaf, "��������� � �������� ������ ���� ��������� ��� ������ �����"));
							break;
							}

							/// Qu = 3 // ����������� 
							if (leaf.Count == 3)
							{
							// ����� ���������
							if (leaf.HandleBeem.PositionBeem != ItemSide.Other)
							{
							list.Add(new TitanRestriction(leaf, "����� ����� �� ���������"));
							}

							// ���� ����� ��������� 35-45
							if (restriction)
							{
							const int minA = 35;
							const int maxA = 45;
							double a = getBeemAngleOtherHorizont(leaf.HandleBeem);
							if (a < minA || a > maxA)
							{
							list.Add(new TitanRestriction(leaf, string.Format("���� ������� ����� ����� = {0} ��������, ��� ���������� {1}-{2}", a, minA, maxA)));
							}
							}

							// �����������
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "��������� ����� � �����������"));
							}

							// �������
							addLimitsError(list, leaf, new int[] {575, 575, 1200, 1200});
							}
							else if (leaf.Count == 4)
							{
							//Label: 
							if (getNextBottom(leaf.HandleBeem) != bottom)
							{
							list.Add(new TitanRestriction(leaf, "����� ����� �� ��������� � ����������"));
							}

							// ������ ��������� ������� �����
							if (isVertical(leaf.HandleBeem))
							{
							/// ����������� ����� �����

							// ���� ��������� 15-165 � ��������� => 0-45 � ���������
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
							list.Add(new TitanRestriction(leaf, string.Format("���� ������� ����� = {0} �������� � �����������, ��� ���������� {1}-{2}", a, minA, maxA)));
							}
							}

							// ��������� 1 ��������� ����� ������ ��� other
							if (getRadiusQu(leaf) > 1)
							{
							list.Add(new TitanRestriction(leaf, "�� ����� ����� ��������� �����"));
							// ����� ��� ����� �� ���������, ������� �� switch (leaf.OpenType)
							break;
							}

							clsBeem ark = getRadiusFirst(leaf);
							if (ark != null && ark.PositionBeem != ItemSide.Top && ark.PositionBeem != ItemSide.Other)
							{
							list.Add(new TitanRestriction(leaf, "��������� ����� �������� ������ � ������� ��� ��������� ���������"));
							// ����� ��� ����� �� ���������, ������� �� switch (leaf.OpenType)
							break;
							}

							if (ark != null && ark.R1 < 0)
							{
							list.Add(new TitanRestriction(leaf, "�������� ��������� �����"));
							}

							// �������
							if (addLimitsError(list, leaf, new int[] {420, 460, ark != null ? 1000 : 1200, 2400})) ; // 900 ���������
							{
							// ����� ��� �������� 815
							if (ark == null)
							{
							const int min = 815;
							if (leaf.HingBeem.LineE.LengthInt < min)
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("�����, ����������� ������ �������� ����� {0}", min)));
							}

							// ��� ����� �����
							int minHandleF = leaf[0].Profile.C <= 60 ? 260 : 600;
							if (leaf.HandleBeem != null && leaf.HandleBeem.LineE.LengthInt < minHandleF)
							{
							list.Add(new TitanRestriction(leaf, string.Format("�����, ����������� ������ ����� � ������ {0}", minHandleF)));
							}
							}
							else
							{
							int a = (int) Math.Round(leaf.HandleBeem.LGabarit.AngelBeetwenLines(ark.LGabarit));
							if (a == 180)
							{
							// ���������� ���������� 
							const int min = 510;
							if (leaf.HandleBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("�������, ����������� ������ ����� ����� {0}", min)));
							}
							else
							{
							// ������� 
							const int min = 550;
							if (leaf.HandleBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("�������, ����������� ������ ����� ����� {0}", min)));
							}

							int b = (int) Math.Round(leaf.HingBeem.LGabarit.AngelBeetwenLines(ark.LGabarit));
							if (b == 180)
							{
							// ���������� ���������� 
							const int min = 495;
							if (leaf.HingBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("�������, ����������� ������ �������� ����� {0}", min)));
							}
							else
							{
							// ������� 
							const int min = 565;
							if (leaf.HingBeem.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("�������, ����������� ������ �������� ����� {0}", min)));
							}
							}
							}
							}
							else
							{
							// ���� = �������������, ������ ������� �������� 
							clsLeafBeem top = getFirst(leaf, ItemSide.Top);
							if (top == null)
							{
							list.Add(new TitanRestriction(leaf, "������� ����� �� ��������������"));
							break;
							}

							// ��������� �����
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "��������� ����� � ������������ �����"));
							// ����� ��� ����� �� ���������, ������� �� switch (leaf.OpenType)
							break;
							}

							// �������
							if (addLimitsError(list, leaf, new int[] {420, 460, 1200, 2400}))
							{
							const int min = 230;
							if (top.LineE.LengthInt < min)
							list.Add(new TitanRestriction(leaf, string.Format("�����, ����������� ������ ����� ������ {0}", min)));
							}
							}
							}
							else if (leaf.Count == 5)
							{
							// ����� ��������� � �������������� ��������� ��� ������ �����
							if (getNextBottom(leaf.HandleBeem) != bottom || !isVertical(leaf.HandleBeem))
							{
							list.Add(new TitanRestriction(leaf, "����� ����� ������� ���� ��������� � ���������� ��� ������ ����"));
							break;
							}

							// ��������� �����
							if (getRadiusQu(leaf) > 0)
							{
							list.Add(new TitanRestriction(leaf, "��������� ����� � ������������ �����"));
							}

							// ��� ����� �����
							int minHandleF = leaf[0].Profile.C <= 60 ? 260 : 600;
							if (leaf.HandleBeem != null && leaf.HandleBeem.LineE.LengthInt < minHandleF)
							{
							list.Add(new TitanRestriction(leaf, string.Format("�����, ����������� ������ ����� � ������ {0}", minHandleF)));
							}

							// ��� �������� ����� �������� �� ��� ����� TSKR1050
							const int minHingeF = 485;
							if (leaf.HingBeem != null && leaf.HingBeem.LineE.LengthInt < 485)
							{
							list.Add(new TitanRestriction(leaf, string.Format("�����, ����������� ������ �������� ����� {0}", minHingeF)));
							}

							// �������
							addLimitsError(list, leaf, new int[] {420, 460, 1200, 2400});
							}
							else
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("���������� {0} {1} ������ �� ������������", leaf.OpenType, leaf.Count)));
							break;
							}

							break;

							case OpenType.��������:
							case OpenType.���������:

							// ������� ������������� �����
							if (leaf.HingBeem.PositionBeem != ItemSide.Bottom && leaf.HingBeem.PositionBeem != ItemSide.Top)
							{
							list.Add(new TitanRestriction(leaf, string.Format("�������� ����� �� �������������")));
							break;
							}

							if (leaf.Count <= 4)
							{
							// �������
							addLimitsError(list, leaf, new int[] {200, 200, 2400, 800});

							// ������ ����� �� �������� �� ����� 410
							if (leaf.HandleBeem.LineE.Length < 510)
							list.Add(new TitanRestriction(leaf, string.Format("����� ����� ����� �� ����� 510 ��")));

							break;
							}
							else
							{
							if (restriction)
							list.Add(new TitanRestriction(leaf, string.Format("���������� {0} {1} ������ �� ������������", leaf.OpenType, leaf.Count)));
							break;
							}

							default:
							list.Add(new TitanRestriction(leaf, string.Format("��� ���������� {0} �� ������������", leaf.OpenType)));
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
			list.Add(new TitanRestriction(leaf, string.Format("������ {0} x ������ {1} �� ������, �������� ����������� ������� {6} �������: ������ �� {2}, ������ �� {3} �� ������",
			wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
			)));
			return false;
			}

			;

			if (wF > limits[2] || hF > limits[3])
			if (restriction)
			{
			list.Add(new TitanRestriction(leaf, string.Format("������ {0} x ������ {1} �� ������, �������� ������������ ������� {6} �������: ������ �� {4}, ������ �� {5} �� ������",
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
			case OpenType.���������_��������:
			case OpenType.����������:
			// ���������� ������� ��� ����������
			int[] limits = new int[] {330, 460, 1560, 2400};

			/// ������������ ������� ���������

			// � ������ ������������ .C ������ 15-50 / 51-60 / 61-120 = DM8 / DM15 / DM25
			// leaf[0].Profile.C;
			// DM8-15 leaf.Dornmas <= 15
			if (leaf[0].Profile.C <= 60)
			{
			// ��������� �������
			if (leaf.ShtulpOpenType == ShtulpOpenType.NoShtulp)
			{
			int hF = leaf.BaseRegion.Lines[0].HeightInt;
			int wF = leaf.BaseRegion.Lines[0].WidthInt;

			// �������� 4020 ����������� ����� WK1
			// �/� �� 461 � * ���������� ����� ������� ��� �� 230
			if (!wk1 && leaf.HandlePositionType == HandlePositionType.����������� && (hF >= 461 || leaf.OpenType == OpenType.����������))
			limits[0] = 230;

			// ����������-��������
			if (leaf.OpenType == OpenType.���������_��������)
			{
			// ����������� 260, ��������� TGMK4010+5090+4020 �� �� WK1 ���� TEUL4200 �� o����� / ���������� 310 = TGKK3010+4020 � WK1
			limits[1] = leaf.HandlePositionType == HandlePositionType.����������� && !wk1 ? 260 : 310;
			}
			// ����������
			else
			{
			// ����������� 230 ��������� AGMD1150 / ���������� 300
			limits[1] = leaf.HandlePositionType == HandlePositionType.����������� ? 230 : 300;
			}
			}
			// ���������� ����� �� ����� ���� ���� 310 ���� ������������� TGKS0010 + 4020
			else
			{
			limits[1] = 310;
			}
			}
			// DM25++
			else
			{
			// ����� �� 3000 �� ������, ��������� ��� ������ �� �� ������� �������������
			limits[3] = 3000;

			// ��� �� 600, ����� ������������ � �������� 8000
			limits[1] = leaf.HandlePositionType == HandlePositionType.����������� ? 600 : 800;
			}


			return limits;

			case OpenType.��������:
			case OpenType.���������:
			return new int[] {200, 200, 2400, 800};
			}

			return null;
			}

			private static bool getWk1(clsLeaf leaf)
			{
			clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("������������ ���������");
			return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
			}

			private static string keepRussian(OpenType openType)
			{
			switch (openType)
			{
			case OpenType.����������:
			return "����������";
			case OpenType.���������_��������:
			return "���������-��������";
			case OpenType.��������:
			return "��������";
			case OpenType.���������:
			return "���������";
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
			return "�������� ����������";
			case ShtulpOpenType.ShtulpOnLeaf:
			return "��������� ����������";
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

		// ������ ����������� �������
		if (model.FurnitureSystem.Name != RotoNT && model.FurnitureSystem.Name != RotoNTDesigno)
		return list;

		// �����������
		bool restriction = getRestriction(model);

		// ��������� RotoNT // ������� 09/09/2019
		if (restriction)
		{
		list.Add(new RotoNTRestriction(model, "����������"));
		}

		// ��������� RotoNTDesigno // �������� 09/07/2019
		if (restriction && model.FurnitureSystem.Name == RotoNTDesigno)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ����������, ������ �� {1}", model.FurnitureSystem.Name, SiegeniaAxxent), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaAxxent); }));
		}

		// ������ ���������� ��������
		if (model.FurnitureSystem.Name == RotoNT && !profSystemsToUse.Contains(model.ProfileSystem.Name))
		list.Add(new RotoNTRestriction(model, string.Format("�� �������� � ���������� ������� {0}", model.ProfileSystem.Name)));
		if (model.FurnitureSystem.Name == RotoNTDesigno && !designoSystemsToUse.Contains(model.ProfileSystem.Name))
		list.Add(new RotoNTRestriction(model, string.Format("�� �������� � ���������� ������� {0}", model.ProfileSystem.Name)));

		// ������ �� ��������
		foreach (clsLeaf leaf in model.Leafs)
		{
		bool wk1 = getWk1(leaf);

		// ����� ����� WK1
		if (model.ProfileSystem.Name == Vario && !wk1)
		list.Add(new SimpleInvariant(model, string.Format("� ���������� ������� {0} ������������ ������ ��������������� ����������", model.ProfileSystem.Name), delegate
		{
		clsUserParam upWk1 = model.UserParameters.GetParam("������������ ���������");
		if (upWk1 != null)
		upWk1.StringValue = "���������������";
		}));
		}

		return list;
		}

		private static bool getWk1(clsLeaf leaf)
		{
		clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("������������ ���������");
		return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
		}
		}


		/// TODO ������ � ������� :( ���� �� ���������������
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
		private const string defaultFill = "4x16x4x16x�4";
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
		return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� �������
		if (!profSystemList.Contains(model.ProfileSystem.Name))
		return list;

		// ��������� === Titan
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		list.Add(new SimpleInvariant
		(model,
		string.Format("� ����������� {0} ������������ ����������� ������� {1}", model.ProfileSystem.Name, string.Join(",", furnSystemList.ToArray())),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
		));
		}

		/// ������� ���������� === defaulThickness = 44
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != defaulThickness)
		{
		clsFill fill = getDefaultFill(model);
		if (fill != null)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model,
		string.Format("� ����������� {0} ����� ������������ ������ ���������� {1} ��", model.ProfileSystem.Name, defaulThickness),
		delegate { region1.Fill = fill; }
		));
		}
		else
		{
		list.Add(new GealanRestriction(region, string.Format("������ ���������� {0} ��", defaulThickness)));
		}
		}
		}

		// ������ ��������� ������ � ����� �����������
		const string upShtapicName = "������";
		const string upColorName = "���� ����������";
		const string skew = "���������";
		const string grey = "�����";
		clsUserParam upShtapic = model.UserParameters.GetParam(upShtapicName);
		clsUserParam upColor = model.UserParameters.GetParam(upColorName);
		if ((upShtapic != null && upShtapic.StringValue != skew) || (upColor != null && upColor.StringValue != grey))
		{
		list.Add(new SimpleInvariant(
		model,
		string.Format("� ����������� {0} ������������ ������ {1} ������ � {2} ���� ����������", model.ProfileSystem.Name, skew, grey),
		delegate
		{
		model.UserParameters[upShtapicName].StringValue = skew;
		model.UserParameters[upColorName].StringValue = grey;
		}));
		}

		// ��� ���� ��� ���� �����������
		clsBeem beem = getSkewOrArcBeem(model);
		if (beem != null)
		{
		if (getRestriction(model))
		list.Add(new GealanRestriction(beem, string.Format("{0}, ���� ����������, ������ ����������� ����", beem.Name)));
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
		// �������� ������������������ �� ��������� ��� ������ �������
		foreach (clsLeaf leaf in model.Leafs)
		{
		const string �� = "��";
		const string ������������������ = "������������ �������������";
		clsUserParam up = leaf.UserParameters.GetParam(������������������);
		if (up != null && up.StringValue != ��)
		{
		list.Add(new ProductDefault(model, string.Format("�������� {0}", ������������������), delegate { up.StringValue = ��; }));
		}
		}
		}

		return list;
		}

		private static bool isSpecialCase(clsModel model)
		{
		// �� ������ ��� ���� ��� ������������������ �� ������� ������� � ������� ���������, �� �� ��� � �� ������������ ������ ������ � ���� ����������
		if (isPvh(model))
		{
		// ������ ���� ����� ���� ����������� ����� �� ��������� � �����, �������� ���� ������� ����� ������ �������� ������� ��� ������� �������� ���� ��������� ������� ������ ����� �� ������
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

		// ������ ����������� �������
		if (model.FurnitureSystem.Name != Axor)
		return list;

		if (model.Leafs.Count == 0)
		return list;

		// �����������
		bool restriction = getRestriction(model);

		// ������ ���������� ��������
		const int ��������������������������� = 33;
		int iddocoper = getiddocoper(model) ?? 0;
		if (!profSystemsToUse.Contains(model.ProfileSystem.Name) && iddocoper != ���������������������������)
		list.Add(new AxorRestriction(model, string.Format("�� �������� � ���������� ������� {0}", model.ProfileSystem.Name)));

		// ������ �� ��������
		foreach (clsLeaf leaf in model.Leafs)
		{
		bool wk1 = getWk1(leaf);

		// ����� ~WK1
		if (wk1)
		list.Add(new SimpleInvariant(model, string.Format("��������� {0} �� ����� �����������������", model.FurnitureSystem.Name), delegate
		{
		clsUserParam upWk1 = model.UserParameters.GetParam("������������ ���������");
		if (upWk1 != null)
		upWk1.StringValue = "��������";
		}));

		// ������ ����������� ��������� �����
		if (leaf.HandlePositionType != HandlePositionType.�����������)
		list.Add(new AxorRestriction(leaf, string.Format("�������� ������ {0} ��������� �����", HandlePositionType.�����������)));

		// ���������� ������� ������ ���, �������� ������ �/�
		switch (leaf.ShtulpOpenType)
		{
		case ShtulpOpenType.ShtulpOnLeaf:
		if (leaf.OpenType != OpenType.����������)
		list.Add(new AxorRestriction(leaf, "��������� ���������� ������� ������ ���������� ����������"));
		break;
		case ShtulpOpenType.NoShtulpOnLeaf:
		if (leaf.OpenType != OpenType.���������_�������� && leaf.OpenType != OpenType.����������)
		list.Add(new AxorRestriction(leaf, "�������� ���������� ������� ������ ���������-�������� ��� ���������� ����������"));
		break;
		}

		// ����� 2 ������ ������ �� �������� � ����������
		if (leaf.HingeCount > 2 && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
		list.Add(new AxorRestriction(leaf, string.Format("{0} ������ ����� ���������� ������ �� ���������� � �������� �������", leaf.HingeCount)));


		// �������
		int hF = leaf.BaseRegion.Lines[0].HeightInt;
		int wF = leaf.BaseRegion.Lines[0].WidthInt;

		// ���/��� <=1.5 ??!!
		if (leaf.OpenType == OpenType.���������_�������� || leaf.OpenType == OpenType.����������)
		{
		if ((double) wF / (double) hF > 1.5)
		if (restriction)
		list.Add(new AxorRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ����������� �� ����������� ������, ������ �� ������ ��������� ������ ����� ��� � 1,5 ����", wF, hF)));
		}

		/// ���������
		/// 
		/// ��������
		if (!isStandart(leaf))
		{
		list.Add(new AxorRestriction(leaf, string.Format("{0} ������ ������������� �������", model.FurnitureSystem.Name)));
		}

		// �������� �������� ����������� ���������
		{
		// ���� ����������
		if (leaf.OpenType != OpenType.���������_�������� && leaf.OpenType != OpenType.���������� && leaf.OpenType != OpenType.�������� && leaf.OpenType != OpenType.���������)
		{
		list.Add(new AxorRestriction(leaf, string.Format("��� ���������� {0} �� ������������", leaf.OpenType)));
		}
		else
		{
		int[] limits = getLimits(leaf);

		if (limits != null && (wF > limits[2] || hF > limits[3]))
		{
		if (restriction)
		list.Add(new AxorRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ������������ ������� {7} {6} ������� {9}: ������ �� {4}, ������ �� {5} �� ������",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType), keepRussian(leaf.ShtulpOpenType), leaf.Dornmas, wk1 ? "WK1" : string.Empty
		)));
		}

		if (limits != null && (wF < limits[0] || hF < limits[1]))
		{
		list.Add(new AxorRestriction(leaf, string.Format("������ {0}, ������ {1} �� ������, �������� ����������� ������� {7} {6} ������� DM{8} {9}: ������ �� {2}, ������ �� {3} �� ������",
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
		list.Add(new AxorRestriction(leaf, string.Format("������ {0} x ������ {1} �� ������, �������� ����������� ������� {6} �������: ������ �� {2}, ������ �� {3} �� ������",
		wF, hF, limits[0], limits[1], limits[2], limits[3], keepRussian(leaf.OpenType)
		)));
		return false;
		}

		;

		if (wF > limits[2] || hF > limits[3])
		if (restriction)
		{
		list.Add(new AxorRestriction(leaf, string.Format("������ {0} x ������ {1} �� ������, �������� ������������ ������� {6} �������: ������ �� {4}, ������ �� {5} �� ������",
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
		case OpenType.���������_��������:
		return new int[] {shtup ? 400 : 300, shtup ? 550 : 450, 1520, 2390};

		case OpenType.����������:
		int wF = leaf.BaseRegion.Lines[0].WidthInt;
		return new int[] {280, shtup ? 550 : wF > 700 ? 450 : 300, 1520, 2400};

		case OpenType.��������:
		case OpenType.���������:
		int hF = leaf.BaseRegion.Lines[0].HeightInt;
		return new int[] {hF > 700 ? 450 : 200, 280, 2000, 1600};
		}

		return null;
		}

		private static bool getWk1(clsLeaf leaf)
		{
		clsUserParam upWk1 = leaf.Model.UserParameters.GetParam("������������ ���������");
		return upWk1 != null && upWk1.StringValue != upWk1.DefaultValue.StrValue;
		}

		private static string keepRussian(OpenType openType)
		{
		switch (openType)
		{
		case OpenType.����������:
		return "����������";
		case OpenType.���������_��������:
		return "���������-��������";
		case OpenType.��������:
		return "��������";
		case OpenType.���������:
		return "���������";
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
		return "�������� ����������";
		case ShtulpOpenType.ShtulpOnLeaf:
		return "��������� ����������";
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
		return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� �������
		if (model.ProfileSystem.Name != RehauGrazio)
		return list;

		// �����������
		bool restriction = getRestriction(model);

		// ��������� 
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
		list.Add(new RuhauGrazioRestriction(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray()))));
		}

		// ������� ���������� === 32 | 40
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != 32 && region.Fill.Thikness != 40 && region.Fill.Thikness != 0)
		{
		list.Add(new RuhauGrazioRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� 32�� ��� 40��", model.ProfileSystem.Name)));
		}
		}

		clsUserParam upShtap = model.UserParameters.GetParam("������");
		clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");
		const string ��������� = "���������";
		const string ������ = "������";
		const string ����� = "�����";

		if (upShtap != null && upShtap.StringValue != ���������)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upShtap.Name, ���������), delegate { upShtap.StringValue = ���������; }));
		}

		// ������ ���� = ������� ������� �� ��������, ����!
		int idseller = getIdseller(model);
		bool isRbnGrazioGreySeller = isRbnGrazioGrey(idseller);

		if (upRubberColor != null)
		{
		if (isRbnGrazioGreySeller)
		{
		if (upRubberColor.StringValue != ������ && upRubberColor.StringValue != �����)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upRubberColor.Name, ������), delegate { upRubberColor.StringValue = ������; }));
		}
		}
		else
		{
		if (upRubberColor.StringValue != ������)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upRubberColor.Name, ������), delegate { upRubberColor.StringValue = ������; }));
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
		return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� �������
		if (model.ProfileSystem.Name != RehauBlitzNew)
		return list;

		// �����������
		bool restriction = getRestriction(model);

		// ��������������
		int? iddocoper = getiddocoper(model);

		// ������ ���� = ������� ������� �� ��������, ����!
		int idseller = getIdseller(model);

		// ������ ���� ������ �������� ������ �� ������� ������
		bool isRbnVorneSeller = Settings.isDealer && isRbnVorne(idseller);
		bool isRbnGrazioGreySeller = isRbnGrazioGrey(idseller);

		// ���������
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (Settings.isDealer && idseller != 798)
		{
		if (isRbnVorneSeller)
		{
		if (model.FurnitureSystem.Name != Vorne)
		list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, Vorne), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(Vorne); }));
		}
		else
		{
		if (model.FurnitureSystem.Name != SiegeniaClassic)
		list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, SiegeniaClassic), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(SiegeniaClassic); }));
		}
		}
		else
		{
		if (!furnSystemList.Contains(model.FurnitureSystem.Name) && restriction)
		{
		list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }));
		}
		}
		}

		/// ������� ���������� === 24 | 32
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && region.Fill.Thikness != 0)
		{
		list.Add(new RuhauBlitzNewRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� 24�� ��� 32��", model.ProfileSystem.Name)));
		}
		}

		// ������ ��������� ������ ������
		clsUserParam upShtap = model.UserParameters.GetParam("������");
		clsUserParam upRubberColor = model.UserParameters.GetParam("���� ����������");
		const string ��������� = "���������";
		const string ������ = "������";
		const string ����� = "�����";

		if (upShtap != null && upShtap.StringValue != ���������)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upShtap.Name, ���������), delegate { upShtap.StringValue = ���������; }));
		}

		if (upRubberColor != null)
		{
		if (isRbnGrazioGreySeller)
		{
		if (upRubberColor.StringValue != ������ && upRubberColor.StringValue != �����)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upRubberColor.Name, ������), delegate { upRubberColor.StringValue = ������; }));
		}
		}
		else
		{
		if (upRubberColor.StringValue != ������)
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} ������ {1}", upRubberColor.Name, ������), delegate { upRubberColor.StringValue = ������; }));
		}
		}
		}

		/// ������������� �����������, ��� ����� ����� ������ �������� �� ���� ����� ������� � ������������
		/// 2) ��� ������� ����� ���� �� (iddocoper == 67) � � (iddocoper == 3) �������� ������ ��� ����������� ��������� ���������� � ���������� 4-16-4 � 24 �������
		if (Settings.isDealer && idseller != 798 && !isRbnVorneSeller && iddocoper != idddocoperOknaDa && iddocoper != idddocoperRekl && iddocoper != idddocoperKrupn && Settings.idpeople != 74 && Settings.idpeople != 159)
		{
		if (model.ConstructionType.Name != _balconGlazing)
		{
		if (restriction)
		{
		list.Add(new RuhauBlitzNewRestriction(model, string.Format("� ���������� ������� {0} �������� ������ ��� ����������� {1}", model.ProfileSystem.Name, _balconGlazing)));
		}
		}
		else
		{
		foreach (clsRegion region in model.VisibleRegions)
		{
		const string defEmpty = "���_������_�_�������";
		const string defEmpty24 = "���_������_24";
		const string defGlass = "4�16�4";
		const string defSandwich = "COS0001.07";
		if (region.Fill.Marking != defEmpty && region.Fill.Marking != defEmpty24 && region.Fill.Marking != defGlass && region.Fill.Marking != defSandwich)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, string.Format("� ���� ����������� {0} �������� ������ ���������� {1} � {2}", model.ConstructionType.Name, "4�16�4", "COS0001.07"), delegate
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
		return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
		}

		public override string uniq()
		{
		return _message;
		}

		public static List<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� �������
		if (model.ProfileSystem.Name != SCHTANDART_START && model.ProfileSystem.Name != SCHTANDART_COMFORT && model.ProfileSystem.Name != SCHTANDART_PREMIUM)
		return list;

		// �����������
		bool restriction = getRestriction(model);

		// ���������
		if (model.FurnitureSystem.Name != FurnSystBEZ && restriction)
		{
		List<string> furnSystemList = model.ProfileSystem.Name != SCHTANDART_PREMIUM ? furnSystemList60 : furnSystemList70;
		if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		list.Add(new SimpleInvariant(model, string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, string.Join(", ", furnSystemList.ToArray())), delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }));
		}

		/// TODO REFACTOR:: ������ ��� � ������� � ����� ���������������, � �� ����� ������������ � ������ ������ � ���������� ��� � ������ �������
		/// ������� ���������� === 24, 32, 32|40
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region.Fill.Thikness > 0)
		{
		switch (model.ProfileSystem.Name)
		{
		case SCHTANDART_START:
		if (region.Fill.Thikness != 24)
		list.Add(new SchtandartRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� {1}��", model.ProfileSystem.Name, 24)));
		break;

		case SCHTANDART_COMFORT:
		if (region.Fill.Thikness != 32)
		list.Add(new SchtandartRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� {1}��", model.ProfileSystem.Name, 32)));
		break;

		case SCHTANDART_PREMIUM:
		if (region.Fill.Thikness != 32 && region.Fill.Thikness != 40)
		list.Add(new SchtandartRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� {1}, {2}��", model.ProfileSystem.Name, 32, 40)));
		break;
		}
		}
		}

		// ���������� = foo(�� ������) �� ������� �� �������, �������� ������ �������� �� ���� ���������� � ������
		const string ������ = "������";
		const string ����� = "�����";
		const string �������������� = "���� ����������";
		clsUserParam upRubberColor = model.UserParameters.GetParam(��������������);
		switch (model.ProfileSystem.Name)
		{
		case SCHTANDART_START:
		if (upRubberColor != null && upRubberColor.StringValue != ������)
		list.Add(new SimpleInvariant(model, string.Format("��� ���� ����������� ����������� {0} {1}", upRubberColor.Name, ������), delegate { upRubberColor.StringValue = ������; }));
		break;
		default:
		if (upRubberColor != null && upRubberColor.StringValue != �����)
		list.Add(new SimpleInvariant(model, string.Format("��� ���� ����������� ����������� {0} {1}", upRubberColor.Name, �����), delegate { upRubberColor.StringValue = �����; }));
		break;
		}

		// ��������� ���� ���������� � ��������� ��������� >0 ���� �� ����� 1 �������
		if(getThicknesses(model).Count > 1)
		list.Add(new SchtandartRestriction(model, "��������� ������� ���������� � �������� ����� ������"));

		// ������ ����� ����� ������ ��� ���������� � ��������� �������� > 0
		if (getThickness(model) > 0)
		{
		List<clsProfile> shtapikProfiles = getShtapikProfiles(model);
		if (shtapikProfiles == null || shtapikProfiles.Count == 0)
		{
		list.Add(new SchtandartRestriction(model, string.Format("�� ������� ��������� ��������� ������ ��� ��������� �������� {0}�� � ������ ��������� {1}", getThickness(model), upRubberColor != null ? upRubberColor.StringValue : string.Empty)));
		}
		else
		{
		clsUserParam upShtap = model.UserParameters.GetParam("������");
		if (upShtap != null)
		{
		clsProfile match = shtapikProfiles.Find(delegate(clsProfile profile) { return profile.Steels.Count > 0 && profile.Steels[0] == upShtap.StringValue; });
		if (match == default(clsProfile))
		{
		string shtapik = shtapikProfiles[0].Steels[0];
		list.Add(new SimpleInvariant(model, string.Format("��� ���� ����������� ����������� {0} {1}", upShtap.Name, shtapik), delegate { upShtap.StringValue = shtapik; }));
		}
		}
		}
		}

		// ��������� - ������������������
		const string ������������������������� = "������������ �������������";
		foreach (clsLeaf leaf in model.Leafs)
		{
		const string �� = "��";
		const string ��� = "���";
		bool microVent = model.ProfileSystem.Name != SCHTANDART_START && leaf.OpenType == OpenType.���������_��������;
		clsUserParam upMicroVent = leaf.UserParameters.GetParam(�������������������������);
		if (upMicroVent != null && upMicroVent.StringValue != (microVent ? �� : ���))
		{
		list.Add(new SimpleInvariant(model, string.Format("{0} = {1}", upMicroVent.Name, microVent ? �� : ���), 
		delegate { upMicroVent.StringValue = microVent ? �� : ���; }));
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

		// ��������� ������� ��� ������� �� ������ ������� ����������
		private static List<clsProfile> getShtapikProfiles(clsModel model)
		{
		List<clsProfile> list = new List<clsProfile>();

		int thickness = getThickness(model);
		const string �������������� = "���� ����������";
		clsUserParam upRubberColor = model.UserParameters.GetParam(��������������);
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

		const string ���_������_6 = "���_������_6";
		const string standart = "M1 04-H";
		const string triplex = "3+3 �����";
		const string triplexMated = "3+3 ������� ������";
		const string sndwich24 = "COS0001.07";

		// ���������, ������ ������������� ��������
		if (!orthogonal(model))
		list.Add(new SlidingRestriction(model, string.Format("{0}, ������ ������������� �����������.", model.ProfileSystem.Name)));

		/// ����� �� �������
		/// ����������� �� ������ �����
		const int max = 4000;
		const int min = 200;

		foreach (clsBeem beem in model.Frame)
		{
		if (beem.Lenght < min || beem.Lenght > max)
		if (restriction)
		list.Add(new SlidingRestriction(model, string.Format("����� ����� ���� {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, min, max)));
		}

		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsBeem beem in leaf)
		{
		if (beem.Lenght < min || beem.Lenght > max)
		list.Add(new SlidingRestriction(model, string.Format("����� ����� ������� {3} {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, min, max, leaf)));
		}
		}

		// .Lenght - ����� ������� �� ����, .Inside_Lenght - ������� ����� ������ ����, beem.LineC1.Length - �� �����
		foreach (clsBeem beem in model.Imposts)
		if (beem.LineC1.Length < min || beem.LineC2.Length < min || beem.Lenght > max)
		if (restriction)
		list.Add(new SlidingRestriction(model, string.Format("����� ������� {0} ������� �� ���������� �������: �� {1} �� {2} ��", beem.Name, 200, max)));

		// �� �������� �������� ������ �������
		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsImpost impost in leaf.Imposts)
		{
		list.Add(new SlidingRestriction(model, string.Format("{0}, {1} � ������� �� ��������", model.ProfileSystem.Name, impost.Name)));
		break;
		}
		}

		// ����������
		foreach (clsRegion region in model.VisibleRegions)
		{
		if (region._Leaf == null)
		{
		// ��������� ������� ��� ��������
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
		list.Add(new SimpleInvariant(model, "������������ ������ ���������� �������� 4�� ��� 6�� ��� �������", delegate { region1.Fill = model.ProfileSystem.Fills[standart]; }));
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
		list.Add(new SimpleInvariant(model, string.Format("��� ������ ���������� ����� {0}, ������������ ������ ��������", 1700), delegate { region1.Fill = model.ProfileSystem.Fills[triplex]; }));
		}
		}
		}
		}
		else if (region.Fill.FillType == FillType.Sandwich)
		{
		if (region.Fill.Thikness != 24 && restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "������������ ������ �������� �������� 24��", delegate { region1.Fill = model.ProfileSystem.Fills[sndwich24]; }));
		}
		}
		else // if (region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown || region.Fill.FillType == FillType.PuzzleFill)
		{
		// ���������� �� ���_������_6
		if (region.Fill.Marking != ���_������_6)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, null, delegate { region1.Fill = model.ProfileSystem.Fills[���_������_6]; }));
		}
		}
		}
		else
		{
		// ������ � �������� , ����� ��� ������� ���-�� ������ ����� ����� ����� ������� (��� ������� .a=0 / � �������� .a >0)
		// �� ������� � �������� .a==0 ������ ������
		if (region.Fill.Marking != ���_������_6 && haveFalseImpost(region))
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, null, delegate { region1.Fill = model.ProfileSystem.Fills[���_������_6]; }));
		}
		}
		}
		else
		{
		clsLeaf leaf = region._Leaf;
		if ((region.Fill.FillType == FillType.NotFill || region.Fill.FillType == FillType.Unknown) && region.Fill.Marking != ���_������_6)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "���������� �� ���_������_6", delegate { region1.Fill = model.ProfileSystem.Fills[���_������_6]; }));
		}

		if (leaf.Height <= 1700)
		{
		if (region.Fill.Thikness != 4 && region.Fill.Thikness != 6 || region.Fill.FillType != FillType.GlassPack)
		{
		if (restriction)
		{
		clsRegion region1 = region;
		list.Add(new SimpleInvariant(model, "������������ ������ ���������� �������� 4�� ��� 6��", delegate { region1.Fill = model.ProfileSystem.Fills[standart]; }));
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
		list.Add(new SimpleInvariant(model, string.Format("��� ������ ������� ����� {0}, ������������ ������ ��������", 1700), delegate { region1.Fill = model.ProfileSystem.Fills[triplex]; }));
		}
		}
		}
		}
		}

		const int maxLeafQu = 4;
		if (model.Leafs.Count > maxLeafQu)
		list.Add(new SlidingRestriction(model, string.Format("{0} �� ����� {1} �������", model.ProfileSystem.Name, maxLeafQu)));

		if (2300 < model.Frame.Height && restriction)
		{
		list.Add(new SlidingRestriction(model, string.Format("{0}, ������������ ������ ����������� {1}", model.ProfileSystem.Name, 2300)));
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
		/// ����������� �� ���������
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

		// ������ �� ��������������
		if (!getRestriction(model))
		return list;

		foreach (clsRegion region in model.VisibleRegions)
		{
		Atechnology.winDraw.Spreading_v2.Spreading spreading = region.SpreadingV2;
		if (spreading != null)
		{
		// ��� ��� ������������ ����������
		List<Vector2F> cache = new List<Vector2F>();

		foreach (Beam beam in spreading.Beams)
		{
		if (beam is ConnectorBeam)
		continue;

		// ���� � ������� ������ ������ ������ ������ ���� �� ��������� ��������� ��������� ������ �������, �� ����� ��������
		int profileWidth = (int) Math.Round(spreading.ProfileWidth * 2);

		// ����� ����, �� ���� ��� �� ��� ������
		int length = (int) Math.Round(beam.Length);

		// ���� ������� �� �������...
		int maxLength = profileWidth <= 8 || model.ConstructionType.Name != _window ? 1000 : profileWidth <= 18 ? 1200 : 1500;

		if (length > maxLength)
		{
		list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ����� ���� = {1} (����������� ��������� {2} ��� ������� ������� {3} � ���� �����������)", region.Part, length, maxLength, spreading.Profile)));
		}

		// ��� ������� �� ������� ���������� 
		// Atechnology.winDraw.Spreading_v2.Spreading.cs:1302 connectorLeg.Length = 20.0; // ������ ������! magic numger rulez fore�a 
		// ������� ��� �������� � ��������� ���� ���� ���� ��� ���������, � ��� ����� ������� ����� ��� ����� � �����
		int minLength = profileWidth <= 8 ? 70 : 90;

		//                            if (length < minLength)
		//                            {
		//                                list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ����� ���� = {1} (��������� ��������� {2} ��� ������� ������� {3})", region.Part, length, minLength, spreading.Profile)));
		//                            }

		// ����������� ������ ������� �� ������� � ��������� ������� ��������� 
		int minRadius = profileWidth <= 8 ? 150 : profileWidth <= 18 ? 300 : int.MaxValue;
		if ((int) beam.Radius != 0 && Math.Abs(beam.Radius) < minRadius)
		{
		list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ������ ������ ���� {1:F0} (���������� ��������� {2})", region.Part, beam.Radius, minRadius)));
		}

		// �������� ���� ���������� ����� ��������� � ������
		foreach (Connector connector in beam.Connectors)
		{
		int lag = 40;

		List<IPrimitive> framePrimitives = getConnectedFramePrimitives(connector);

		// Baned
		// if (framePrimitives.Count > 1)
		//    list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ��� �� ������ ��������� � ���� �����", region.Part)));

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
		// // ����������, ����� .ComputeAngle �������� � ������������� ��������� ����������� ��� ������ ��������������� ���� ����, � ������ ������� ������ ������������ ������ ��������� ����������
		// v0.Normalize();
		// v1.Normalize();

		// �������
		double a = Angle.ComputeAngle(v1, v0);

		// �������� � ��������� [0,pi/2]
		if (a < 0)
		a += Angle.TWO_PI;

		if (a > Angle.PI)
		a -= Angle.PI;

		if (a > Angle.HALF_PI)
		a = Angle.PI - a;

		// �������
		a = Math.Round(a * 180 / Angle.PI);

		const double min = 30;
		if (a < min)
		{
		list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ���� ����� ����� ��������� � ������ = {1}� (���������� ��������� {2}�)", region.Part, a, min)));
		}
		}
		}
		}
		}
		else
		{
		// �������� ����� �� �����������,  > 8��
		if (profileWidth > 8)
		{
		// ������� ������������ 20 ��, ������ 1 ��
		foreach (Connector connectedConnector in connector.ConnectedConnectors)
		{
		Connector center = connectedConnector.Opposite;

		// ���� ������ ������ 1
		List<Connector> connected = new List<Connector>(center.ConnectedConnectors);

		// ���� ������ 1 � ��� �� ��������� �� ���������� ������, todo ������ ��� ������� ��� ������������ ����� ������� �����������, ���� ����� �������� ���������� Vector2F
		Vector2F hash = center.Coord.To2D().To2F();
		if (connected.Count > 1 && !cache.Contains(hash))
		{
		foreach (Connector center1 in connected)
		{
		// ���� ��� ����� ���������� ������ �� ���������������, ����� ����� ������ ������� ��� ���� ��� �������� � ���������� ������� �������� �� ����� ������
		Vector2F v0 = (center.Beam.Primitive.End - center.Beam.Primitive.Start).To2D().To2F();
		Vector2F v1 = (center1.Beam.Primitive.End - center1.Beam.Primitive.Start).To2D().To2F();
		// ����������, ����� .ComputeAngle �������� � ������������� ��������� ����������� ��� ������ ��������������� ���� ����, � ������ ������� ������ ������������ ������ ��������� ����������
		v0.Normalize();
		v1.Normalize();

		// ������� [-360;+360] ����� ����� ��������� ��� 89� = 90�, � ��������� ��� ������� �������, � � �������� ������ (int)
		int a = (int) Math.Round(Angle.ComputeAngle(v1, v0) * 180 / Angle.PI);

		if (a < 0)
		a += 360;

		if (a > 180)
		a = 360 - a;

		// �������� ��������������� = 90� +- 1�
		if (Math.Abs(a > 90 ? 180 - a : a % 90) > 1)
		{
		list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: �� �������������� ���� � ���� ���������� = {1}� (���������� {2}�)", region.Part, a, 90)));
		cache.Add(hash);
		break;
		}
		}

		cache.Add(hash);
		}
		}
		}
		}

		// ����� ���� ��� ����� ������� ��������
		if (length < minLength - lag)
		{
		list.Add(new SpreadingRestriction(region, string.Format("��������� {0}: ����� ���� = {1} (��������� ��������� {2} ��� ������� ������� {3})", region.Part, length + lag, minLength, spreading.Profile)));
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

		// ������� ��������� �����
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

		private const string ���������������������� = "������ ���������� ������";
		private const string �������������� = "��������� �����";

		public enum ColorType
		{
		�����,
		����������,
		unpredictable
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� ��� ����������� �/� �� ����� �������� ���� �������
		if (model.ConstructionType.Name == ��������������)
		return list;

		// ���� ����� ������ ���������� �� �����-�
		clsUserParam upManual = model.UserParameters.GetParam(����������������������);
		if (upManual != null && upManual.StringValue != upManual.DefaultValue.StrValue)
		return list;

		Color outsideColor = Color.FromArgb(model.ColorOutside.Index);
		ColorType colorTypeOutside = getColorType(model.ColorOutside.Index);

		// ��������� �����
		foreach (clsLeaf leaf in model.Leafs)
		{
		clsUserParam up = leaf.UserParameters[��������������];
		if (up == null)
		continue;

		if (colorTypeOutside != ColorType.unpredictable)
		{
		if (up.StringValue2 != colorTypeOutside.ToString())
		{
		if (leaf.IsMoskit != IsMoskit.���)
		{
		clsUserParam up1 = up;
		list.Add(new SimpleInvariant(model, string.Format("������� ���� ������� '{0}', ������������� {1} ���� ��������� �����", model.ColorOutside.ColorName, colorTypeOutside), delegate { up1.StringValue2 = colorTypeOutside.ToString(); }));
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
		if (leaf.IsMoskit != IsMoskit.���)
		list.Add(new Warning(model, string.Format("������� ���� ������� '{0}', ���� �/� `{1}`. ���������������� ���� �/� ��������������!", model.ColorOutside.ColorName, up.StringValue2)));
		}
		}

		return list;
		}

		private static ColorType getColorType(int color)
		{
		// ������� 300 = ��������, 120 �������
		// http://www.colory.ru/colorwheel/
		// https://en.wikipedia.org/wiki/HSL_and_HSV#/media/File:HSV-RGB-comparison.svg
		float hue = Color.FromArgb(color).GetHue();

		float b = Color.FromArgb(color).GetBrightness();

		if (b > 0.89)
		return ColorType.�����;

		if (Color.FromArgb(color).GetSaturation() < 0.15)
		return ColorType.����������;

		// ���� ������� �������� �� ��
		// return !(hue > 120 && hue < 300);
		return !(hue > 90 && hue < 330) ? ColorType.���������� : ColorType.unpredictable;
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
		// ������� �-1 �� �-1 ��� �� �������� �������� �-1
		clsRegion region = subject as clsRegion;
		return region != null ? string.Format("���������� {0} {1}", region.Part, _message) : _message;
		}

		public static IEnumerable<Invalidance> test(clsModel model)
		{
		List<Invalidance> list = new List<Invalidance>(0);

		// ���� �������
		if (!profSystemList.Contains(model.ProfileSystem.Name))
		return list;

		// ����� ��������
		tryFixProfile(model);

		bool restriction = getRestriction(model);

		int idseller = getIdseller(model);

		// ������ ��������, ��� � ����� ������ ����� ������ ���������, �� ���� ������������� ������ ������ � ������ ������� ����� ����� ����������� ���� ����� ����
		if (restriction && model.ProfileSystem.Name == Eco60)
		{
		list.Add(new DeceuninckRestriction(model, string.Format("���������� ������� {0} �� ��������", Eco60)));
		}

		// ��������� ������ �������� �� ���� ��� ���������
		foreach (clsFrame frame in model.Frame)
		{
		if (frame.bType == ComponentType.Porog && frame.ConnectType != ConnectType.�������)
		{
		// C# 2.0 behaviour
		clsFrame frame1 = frame;
		list.Add(new SimpleInvariant(model, null, delegate { frame1.ConnectType = ConnectType.�������; }));
		}
		}

		// ��������� === Classic | ������� | ���*
		if (model.FurnitureSystem.Name != FurnSystBEZ)
		{
		if (_outdoor == model.ConstructionType.Name)
		{
		if (FurnSystDver != model.FurnitureSystem.Name)
		list.Add(new SimpleInvariant
		(model,
		null, //string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, furnSystemList[0]),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(FurnSystDver); }
		));
		}
		else if (_pskportal == model.ConstructionType.Name)
		{
		// SELECT * FROM system WHERE name LIKE '��������� PSK%'
		if (!model.FurnitureSystem.Name.StartsWith("��������� PSK"))
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
		string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, Vorne),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(Vorne); }
		));
		}
		}
		else if (!furnSystemList.Contains(model.FurnitureSystem.Name))
		{
		list.Add(new SimpleInvariant
		(model,
		string.Format("� ����������� {0} ������������ {1}", model.ProfileSystem.Name, furnSystemList[0]),
		delegate { model.FurnitureSystem = model.WinDraw.FurnitureSystems.FromName(furnSystemList[0]); }
		));
		}
		}
		}

		// ������� ���������� === 0 | 4 | 24 | 32 | 42
		List<int> avaliableThicknesses = getAvaliableThicknesses(model);

		if (avaliableThicknesses.Count > 0)
		{
		foreach (clsRegion region in model.VisibleRegions)
		{
		//region.Fill.Thikness != 4 && region.Fill.Thikness != 24 && region.Fill.Thikness != 32 && 
		if (region.Fill.Thikness > 0 && !avaliableThicknesses.Contains(region.Fill.Thikness))
		{
		list.Add(new DeceuninckRestriction(region, string.Format("� ����������� {0} ����� ������������ ������ ���������� �������� {1} ��", model.ProfileSystem.Name, string.Join(", ", avaliableThicknesses.ConvertAll<string>(delegate(int input) { return input.ToString(); }).ToArray()))));
		}
		}
		}
		else
		{
		list.Add(new DeceuninckRestriction(model, string.Format("�� ������� �������� ��������� ������ ���������� ��� ������� {0}. ��������� ���� Thikness � ���������� ������", model.ProfileSystem.Name)));
		}

		// ������ ��������� �����! ������ // ���� �� ���������� �� ������������ ��� ����� �������� �� ������� 
		const string upShtapicName = "������";
		const string upColorName = "���� ����������";
		const string skew = "���������";
		string rightRubberColor = model.ProfileSystem.Name == BAUTEKmass ? "������" : "�����";
		clsUserParam upShtapic = model.UserParameters.GetParam(upShtapicName);
		clsUserParam upColor = model.UserParameters.GetParam(upColorName);
		if ((upShtapic != null && upShtapic.StringValue != skew) || (upColor != null && upColor.StringValue != rightRubberColor))
		{
		list.Add(new SimpleInvariant(
		model,
		string.Format("� ����������� {0} ������������ ������ {1} ������ � {2} ���� ����������", model.ProfileSystem.Name, skew, rightRubberColor),
		delegate
		{
		model.UserParameters[upShtapicName].StringValue = skew;
		model.UserParameters[upColorName].StringValue = rightRubberColor;
		}));
		}

		// ��� ����
		{
		clsBeem beem = getArcBeem(model);
		if (beem != null)
		{
		if (restriction)
		list.Add(new DeceuninckRestriction(beem, string.Format("{0}, ���� ����������", beem.Name)));
		}
		}

		// �� ������ �� �������� ������� ������ // ����� �� RehauProfileRestiction // ���� ��������
		foreach (clsBeem beem in model.Frame)
		{
		if (beem.BalkaType == ModelPart.Porog)
		{
		foreach (clsBeem impost in beem.ConnectedImposts)
		{
		if (impost.BalkaType == ModelPart.Impost)
		{
		list.Add(new RehauProfileRestriction(model, "�� �������� ���������� ������ �� �����"));
		break;
		}
		}
		}
		}

		return list;
		}

		/// ���������� /D /P � ������ string.Empty, ���� ����� ECO 60 � ����������
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

		// �.�. ��� ��� Thickness, f1, f2 ������� ����� � �������� ����
		ProfileSystem profileSystem = SettingsLoad.currentSettings.GetProfileSystemByName(model.ProfileSystem.Name);

		if (profileSystem != null)
		{
		// ��������� ��������� ��� ������� ����������� ������ �������� (� ����� ��� ��� ����������� ������ � ������ ����������, ��� ������������ ���������� � ������ ��������)
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
		list.Add(new InactiveProfileRestriction(beem, string.Format("������� {0} �� �������", beem.Profile.Marking)));
		}
		}

		foreach (clsLeaf leaf in model.Leafs)
		{
		foreach (clsBeem beem in leaf)
		{
		if (beem.Profile.Marking.StartsWith("554012"))
		{
		list.Add(new InactiveProfileRestriction(beem, string.Format("������� {0} �� �������", beem.Profile.Marking)));
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
