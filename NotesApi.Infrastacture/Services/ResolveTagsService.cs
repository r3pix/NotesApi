using NotesApi.Domain.ValueObject;
using NotesApi.Infrastacture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotesApi.Infrastacture.Services
{
    public class ResolveTagsService : IResolveTagsService
    {
        public ResolveTagsService()
        {
            _phoneRegex = new Regex(PhoneTemplate);
            _emailRegex = new Regex(EmailTemplate);
        }

        private readonly Regex _phoneRegex;
        private readonly Regex _emailRegex;

        private string PhoneTemplate => @"\b(?:\+\d{2}\s?)?(?:[1-9]\d{8})?\b|\b(?:\+\d{2}\s?)?[1-9]\d{2}\s?\d{3}\s?\d{3}\b|\b(?:\+\d{2}\s?)?[1-9]\d{2}-\d{3}-\d{3}\b";

        private string EmailTemplate => @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";

        public IEnumerable<TagTypes> ResolveTags(string content)
        {
            var tags = new List<TagTypes>();

            if (_phoneRegex.IsMatch(content))
                tags.Add(TagTypes.Phone);

            if (_emailRegex.IsMatch(content))
                tags.Add(TagTypes.Email);

            if (tags.Count == 0)
                tags.Add(TagTypes.None);

            return tags;
        }
    }
}
