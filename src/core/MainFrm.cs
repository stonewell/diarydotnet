using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Diary.Net.DB;
using Diary.Net.Tree;
using System.Collections;
using System.Data.Common;
using Diary.Net.ListView;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace Diary.Net
{
	public partial class MainFrm : Form
	{
#if !__MonoCS__
		[DllImport("user32.dll")]
			private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint vk);
		[DllImport("user32.dll")]
			private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[DllImport("kernel32.dll")]
			private static extern int GlobalAddAtom(String atomString);
		[DllImport("kernel32.dll")]
			private static extern int GlobalDeleteAtom(int atom);
		private int hotKeyAtom_ = 0;
#endif

		private static readonly Hashtable yearNodes_ = new Hashtable();

		private DateTime currentDay_ = DateTime.Now;

		private ArrayList deletedAttachments_ = new ArrayList();
		private ArrayList insertedAttachments_ = new ArrayList();

		public MainFrm()
		{
			InitializeComponent();
		}

		private void MainFrm_Load(object sender, EventArgs e)
		{
			scNaviContent.Panel1Collapsed = false;
			statusStrip1.Visible = false;
			scContents.Panel2Collapsed = true;
			scMainOthers.Panel2Collapsed = true;

#if !__MonoCS__
			hotKeyAtom_ = GlobalAddAtom("Ctrl+Alt+a");
			RegisterHotKey(this.Handle, hotKeyAtom_, 0x3, 'A');
#endif
			richTextBox.Modified = false;

			if (!DoLogin())
			{
#if !__MonoCS__
				Application.Exit();
#else
				System.Environment.Exit(0);
#endif
			}
		}

		private void LoadDialyNotes()
		{
			tvwDiary.Nodes.Clear();

			DbDataAdapter adapter =
				DBManager.CreateDiaryDataAdapter(Program.DbProvideFactory, Program.DbConnection);

			adapter.Fill(Program.DiaryNetDS.DiaryNotes);

			foreach (DiaryNetDS.DiaryNotesRow row in
					Program.DiaryNetDS.DiaryNotes)
			{
				DayTreeNode node = AddDayNode(row.Note_Date, row.ID);
				node.Commit();
			}
		}

		private DayTreeNode AddDayNode(DateTime date, int id)
		{
			YearTreeNode yearNode = GetYearNode(date.Year);

			if (yearNode == null)
				yearNode = AddYearNode(date.Year);

			MonthTreeNode monthNode = yearNode.GetMonthNode(date.Month);

			if (monthNode == null)
				monthNode = yearNode.AddMonthNode(date.Month);

			DayTreeNode dayNode = monthNode.GetDayNode(date.Day);

			if (dayNode == null)
			{
				dayNode = monthNode.AddDayNode(date.Day, id);
				dayNode.ContextMenuStrip = dayNodeContextMenuStrip;
			}
			else
			{
				dayNode.ID = id;
			}

			yearNode.Expand();
			monthNode.Expand();

			return dayNode;
		}

		private YearTreeNode AddYearNode(int year)
		{
			if (yearNodes_.ContainsKey(year))
			{
				return yearNodes_[year] as YearTreeNode;
			}

			YearTreeNode node = new YearTreeNode(year);

			bool bInserted = false;

			foreach (YearTreeNode yNode in tvwDiary.Nodes)
			{
				if (yNode.Year > year)
				{
					bInserted = true;
					tvwDiary.Nodes.Insert(yNode.Index, node);
					break;
				}
			}

			if (!bInserted)
			{
				tvwDiary.Nodes.Add(node);
			}

			yearNodes_[year] = node;
			return node;
		}

		private YearTreeNode GetYearNode(int year)
		{
			if (yearNodes_.ContainsKey(year))
				return yearNodes_[year] as YearTreeNode;

			return null;
		}

		private void UpdateViewMenu()
		{
			navigationPaneToolStripMenuItem.Checked =
				!scNaviContent.Panel1Collapsed;
			statusBarToolStripMenuItem.Checked =
				statusStrip1.Visible;
			attachmentToolStripMenuItem.Checked =
				!scContents.Panel2Collapsed;
			findResultToolStripMenuItem.Checked =
				!scMainOthers.Panel2Collapsed;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void navigationPaneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			scNaviContent.Panel1Collapsed =
				!scNaviContent.Panel1Collapsed;
			UpdateViewMenu();
		}

		private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip1.Visible = !statusStrip1.Visible;
			UpdateViewMenu();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBoxFrm frm = new AboutBoxFrm();

			frm.ShowDialog(this);
		}

		private void attachmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			scContents.Panel2Collapsed =
				!scContents.Panel2Collapsed;
			UpdateViewMenu();
		}

		private void selectDateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectDateFrm frm = new SelectDateFrm();

			frm.SelectedDate = currentDay_;

			if (DialogResult.OK == frm.ShowDialog(this))
			{
				ChangeToDate(frm.SelectedDate);
			}
		}

		private bool DoesSaveModified()
		{
			if (!richTextBox.Modified &&
					!IsAttachmentsModified() &&
					!IsDocumentsNodeModified() &&
					!IsDiaryNodeModified())
			{
				return false;
			}

			return DialogResult.Yes ==
				MessageBox.Show(this,
						"There is modification, Save it before change?",
						"Save Modification",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tcNavigation.SelectedIndex == 0)
			{
				SaveCurrentDiaryNote();
			}
			else
			{
				SaveCurrentDocument();
			}
		}

		private void SaveCurrentDiaryNote()
		{
			DayTreeNode dayNode = GetDayNode(currentDay_);

			using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
			{
				Program.DiaryNetDS.AcceptChanges();

				try
				{
					DiaryNetDS.DiaryNotesRow row = null;

					if (dayNode != null)
					{
						row = Program.DiaryNetDS.DiaryNotes.FindByID(dayNode.ID);
					}

					bool bNew = false;

					if (row == null)
					{
						row = Program.DiaryNetDS.DiaryNotes.NewDiaryNotesRow();
						bNew = true;
					}
					else
						row.BeginEdit();

					row.Modify_Date = DateTime.Now;

					if (bNew)
					{
						row.Binary_ID = -1;
						row.Text_ID = -1;
						row.Note_Date = currentDay_;

						Program.DiaryNetDS.DiaryNotes.AddDiaryNotesRow(row);
					}
					else
					{
						row.EndEdit();
					}

					row.BeginEdit();
					if (richTextBox.Modified)
					{
						row.Binary_ID =
							DBManager.SaveBinary(Program.DbConnection,
									row.Binary_ID,
									richTextBox.Rtf);
						row.Text_ID = DBManager.SaveText(Program.DbConnection,
								row.Text_ID,
								row.ID,
								false,
								"",
								richTextBox.Text);
					}
					row.EndEdit();

					if (IsAttachmentsModified())
					{
						SaveAttachments(row.ID, true);
					}

					DBManager.CreateDiaryDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.DiaryNotes);
					DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.Attachments);

					dbTrans.Commit();

					dayNode = AddDayNode(row.Note_Date, row.ID);
					dayNode.Commit();

					richTextBox.Modified = false;

					tvwDiary.SelectedNode = dayNode;
				}
				catch (Exception ex)
				{
					dbTrans.Rollback();
					Program.DiaryNetDS.RejectChanges();
					MessageBox.Show(this, ex.Message, "Error",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
				}
			}
		}

		private DayTreeNode GetDayNode(DateTime currentDay)
		{
			if (yearNodes_.ContainsKey(currentDay.Year))
			{
				YearTreeNode yNode = yearNodes_[currentDay.Year] as YearTreeNode;

				MonthTreeNode mNode = yNode.GetMonthNode(currentDay.Month);

				if (mNode != null)
				{
					return mNode.GetDayNode(currentDay.Day);
				}
			}

			return null;
		}

		private void tvwDiary_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DayTreeNode)
			{
				DayTreeNode node = e.Node as DayTreeNode;

				UpdateContentPane(node);
			}
		}

		private void UpdateContentPane(DayTreeNode node)
		{
			currentDay_ = new DateTime(node.Year, node.Month, node.Day);

			DiaryNetDS.DiaryNotesRow row =
				Program.DiaryNetDS.DiaryNotes.FindByID(node.ID);

			if (row == null)
			{
				ClearContentPane();
			}
			else
			{
				string rtf = DBManager.GetBinary(Program.DbConnection, row.Binary_ID);
				richTextBox.Rtf = rtf;

				UpdateAttachmentsListView(row.ID, true);
			}

			richTextBox.Modified = false;
			richTextBox.Focus();
		}

		private void tvwDiary_MouseClick(object sender, MouseEventArgs e)
		{
			TreeViewHitTestInfo info = tvwDiary.HitTest(e.Location);

			if (info.Node is DayTreeNode)
			{
				if (e.Button == MouseButtons.Right)
				{
					tvwDiary.SelectedNode = info.Node;
				}
			}
		}

		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DoLogin();
		}

		private bool DoLogin()
		{
			HandleModifiedState();

			LoginFrm frm = new LoginFrm();

			if (frm.ShowDialog(this) == DialogResult.OK)
			{
				InitializeData();
				return true;
			}

			return false;
		}

		private void InitializeData()
		{
			UpdateViewMenu();

			yearNodes_.Clear();
			currentDay_ = DateTime.Now;
			deletedAttachments_.Clear();
			insertedAttachments_.Clear();

			Program.DiaryNetDS = new DiaryNetDS();

			LoadAttachements();
			LoadDialyNotes();
			LoadDocuments();

			ChangeToDate(DateTime.Today);

			RefreshView();

			Program.DiaryNetDS.AcceptChanges();

#if __MonoCS__
			DBManager.UpdateAutoIncrementSeed(Program.DiaryNetDS);
#endif
		}

		private void ChangeToDate(DateTime date)
		{
			currentDay_ = date;

			tcNavigation.SelectedIndex = 0;

			DayTreeNode node = GetDayNode(currentDay_);

			if (node != null)
			{
				tvwDiary.SelectedNode = node;
			}
			else
			{
				tvwDiary.SelectedNode = AddDayNode(currentDay_, -1);
			}
		}

		private void LoadAttachements()
		{
			DbDataAdapter adapter =
				DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory, Program.DbConnection);

			adapter.Fill(Program.DiaryNetDS.Attachments);
		}

		private bool IsAttachmentsModified()
		{
			return deletedAttachments_.Count > 0 ||
				insertedAttachments_.Count > 0;
		}

		private void SaveAttachments(int ref_ID, bool isNotes)
		{
			foreach (AttachmentViewItem item in deletedAttachments_)
			{
				DiaryNetDS.AttachmentsRow row = item.Row;

				if (row != null)
				{
					DBManager.DeleteBinary(Program.DbConnection, row.Binary_ID);
					row.Delete();
				}
			}

			foreach (AttachmentViewItem item in insertedAttachments_)
			{
				DiaryNetDS.AttachmentsRow row =
					Program.DiaryNetDS.Attachments.AddAttachmentsRow(isNotes ? '1' : '0',
							ref_ID, item.FileName, -1);

				row.BeginEdit();
				row.Binary_ID =
					DBManager.SaveBinary(Program.DbConnection, -1,
							GetAttachmentContent(item.FileName));
				row.EndEdit();
			}

			deletedAttachments_.Clear();
			insertedAttachments_.Clear();
		}

		private string GetAttachmentContent(string filename)
		{
			using (FileStream sr = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				byte[] buf = new byte[1024];

				int count = 0;

				using (StringWriter sw = new StringWriter())
				{
					while ((count = sr.Read(buf, 0, 1024)) > 0)
					{
						sw.WriteLine(Convert.ToBase64String(buf, 0, count));
					}

					return sw.ToString();
				}
			}
		}

		private void UpdateAttachmentsListView(int ref_ID, bool isNotes)
		{
			deletedAttachments_.Clear();
			insertedAttachments_.Clear();
			lvwAttachments.Clear();

			lvwAttachments.Columns.Add(columnHeader1);

			DataView view = new DataView(Program.DiaryNetDS.Attachments,
					"REF_ID =" + ref_ID + " AND Is_Notes='" + (isNotes ? "1" : "0") + "'",
					"ID",
					DataViewRowState.CurrentRows);

			for (int i = 0; i < view.Count; i++)
			{
				DiaryNetDS.AttachmentsRow row =
					view[i].Row as DiaryNetDS.AttachmentsRow;

				AttachmentViewItem item = new AttachmentViewItem(row);

				item.ImageKey = GetImageKey(row.FileName);

				lvwAttachments.Items.Add(item);
			}

			scContents.Panel2Collapsed = lvwAttachments.Items.Count == 0;

			UpdateLvwAttachmentsMenu();
			UpdateViewMenu();
		}

		private string GetImageKey(string filename)
		{
			string ext = Path.GetExtension(filename);

			if (ext.Length == 0)
				ext = "noextension";

			Icon iconForFile = SystemIcons.Question;

			if (File.Exists(filename))
			{
				iconForFile =
					System.Drawing.Icon.ExtractAssociatedIcon(filename);
			}
			else
			{
				filename = Path.GetTempPath();

				if (!filename.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					filename += Path.DirectorySeparatorChar;
				}

				filename += "1" + ext;

				try
				{
					if (!File.Exists(filename))
					{
						FileStream fs = File.Create(filename);
						fs.Close();
					}

					iconForFile = Icon.ExtractAssociatedIcon(filename);
				}
				finally
				{
					try
					{
						File.Delete(filename);
					}
					catch
					{
					}
				}
			}

			if (!imageListSmall.Images.ContainsKey(ext))
			{
				imageListSmall.Images.Add(ext, iconForFile);
			}

			if (!imageListLarge.Images.ContainsKey(ext))
			{
				imageListLarge.Images.Add(ext, iconForFile);
			}

			return ext;
		}

		private void deleteNotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tvwDiary.InvokeRequired)
			{
				tvwDiary.Invoke(new EventHandler(deleteNotesToolStripMenuItem_Click));
			}
			else
			{
				if (tvwDiary.SelectedNode is DayTreeNode)
				{
					DayTreeNode node = tvwDiary.SelectedNode as DayTreeNode;
					DateTime dt =
						new DateTime(node.Year, node.Month, node.Day);

					if (DialogResult.Yes != MessageBox.Show(this,
								"Delete the notes for " + dt.ToLongDateString() + "?",
								"Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					{
						return;
					}

					DiaryNetDS.DiaryNotesRow row =
						Program.DiaryNetDS.DiaryNotes.FindByID(node.ID);

					if (row == null)
						return;

					using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
					{
						Program.DiaryNetDS.AcceptChanges();
						try
						{
							DBManager.DeleteBinary(Program.DbConnection,
									row.Binary_ID);
							DBManager.DeleteText(Program.DbConnection,
									row.ID,
									row.Text_ID,
									false);

							//Delete Attachments
							DataView view = new DataView(Program.DiaryNetDS.Attachments,
									"REF_ID =" + row.ID + " AND Is_Notes='1'",
									"ID",
									DataViewRowState.CurrentRows);

							while (view.Count > 0)
							{
								DiaryNetDS.AttachmentsRow attachmentRow =
									view[0].Row as DiaryNetDS.AttachmentsRow;

								DBManager.DeleteBinary(Program.DbConnection,
										attachmentRow.Binary_ID);

								attachmentRow.Delete();
							}

							//Delete Notes
							row.Delete();

							DBManager.CreateDiaryDataAdapter(Program.DbProvideFactory,
									Program.DbConnection).Update(Program.DiaryNetDS.DiaryNotes);
							DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory,
									Program.DbConnection).Update(Program.DiaryNetDS.Attachments);

							dbTrans.Commit();

							ClearContentPane();

							node.Remove();
						}
						catch (Exception ex)
						{
							dbTrans.Rollback();
							Program.DiaryNetDS.RejectChanges();
							MessageBox.Show(this, ex.Message, "Error",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
						}
					}
				}
			}//if invoke required
		}

		private void deleteAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArrayList results = new ArrayList();

			foreach (AttachmentViewItem item in lvwAttachments.SelectedItems)
			{
				if (insertedAttachments_.Contains(item))
				{
					insertedAttachments_.Remove(item);
				}
				else
				{
					deletedAttachments_.Add(item);
				}

				results.Add(item);
			}

			foreach (AttachmentViewItem item in results)
				item.Remove();
		}

		private void addAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == openFileDialog1.ShowDialog(this))
			{
				AttachmentViewItem item = new AttachmentViewItem(null);
				item.FileName = openFileDialog1.FileName;
				item.ImageKey = GetImageKey(item.FileName);

				lvwAttachments.Items.Add(item);

				insertedAttachments_.Add(item);

				scContents.Panel2Collapsed = lvwAttachments.Items.Count == 0;

				UpdateLvwAttachmentsMenu();
				UpdateViewMenu();
			}
		}

		private void removeAllAttachmentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (AttachmentViewItem item in lvwAttachments.Items)
			{
				if (insertedAttachments_.Contains(item))
				{
					insertedAttachments_.Remove(item);
				}
				else
				{
					deletedAttachments_.Add(item);
				}
			}

			lvwAttachments.Items.Clear();
		}

		private void lvwAttachments_MouseClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo info =
				lvwAttachments.HitTest(e.Location);

			if (e.Button == MouseButtons.Right)
			{
				info.Item.Selected = true;
			}
		}

		private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			HandleModifiedState();

#if !__MonoCS__
			UnregisterHotKey(this.Handle, hotKeyAtom_);
			GlobalDeleteAtom(hotKeyAtom_);
#endif
		}

		private void detailedViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lvwAttachments.View = View.Details;
			UpdateLvwAttachmentsMenu();
		}

		private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lvwAttachments.View = View.LargeIcon;
			UpdateLvwAttachmentsMenu();
		}

		private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lvwAttachments.View = View.SmallIcon;
			UpdateLvwAttachmentsMenu();
		}

		private void listViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lvwAttachments.View = View.List;
			UpdateLvwAttachmentsMenu();
		}

		private void tileViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lvwAttachments.View = View.Tile;
			UpdateLvwAttachmentsMenu();
		}

		private void UpdateLvwAttachmentsMenu()
		{
			detailedViewToolStripMenuItem.Checked = false;
			largeIconToolStripMenuItem.Checked = false;
			smallIconToolStripMenuItem.Checked = false;
			listViewToolStripMenuItem.Checked = false;
			tileViewToolStripMenuItem.Checked = false;

			switch (lvwAttachments.View)
			{
				case View.List:
					listViewToolStripMenuItem.Checked = true;
					break;
				case View.Details:
					detailedViewToolStripMenuItem.Checked = true;
					break;
				case View.LargeIcon:
					largeIconToolStripMenuItem.Checked = true;
					break;
				case View.SmallIcon:
					smallIconToolStripMenuItem.Checked = true;
					break;
				case View.Tile:
					tileViewToolStripMenuItem.Checked = true;
					break;
				default:
					break;
			}
		}

		private void lvwAttachments_Resize(object sender, EventArgs e)
		{
			columnHeader1.Width = lvwAttachments.Width - 20;
		}

		private void saveAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lvwAttachments.SelectedItems.Count == 0)
				return;

			if (DialogResult.OK == folderBrowserDialog1.ShowDialog(this))
			{
				foreach (AttachmentViewItem item in lvwAttachments.SelectedItems)
				{
					SaveAttachmentFile(folderBrowserDialog1.SelectedPath,
							item.FileName,
							item.Row);
				}
			}
		}

		private void SaveAttachmentFile(string path, string filename,
				DiaryNetDS.AttachmentsRow attachmentsRow)
		{
			string fullname = path + Path.DirectorySeparatorChar + Path.GetFileName(filename);

			if (File.Exists(fullname))
			{
				if (DialogResult.No == MessageBox.Show(this,
							"File " + fullname + " is already exists, overwrite?",
							"File existing warning",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Warning))
				{
					return;
				}
			}

			string binary = null;

			if (attachmentsRow == null)
			{
				if (File.Exists(filename))
					binary = GetAttachmentContent(filename);
				else
				{
					binary = "";
				}
			}
			else
			{
				binary =
					DBManager.GetBinary(Program.DbConnection, attachmentsRow.Binary_ID);
			}

			using (FileStream fs = new FileStream(fullname, FileMode.OpenOrCreate))
			{
				using (StringReader sr = new StringReader(binary))
				{
					string line = null;

					while ((line = sr.ReadLine()) != null)
					{
						byte[] buf = Convert.FromBase64String(line);

						fs.Write(buf, 0, buf.Length);
					}
				}
			}
		}

		private void tcNavigation_Deselected(object sender, TabControlEventArgs e)
		{
			HandleModifiedState();
		}

		private void tcNavigation_Selected(object sender, TabControlEventArgs e)
		{
			RefreshView();
		}

		private void RefreshView()
		{
			if (tcNavigation.SelectedIndex == 0)
			{
				if (tvwDiary.SelectedNode is DayTreeNode)
				{
					UpdateContentPane(tvwDiary.SelectedNode as DayTreeNode);
				}
				else
				{
					ChangeToDate(currentDay_);
				}
			}
			else
			{
				UpdateContentPane(tvwDocuments.SelectedNode as DocumentTreeNode);
			}
		}

		private void UpdateContentPane(DocumentTreeNode node)
		{
			if (node == null)
			{
				ClearContentPane();

				return;
			}

			DiaryNetDS.DocumentsRow row =
				Program.DiaryNetDS.Documents.FindByID(node.ID);

			if (row == null)
			{
				ClearContentPane();
			}
			else
			{
				richTextBox.Rtf = DBManager.GetBinary(Program.DbConnection,
						row.Binary_ID);

				UpdateAttachmentsListView(row.ID, false);
			}

			richTextBox.Focus();
			richTextBox.Modified = false;
		}

		private void ClearContentPane()
		{
			richTextBox.Text = "";
			richTextBox.Modified = false;
			UpdateAttachmentsListView(-1, false);
		}

		private void todayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ChangeToDate(DateTime.Today);
		}

		private void newDocumentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewDocumentFrm frm = new NewDocumentFrm();

			if (DialogResult.OK == frm.ShowDialog(this))
			{
				DocumentTreeNode node = new DocumentTreeNode();
				node.Title = frm.Title;
				node.ContextMenuStrip = docNodeContextMenuStrip;

				tvwDocuments.Nodes.Add(node);

				scNaviContent.Panel1Collapsed = false;

				UpdateViewMenu();

				tcNavigation.SelectedIndex = 1;

				tvwDocuments.SelectedNode = node;

				node.UpdateText();
			}
		}

		private void tvwDocuments_MouseClick(object sender, MouseEventArgs e)
		{
			TreeViewHitTestInfo info = tvwDocuments.HitTest(e.Location);

			if (info.Node is DocumentTreeNode)
			{
				if (e.Button == MouseButtons.Right)
				{
					tvwDocuments.SelectedNode = info.Node;
				}
			}
		}

		private void tvwDocuments_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DocumentTreeNode)
			{
				DocumentTreeNode node = e.Node as DocumentTreeNode;

				UpdateContentPane(node);
			}
		}

		private void SaveCurrentDocument()
		{
			if (!(tvwDocuments.SelectedNode is DocumentTreeNode))
			{
				return;
			}

			DocumentTreeNode docNode = tvwDocuments.SelectedNode as DocumentTreeNode;

			using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
			{
				Program.DiaryNetDS.AcceptChanges();
				try
				{
					DiaryNetDS.DocumentsRow row =
						Program.DiaryNetDS.Documents.FindByID(docNode.ID);

					if (row == null)
					{
						row = Program.DiaryNetDS.Documents.NewDocumentsRow();
						row.Binary_ID = -1;
						row.Text_ID = -1;
						row.Title = docNode.Title;
						row.Create_Date = currentDay_;
						row.Modify_Date = DateTime.Now;
						Program.DiaryNetDS.Documents.AddDocumentsRow(row);
					}
					else
					{
						row.BeginEdit();
						row.Modify_Date = DateTime.Now;
						row.Title = docNode.Title;
						row.EndEdit();
					}

					if (richTextBox.Modified)
					{
						row.BeginEdit();
						row.Binary_ID =
							DBManager.SaveBinary(Program.DbConnection,
									row.Binary_ID,
									richTextBox.Rtf);
						row.Text_ID = DBManager.SaveText(Program.DbConnection,
								row.Text_ID,
								row.ID,
								true,
								docNode.Title,
								richTextBox.Text);
						row.EndEdit();
					}

					if (IsAttachmentsModified())
					{
						SaveAttachments(row.ID, false);
					}

					DBManager.CreateDocumentsDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.Documents);
					DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.Attachments);

					dbTrans.Commit();

					richTextBox.Modified = false;
					docNode.ID = row.ID;
					docNode.Commit();
				}
				catch (Exception ex)
				{
					dbTrans.Rollback();
					Program.DiaryNetDS.RejectChanges();
					MessageBox.Show(this, ex.Message, "Error",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
				}
			}
		}

		private void LoadDocuments()
		{
			tvwDocuments.Nodes.Clear();

			DbDataAdapter adapter =
				DBManager.CreateDocumentsDataAdapter(Program.DbProvideFactory, Program.DbConnection);

			adapter.Fill(Program.DiaryNetDS.Documents);

			foreach (DiaryNetDS.DocumentsRow row in
					Program.DiaryNetDS.Documents)
			{
				DocumentTreeNode node = new DocumentTreeNode();
				node.Title = row.Title;
				node.ID = row.ID;
				node.ContextMenuStrip = docNodeContextMenuStrip;
				node.Commit();

				tvwDocuments.Nodes.Add(node);
			}
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] formats = new string[] {
				DataFormats.Rtf,
					DataFormats.Html,
					DataFormats.UnicodeText,
					DataFormats.Text,
					DataFormats.Bitmap,
					DataFormats.CommaSeparatedValue,
					DataFormats.Dib,
					DataFormats.Dif,
					DataFormats.EnhancedMetafile,
					DataFormats.FileDrop,
					DataFormats.MetafilePict,
					DataFormats.Palette,
					DataFormats.PenData,
					DataFormats.Riff,
					DataFormats.Serializable,
					DataFormats.StringFormat,
					DataFormats.SymbolicLink,
					DataFormats.Tiff,
					DataFormats.WaveAudio,
			};

			foreach (string format in formats)
			{
				if (Clipboard.ContainsData(format))
				{
					//if (format.Equals(DataFormats.Html))
					//{
					//    try
					//    {
					//        object r = Html2Rtf.Html2Rtf.Clipboard2Rtf();

					//        if (r != null)
					//        {
					//            richTextBox.SelectedRtf = r.ToString();
					//            break;
					//        }
					//    }
					//    catch (Exception ex)
					//    {
					//        MessageBox.Show(this,
					//            "Paste from Html Error:" + ex.Message +
					//            "\n" + ex.StackTrace,
					//            "Error",
					//            MessageBoxButtons.OK,
					//            MessageBoxIcon.Error);
					//    }
					//}
					//else
					//{
					//if (richTextBox.CanPaste(DataFormats.GetFormat(format)))
					//{
					//    richTextBox.Paste(DataFormats.GetFormat(format));
					//    break;
					//}
					//}
					Font f = richTextBox.Font;
					Color fc = richTextBox.ForeColor;
					Color bc = richTextBox.BackColor;

					richTextBox.Paste();
					richTextBox.SelectionFont = f;
					richTextBox.SelectionColor = fc;
					richTextBox.SelectionBackColor = bc;
				}
			}
		}

		private void removeAllDocumentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tvwDocuments.InvokeRequired)
			{
				tvwDocuments.Invoke(new EventHandler(removeAllDocumentsToolStripMenuItem_Click));
			}
			else
			{
				if (DialogResult.Yes != MessageBox.Show(this,
							"Delete the all documents?",
							"Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
				{
					return;
				}

				using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
				{
					Program.DiaryNetDS.AcceptChanges();
					try
					{
						foreach (DocumentTreeNode node in tvwDocuments.Nodes)
						{
							DeleteDocument(node);
						}

						dbTrans.Commit();

						tvwDocuments.Nodes.Clear();

						ClearContentPane();
					}
					catch (Exception ex)
					{
						dbTrans.Rollback();
						Program.DiaryNetDS.RejectChanges();
						MessageBox.Show(this, ex.Message, "Error",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
					}
				}//using
			}//if invoke required
		}

		private void editTitleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tvwDocuments.InvokeRequired)
			{
				tvwDocuments.Invoke(new EventHandler(deleteDocumentToolStripMenuItem_Click));
			}
			else
			{
				if (tvwDocuments.SelectedNode is DocumentTreeNode)
				{
					DocumentTreeNode node = tvwDocuments.SelectedNode as DocumentTreeNode;
					NewDocumentFrm frm = new NewDocumentFrm();
					frm.Title = node.Title;

					if (DialogResult.OK == frm.ShowDialog(this))
					{
						if (!node.Title.Equals(frm.Title))
						{
							node.Title = frm.Title;
						}
					}
				}
			}
		}

		private void deleteDocumentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tvwDocuments.InvokeRequired)
			{
				tvwDocuments.Invoke(new EventHandler(deleteDocumentToolStripMenuItem_Click));
			}
			else
			{
				if (tvwDocuments.SelectedNode is DocumentTreeNode)
				{
					DocumentTreeNode node = tvwDocuments.SelectedNode as DocumentTreeNode;

					if (DialogResult.Yes != MessageBox.Show(this,
								"Delete the document of " + node.Title + "?",
								"Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					{
						return;
					}

					DiaryNetDS.DocumentsRow row =
						Program.DiaryNetDS.Documents.FindByID(node.ID);

					if (row == null)
					{
						node.Remove();
						return;
					}

					using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
					{
						Program.DiaryNetDS.AcceptChanges();
						try
						{
							DeleteDocument(node);

							dbTrans.Commit();

							ClearContentPane();

							node.Remove();
						}
						catch (Exception ex)
						{
							dbTrans.Rollback();
							Program.DiaryNetDS.RejectChanges();
							MessageBox.Show(this, ex.Message, "Error",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
						}
					}
				}
			}//if invoke required
		}

		private static void DeleteDocument(DocumentTreeNode node)
		{
			DiaryNetDS.DocumentsRow row =
				Program.DiaryNetDS.Documents.FindByID(node.ID);

			if (row == null)
				return;

			DBManager.DeleteBinary(Program.DbConnection,
					row.Binary_ID);
			DBManager.DeleteText(Program.DbConnection,
					row.ID,
					row.Text_ID,
					true);

			//Delete Attachments
			DataView view = new DataView(Program.DiaryNetDS.Attachments,
					"REF_ID =" + row.ID + " AND Is_Notes='0'",
					"ID",
					DataViewRowState.CurrentRows);

			while (view.Count > 0)
			{
				DiaryNetDS.AttachmentsRow attachmentRow =
					view[0].Row as DiaryNetDS.AttachmentsRow;

				DBManager.DeleteBinary(Program.DbConnection,
						attachmentRow.Binary_ID);
				attachmentRow.Delete();
			}

			//Delete Document
			row.Delete();

			DBManager.CreateDocumentsDataAdapter(Program.DbProvideFactory,
					Program.DbConnection).Update(Program.DiaryNetDS.Documents);
			DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory,
					Program.DbConnection).Update(Program.DiaryNetDS.Attachments);
		}

		private void tvwDocuments_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			HandleModifiedState();
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_HOTKEY = 0x0312;
			// Listen for operating system messages.
			switch (m.Msg)
			{
				case WM_HOTKEY:
					Visible = true;
					this.Activate();
					break;
			}

			base.WndProc(ref m);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
#if __MonoCS__
			if (e.KeyChar == 27)
			{
				Visible = false;
			}
#endif
		}

		private void MainFrm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
#if __MonoCS__
			if (e.KeyCode == Keys.Escape)
			{
				Hide();
			}
#endif
		}

		protected override bool IsInputKey(Keys keyData)
		{
			return base.IsInputKey(keyData) || keyData == Keys.Escape;
		}

		private void searchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SearchFrm frm = new SearchFrm();

			if (DialogResult.OK == frm.ShowDialog(this))
			{
				SearchData(frm.FindWhat,
						frm.UseCreate,
						frm.CreateFrom, frm.CreateTo,
						frm.UseModify,
						frm.ModifyFrom, frm.ModifyTo);
			}
		}

		private void tvwDiary_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			HandleModifiedState();
		}

		private bool handlingModifiedState_ = false;
		private void HandleModifiedState()
		{
			if (handlingModifiedState_) return;

			handlingModifiedState_ = true;

			try 
			{
				if (DoesSaveModified())
				{
					if (tcNavigation.SelectedIndex == 0)
						SaveCurrentDiaryNote();
					else
						SaveCurrentDocument();
				}
				else
				{
					if (richTextBox.Modified)
					{
						richTextBox.Text = "";
						richTextBox.Modified = false;
					}

					if (IsAttachmentsModified())
					{
						UpdateAttachmentsListView(-1, true);
					}

					if (IsDocumentsNodeModified())
					{
						DocumentTreeNode node = tvwDocuments.SelectedNode as DocumentTreeNode;

						if (node != null)
						{
							node.Rollback();
						}
					}

					if (IsDiaryNodeModified())
					{
						DayTreeNode node = tvwDiary.SelectedNode as DayTreeNode;

						if (node != null)
						{
							node.Rollback();
						}
					}
				}
			}
			finally 
			{
				handlingModifiedState_ = false;
			}	
		}

		private bool IsDocumentsNodeModified()
		{
			if (tcNavigation.SelectedIndex == 1)
			{
				if (tvwDocuments.SelectedNode is DocumentTreeNode)
				{
					DocumentTreeNode node =
						tvwDocuments.SelectedNode as DocumentTreeNode;

					return node.Modified;
				}
			}

			return false;
		}

		private bool IsDiaryNodeModified()
		{
			if (tcNavigation.SelectedIndex == 0)
			{
				if (tvwDiary.SelectedNode is DayTreeNode)
				{
					DayTreeNode node =
						tvwDiary.SelectedNode as DayTreeNode;

					return node.Modified;
				}
			}

			return false;
		}

		private void findResultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			scMainOthers.Panel2Collapsed =
				!scMainOthers.Panel2Collapsed;

			UpdateViewMenu();
		}

		private void SearchData(string findWhat,
				bool useCreate, DateTime createFrom, DateTime createTo,
				bool useModify, DateTime modifyFrom, DateTime modifyTo)
		{
			ArrayList notesIds = new ArrayList();
			ArrayList documentsIds = new ArrayList();

			DBManager.FullTextSearch(Program.DbConnection,
					findWhat, notesIds, documentsIds);

			tvwFindResults.Nodes.Clear();

			AddNotesResult(notesIds, useCreate, createFrom, createTo,
					useModify, modifyFrom, modifyTo);
			AddDocumentsResult(documentsIds, useCreate, createFrom, createTo,
					useModify, modifyFrom, modifyTo);

			tvwFindResults.ExpandAll();

			tpFindResults.Text = "Find Results - " +
				(notesIds.Count + documentsIds.Count) +
				" found";

			scMainOthers.Panel2Collapsed = false;
			UpdateViewMenu();
		}

		private void AddNotesResult(ArrayList notesIds,
				bool useCreate, DateTime createFrom, DateTime createTo,
				bool useModify, DateTime modifyFrom, DateTime modifyTo)
		{
			if (notesIds.Count == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("ID in (");
			foreach (int id in notesIds)
			{
				sb.Append(id);
				sb.Append(",");
			}
			sb.Append("-1");
			sb.Append(")");

			DataView view = new DataView(Program.DiaryNetDS.DiaryNotes,
					sb.ToString(), "Note_Date", DataViewRowState.CurrentRows);

			if (view.Count == 0)
				return;

			TreeNode root = new TreeNode("Notes");

			tvwFindResults.Nodes.Add(root);

			foreach (DataRowView rowView in view)
			{
				bool result = true;
				DiaryNetDS.DiaryNotesRow row =
					rowView.Row as DiaryNetDS.DiaryNotesRow;

				if (useCreate)
				{
					result = row.Note_Date >= createFrom &&
						row.Note_Date <= createTo;
				}

				if (useModify)
				{
					result = row.Modify_Date >= modifyFrom &&
						row.Modify_Date <= modifyTo;
				}

				if (result)
				{
					AddNoteResultNode(root, row);
				}
			}
		}

		private void AddNoteResultNode(TreeNode root, DiaryNetDS.DiaryNotesRow row)
		{
			root.Nodes.Add(new NoteResultNode(row.Note_Date));
		}

		private void AddDocumentsResult(ArrayList documentsIds,
				bool useCreate, DateTime createFrom, DateTime createTo,
				bool useModify, DateTime modifyFrom, DateTime modifyTo)
		{
			if (documentsIds.Count == 0)
				return;

			StringBuilder sb = new StringBuilder();
			sb.Append("ID in (");
			foreach (int id in documentsIds)
			{
				sb.Append(id);
				sb.Append(",");
			}
			sb.Append("-1");
			sb.Append(")");

			DataView view = new DataView(Program.DiaryNetDS.Documents);
			view.Sort = "Create_Date";
			view.RowFilter = sb.ToString();
			view.RowStateFilter = DataViewRowState.CurrentRows;

			if (view.Count == 0)
				return;

			TreeNode root = new TreeNode("Documents");

			tvwFindResults.Nodes.Add(root);

			foreach (DataRowView rowView in view)
			{
				bool result = true;
				DiaryNetDS.DocumentsRow row =
					rowView.Row as DiaryNetDS.DocumentsRow;

				if (useCreate)
				{
					result = row.Create_Date >= createFrom &&
						row.Create_Date <= createTo;
				}

				if (useModify)
				{
					result = row.Modify_Date >= modifyFrom &&
						row.Modify_Date <= modifyTo;
				}

				if (result)
				{
					AddDocumentResultNode(root, row);
				}
			}
		}

		private void AddDocumentResultNode(TreeNode root, DiaryNetDS.DocumentsRow row)
		{
			root.Nodes.Add(new DocumentResultNode(row.Title, row.ID));
		}

		private void tvwFindResults_DoubleClick(object sender, EventArgs e)
		{
			if (tvwFindResults.SelectedNode is NoteResultNode)
			{
				NoteResultNode node = tvwFindResults.SelectedNode as NoteResultNode;

				ChangeToDate(node.Date);
			}
			else if (tvwFindResults.SelectedNode is DocumentResultNode)
			{
				DocumentResultNode node = tvwFindResults.SelectedNode as DocumentResultNode;

				foreach (DocumentTreeNode node1 in tvwDocuments.Nodes)
				{
					if (node1.ID == node.ID)
					{
						tcNavigation.SelectedIndex = 1;
						tvwDocuments.SelectedNode = node1;
						break;
					}
				}
			}
		}

		private void importNotesMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog2.Title = "Import From";

			if (openFileDialog2.ShowDialog(this) == DialogResult.OK)
			{
				ImportFrom(openFileDialog2.FileName);
			}
		}

		private void exportNotesMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.Title = "Export To";

			if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				ExportTo(saveFileDialog1.FileName);
			}
		}

		private void ExportTo(string filename)
		{
			HandleModifiedState();

			try
			{
				DiaryNetDS ds = new DiaryNetDS();

				DbDataAdapter adapter =
					DBManager.CreateDiaryDataAdapter(Program.DbProvideFactory, Program.DbConnection);

				adapter.Fill(ds.DiaryNotes);

				adapter =
					DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory, Program.DbConnection);
				adapter.Fill(ds.Attachments);

				adapter =
					DBManager.CreateDocumentsDataAdapter(Program.DbProvideFactory, Program.DbConnection);
				adapter.Fill(ds.Documents);

				adapter =
					DBManager.CreateDataAdapter(Program.DbProvideFactory, Program.DbConnection, "Content_Text");
				adapter.Fill(ds.Content_Text);

				adapter =
					DBManager.CreateDataAdapter(Program.DbProvideFactory, Program.DbConnection, "Content_Binary");
				adapter.Fill(ds.Content_Binary);

				using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					ds.WriteXml(fs);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ImportFrom(string filename)
		{
			if (!File.Exists(filename))
				return;

			HandleModifiedState();

			using (DbTransaction dbTrans = Program.DbConnection.BeginTransaction())
			{
				Program.DiaryNetDS.AcceptChanges();

				try
				{
					DiaryNetDS ds = new DiaryNetDS();

					using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
					{
						ds.ReadXml(fs);
					}

					foreach (DiaryNetDS.DiaryNotesRow row in ds.DiaryNotes.Rows)
					{
						MergeDiaryNote(ds, row);
					}

					foreach (DiaryNetDS.DocumentsRow row in ds.Documents.Rows)
					{
						MergeDocument(ds, row);
					}

					DBManager.CreateDiaryDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.DiaryNotes);
					DBManager.CreateAttachmentsDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.Attachments);
					DBManager.CreateDocumentsDataAdapter(Program.DbProvideFactory,
							Program.DbConnection).Update(Program.DiaryNetDS.Documents);
					dbTrans.Commit();
				}
				catch (Exception ex)
				{
					dbTrans.Rollback();
					Program.DiaryNetDS.RejectChanges();
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}//try
			}//using trans

			InitializeData();
		}

		private void MergeDiaryNote(DiaryNetDS mergeDS, DiaryNetDS.DiaryNotesRow mergeRow)
		{
			DiaryNetDS.DiaryNotesRow row = null;

			DiaryNetDS.Content_BinaryRow bRow =
				mergeDS.Content_Binary.FindByID(mergeRow.Binary_ID);
			DiaryNetDS.Content_TextRow tRow =
				mergeDS.Content_Text.FindByID(mergeRow.Text_ID);

			string bRowContent = string.Empty;
			string tRowContent = string.Empty;

			if (bRow != null) bRowContent = Program.GetEncoding().GetString(bRow.Content);
			if (tRow != null) tRowContent = tRow.Content;

			var q = from Note in Program.DiaryNetDS.DiaryNotes
				where Note.Note_Date == mergeRow.Note_Date
				select Note;

			IEnumerator<DiaryNetDS.DiaryNotesRow> it = q.GetEnumerator();

			if (it.MoveNext())
			{
				row = it.Current;
			}

			bool bNew = false;

			if (row == null)
			{
				row = Program.DiaryNetDS.DiaryNotes.NewDiaryNotesRow();
				bNew = true;
			}
			else
			{
				row.BeginEdit();
			}

			row.Modify_Date = DateTime.Now;

			if (bNew)
			{
				row.Binary_ID = -1;
				row.Text_ID = -1;
				row.Note_Date = mergeRow.Note_Date;
				row.Binary_ID =
					DBManager.SaveBinary(Program.DbConnection,
							row.Binary_ID,
							bRowContent);
				row.Text_ID = DBManager.SaveText(Program.DbConnection,
						row.Text_ID,
						row.ID,
						false,
						"",
						tRowContent);

				Program.DiaryNetDS.DiaryNotes.AddDiaryNotesRow(row);
			}
			else
			{
				RichTextBox rch = new RichTextBox();

				rch.Rtf = DBManager.GetBinary(Program.DbConnection,
						row.Binary_ID);

				if (string.Compare(rch.Rtf, bRowContent) != 0)
				{
					rch.Select(rch.Text.Length, 0);
					rch.SelectedText =
						string.Format("\r\n\r\n---------Merged Data Begin {0} ------\r\n\r\n",
								DateTime.Now);
					rch.SelectedRtf = bRowContent;
					rch.Select(rch.Text.Length, 0);
					rch.SelectedText =
						string.Format("\r\n\r\n---------Merged Data End {0} ------\r\n\r\n",
								DateTime.Now);

					row.Binary_ID =
						DBManager.SaveBinary(Program.DbConnection,
								row.Binary_ID,
								rch.Rtf);
					row.Text_ID = DBManager.SaveText(Program.DbConnection,
							row.Text_ID,
							row.ID,
							false,
							"",
							rch.Text);
				}
				row.EndEdit();
			}

			//Merge all attachments
			MergeAllAttachments(mergeDS, row.ID, mergeRow.ID, true);

		}//MergeDiaryNote

		private void MergeDocument(DiaryNetDS mergeDS, DiaryNetDS.DocumentsRow mergeRow)
		{
			DiaryNetDS.Content_BinaryRow bRow =
				mergeDS.Content_Binary.FindByID(mergeRow.Binary_ID);
			DiaryNetDS.Content_TextRow tRow =
				mergeDS.Content_Text.FindByID(mergeRow.Text_ID);

			string bRowContent = string.Empty;
			string tRowContent = string.Empty;

			if (bRow != null) bRowContent = Program.GetEncoding().GetString(bRow.Content);
			if (tRow != null) tRowContent = tRow.Content;

			var q = from document in Program.DiaryNetDS.Documents
				where document.Title == mergeRow.Title
				select document;

			IEnumerator<DiaryNetDS.DocumentsRow> it = q.GetEnumerator();

			bool createNew = true;
			DiaryNetDS.DocumentsRow row = null;

			while (it.MoveNext())
			{
				string curBinaryData =
					DBManager.GetBinary(Program.DbConnection,
							it.Current.Binary_ID);

				if (string.Compare(curBinaryData, bRowContent) == 0)
				{
					createNew = false;
					row = it.Current;
					break;
				}
			}

			if (createNew)
			{
				row =
					Program.DiaryNetDS.Documents.NewDocumentsRow();

				row.Binary_ID = -1;
				row.Text_ID = -1;
				row.Title = mergeRow.Title;
				row.Create_Date = mergeRow.Create_Date;
				row.Modify_Date = mergeRow.Modify_Date;

				row.Binary_ID =
					DBManager.SaveBinary(Program.DbConnection,
							row.Binary_ID,
							bRowContent);

				row.Text_ID = DBManager.SaveText(Program.DbConnection,
						row.Text_ID,
						row.ID,
						true,
						mergeRow.Title,
						tRowContent);

				Program.DiaryNetDS.Documents.AddDocumentsRow(row);
			}
			else
			{
				row.BeginEdit();
				row.Modify_Date = DateTime.Now;
				row.EndEdit();
			}

			//Merge all attachments
			MergeAllAttachments(mergeDS, row.ID, mergeRow.ID, false);

		}//merge documents

		private void MergeAllAttachments(DiaryNetDS mergeDS, int refId, int mergeRefId, bool isNotes)
		{
			var qq = from arow in mergeDS.Attachments
				where arow.Ref_ID == mergeRefId &&
				arow.Is_Notes == (isNotes ? '1':'0')
				select arow;

			IEnumerator<DiaryNetDS.AttachmentsRow> itt = qq.GetEnumerator();

			while (itt.MoveNext())
			{
				DiaryNetDS.AttachmentsRow mergeAttachRow = itt.Current;

				DiaryNetDS.Content_BinaryRow bbRow =
					mergeDS.Content_Binary.FindByID(mergeAttachRow.Binary_ID);

				if (bbRow == null)
					continue;

				var attachQ = from tmpRow in Program.DiaryNetDS.Attachments
					where string.Compare(tmpRow.FileName, mergeAttachRow.FileName) == 0
					&& tmpRow.Is_Notes == (isNotes ? '1' : '0')
					select tmpRow;

				bool createNew = true;

				string bbRowContent = 
					Program.GetEncoding().GetString(bbRow.Content);

				IEnumerator<DiaryNetDS.AttachmentsRow> attachIt = attachQ.GetEnumerator();

				while (attachIt.MoveNext())
				{
					string binary = DBManager.GetBinary(Program.DbConnection,
							attachIt.Current.Binary_ID);

					if (string.Compare(binary, bbRowContent) == 0)
					{
						createNew = false;
						break;
					}
				}

				if (!createNew)
					continue;

				DiaryNetDS.AttachmentsRow attachRow =
					Program.DiaryNetDS.Attachments.AddAttachmentsRow(isNotes ? '1' : '0',
							refId, mergeAttachRow.FileName, -1);

				attachRow.BeginEdit();
				attachRow.Binary_ID =
					DBManager.SaveBinary(Program.DbConnection, -1,
							bbRowContent);
				attachRow.EndEdit();
			}
		}
	}//class
}//namespace

