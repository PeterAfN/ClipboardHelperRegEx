using ClipboardHelperRegEx.Properties;
using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class ValidateXml
    {
        public enum XmlType { Auto, Manual }

        public static bool Passed { get; private set; } = true;

        /// <summary>
        /// Controls if a Xml file of type Auto or Manual is valid.
        /// If valid, Passed is true.
        /// If not valid, Passed is false.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="xmlType"></param>
        public ValidateXml(string file, XmlType xmlType)
        {
            var schemaSet = new XmlSchemaSet();
            switch (xmlType)
            {
                case XmlType.Auto:
                    schemaSet.Add(null, "AutoXmlSchema.xsd");
                    break;
                case XmlType.Manual:
                    schemaSet.Add(null, "ManualXmlSchema.xsd");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(xmlType), xmlType, null);
            }
            if (file != null) Validate(file, schemaSet);
        }

        private static void Validate(string filename, XmlSchemaSet schemaSet)
        {
            XmlSchema compiledSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
                compiledSchema = schema;

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(compiledSchema ?? throw new InvalidOperationException());
            settings.ValidationEventHandler += ValidationCallBack;
            settings.ValidationType = ValidationType.Schema;
            var vReader = XmlReader.Create(filename, settings);
            try
            {
                while (vReader.Read()) { }
            }
            catch (Exception e)
            {
                Passed = false;
                MessageBox.Show(Resources.ValidateXml_Validate_ +
                                e.Message, Resources.ValidateXml_Validate_Clipboard_Helper_Xml_file_validation_failed, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                vReader.Close();
            }
        }

        //Display any warnings or errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                Passed = false;
                MessageBox.Show(Resources.ValidateXml_Validate_ +
                                args.Message, Resources.ValidateXml_Validate_Clipboard_Helper_Xml_file_validation_failed, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Passed = false;
                MessageBox.Show(Resources.ValidateXml_Validate_ +
                                args.Message, Resources.ValidateXml_Validate_Clipboard_Helper_Xml_file_validation_failed, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
