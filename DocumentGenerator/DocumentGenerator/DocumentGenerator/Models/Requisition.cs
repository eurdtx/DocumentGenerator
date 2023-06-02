using System;

namespace DocumentGenerator.Models
{
  public class Requisition
  {
    private string _patientFirstName = string.Empty;
    private string _patientLastName = string.Empty;
    private string _patientMiddleName = string.Empty;
    private string _clientPatientID = string.Empty;
    private string _physicianFirstName = string.Empty;
    private string _physicianLastName = string.Empty;
    private string _physicianMiddleName = string.Empty;
    private string _patientGender = "N/A";
    private int _patientAgeInYears = 0;
    private DateTime _patientDOB = new DateTime(1819, 12, 14);
    private string _physicianPhone = string.Empty;
    private string _specimenSource = string.Empty;
    private string _otherSpecimenSourceSpecified = string.Empty;
    private string _hasCollectionDateInd = "N";
    private string _hasCollectionTimeInd = "N";
    private DateTime _specimenCollectionDate = new DateTime(1819, 12, 14);
    private string _clientName = string.Empty;
    private DateTime _specimenReceiveDate = new DateTime(1819, 12, 14);
    private long _trlClientID = 0;
    private string _specimenID = string.Empty;
    private long _accessionID = 0;
    private DateTime _releaseDate = new DateTime(1819, 12, 14);
    private string _clientAddress = string.Empty;
    private string _clientCityPlusState = string.Empty;
    private string _clientZIP = string.Empty;

    public string PatientFirstName
    {
      get { return _patientFirstName; }
      set { _patientFirstName = value; }
    }

    public string PatientLastName
    {
      get { return _patientLastName; }
      set { _patientLastName = value; }
    }

    public string PatientMiddleName
    {
      get { return _patientMiddleName; }
      set { _patientMiddleName = value; }
    }

    public string ClientPatientID
    {
      get { return _clientPatientID; }
      set { _clientPatientID = value; }
    }

    public string PhysicianFirstName
    {
      get { return _physicianFirstName; }
      set { _physicianFirstName = value; }
    }

    public string PhysicianLastName
    {
      get { return _physicianLastName; }
      set { _physicianLastName = value; }
    }

    public string PhysicianMiddleName
    {
      get { return _physicianMiddleName; }
      set { _physicianMiddleName = value; }
    }

    /// <summary>
    /// M=Male, F=Female, N=N/A 
    /// </summary>    
    public string PatientGenderInd
    {
      get
      {
        string temp = "M";
        if (PatientGender.Length > 0)
          temp = PatientGender.Substring(0, 1);
        return temp;
      }
    }

    /// <summary>
    /// Male, Female, or N/A [default]
    /// see also PatientGenderInd
    /// </summary>
    public string PatientGender
    {
      get { return this._patientGender; }
      set
      {
        string temp = value.Trim();
        if (temp.Length == 0)
          temp = "N/A";
        else if (temp.Length == 1)
        {
          if (temp.Equals("M"))
            temp = "Male";
          else if (temp.Equals("F"))
            temp = "Female";
          else if (temp.Equals("U"))
            temp = "Unknown";
          else
            temp = "N/A";
        }
        _patientGender = temp;
      }
    }

    public int PatientAgeInYears
    {
      get { return _patientAgeInYears; }
      set { _patientAgeInYears = value; }
    }

    public DateTime PatientDOB
    {
      get { return _patientDOB; }
      set { _patientDOB = value; }
    }

    public string PhysicianPhone
    {
      get { return _physicianPhone; }
      set { _physicianPhone = value; }
    }

    public string SpecimenSource
    {
      get { return _specimenSource; }
      set { _specimenSource = value; }
    }

    public string OtherSpecimenSourceSpecified
    {
      get { return _otherSpecimenSourceSpecified; }
      set { _otherSpecimenSourceSpecified = value; }
    }

    /// <summary>
    /// Gets/Sets Y if specimen collection date has been set,else returns N (default)
    /// </summary>
    public string HasCollectionDateInd
    {
      get { return this._hasCollectionDateInd; }
      set
      {
        string temp = value.Trim().ToUpper();
        if (temp.Equals("Y"))
          this._hasCollectionDateInd = "Y";
        else
          this._hasCollectionDateInd = "N";
      }
    }

    /// <summary>
    /// Gets/Sets Y if specimen collection time has been set,else returns N (default)
    /// </summary>
    public string HasCollectionTimeInd
    {
      get { return this._hasCollectionTimeInd; }
      set
      {
        string temp = value.Trim().ToUpper();
        if (temp.Equals("Y"))
          this._hasCollectionTimeInd = "Y";
        else
          this._hasCollectionTimeInd = "N";
      }
    }

    public DateTime SpecimenCollectionDate
    {
      get { return _specimenCollectionDate; }
      set { _specimenCollectionDate = value; }
    }

    public string ClientName
    {
      get { return _clientName; }
      set { _clientName = value; }
    }

    public DateTime SpecimenReceiveDate
    {
      get { return _specimenReceiveDate; }
      set { _specimenReceiveDate = value; }
    }

    public long TRLClientID
    {
      get { return _trlClientID; }
      set { _trlClientID = value; }
    }

    public string SpecimenID
    {
      get { return _specimenID; }
      set { _specimenID = value; }
    }

    public long AccessionID
    {
      get { return _accessionID; }
      set { _accessionID = value; }
    }

    public DateTime ReleaseDate
    {
      get { return _releaseDate; }
      set { _releaseDate = value; }
    }

    public string ClientAddress
    {
      get { return _clientAddress; }
      set { _clientAddress = value; }
    }

    public string ClientCityPlusState
    {
      get { return _clientCityPlusState; }
      set { _clientCityPlusState = value; }
    }

    public string ClientZIP
    {
      get { return _clientZIP; }
      set { _clientZIP = value; }
    }
  }
}
