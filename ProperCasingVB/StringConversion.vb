Option Explicit On
Option Compare Binary
Option Strict On

''' <summary>
''' String Utility (VB)
''' </summary>
''' <remarks></remarks>
Public Class StringConversion
	''' <summary>
	''' Convert value to Proper-case.
	''' </summary>
	''' <param name="inputString"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ProperCase(ByVal inputString As String) As String
		Return StrConv(inputString, VbStrConv.ProperCase)
	End Function
End Class
